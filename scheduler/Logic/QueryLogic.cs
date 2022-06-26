using System.Diagnostics;
using System.IO.Compression;
using System.Text;
using System.Text.Json.Nodes;
using data;
using data.DataModels;
using Microsoft.EntityFrameworkCore;
using shared.Services;

namespace scheduler.Logic;

public class QueryLogic
{
    private readonly DeployerContext _deployerContext;
    private readonly IConfiguration _configuration;
    private readonly Github _github;

    public QueryLogic(DeployerContext deployerContext, IConfiguration configuration, Github github)
    {
        _deployerContext = deployerContext;
        _configuration = configuration;
        _github = github;
    }
    
    public async Task RunQueries(CancellationToken stoppingToken)
    {
        var jobs = await _deployerContext.Jobs
            .Where(x => x.State.State == "ready")
            .ToListAsync(stoppingToken);

        foreach (var job in jobs)
        {
            await ProcessJob(job);
        }
        
        var pendingJobs = await _deployerContext.Jobs
            .Where(x => x.State.State == "pending")
            .ToListAsync(stoppingToken);
        
        foreach (var job in pendingJobs)
        {
            await ProcessPending(job);
        }
    }

    private async Task ProcessPending(JobDTO job)
    {
        // IMPORTANT: When you modify the state, you need to manually set the dirty flag
        _deployerContext.Entry(job).State = EntityState.Modified;
        
        var tempArtifactLocation = Worker.GetCacheDir($"temp{Guid.NewGuid()}.zip");
        var tempExtractLocation = Worker.GetCacheDir($"temp{Guid.NewGuid()}");

        Console.WriteLine($"Pulling job {job.ID}");

        var app = await _deployerContext.Applications.FindAsync(job.ApplicationId);

        if (app == null)
        {
            throw new Exception("how is this possible?");
        }

        var version = await _deployerContext.PipelineVersions.FirstAsync(x => x.ID == job.PipelineVersionId);
        
        var replacements = new Dictionary<string, string>();
        
        foreach (var variable in job.Code.Variables)
        {
            var v = app.Variables.Get(variable.Name);
            if (v == null)
            {
                Console.WriteLine($"variable {variable.Name} doesn't exist in app");
                job.State.State = "error";
                await _deployerContext.SaveChangesAsync();
                return;
            }
            
            replacements.Add($"variables.{variable.Name}", v);
        }

        var source = app.Source;
        var artifactLocation = Worker.GetCacheDir($"{job.ID}.zip");

        var package = await _github.GetReference(app.Source, job.SourceReference);
        
        // Write to temp location
        await File.WriteAllBytesAsync(tempArtifactLocation, package);
        
        // Extract to temp location
        ZipFile.ExtractToDirectory(tempArtifactLocation, tempExtractLocation);
        
        // Get actual artifact folder
        var artifactFolder = Directory.GetDirectories(tempExtractLocation)[0];
        
        // do file adds
        foreach (var file in job.Code.Files)
        {
            var fileBytes = Convert.FromBase64String(version.Files.Files[file.Name]);

            if (file.IsBinary)
            {
                // we don't do replacements on binary files
                await File.WriteAllBytesAsync(
                    Path.Join(artifactFolder, file.Location),
                    fileBytes);
                
                continue;
            }

            var fileContent = Encoding.UTF8.GetString(fileBytes);
            await File.WriteAllTextAsync(
                Path.Join(artifactFolder, file.Location), 
                ProcessTemplate(fileContent, replacements));
        }
        
        // Compress to job artifact
        ZipFile.CreateFromDirectory(artifactFolder, artifactLocation);
        
        // cleanup on aisle 5
        Directory.Delete(tempExtractLocation, true);
        File.Delete(tempArtifactLocation);

        job.Code.Flow.ForEach(x =>
        {
            Console.WriteLine("flow here");
            // set parentless steps to ready
            job.State.StepState[x.Step] = x.DependsOn.Any() ? "pending" : "ready";
        });

        job.State.State = "ready";
        await _deployerContext.SaveChangesAsync();
    }

    private async Task ProcessJob(JobDTO job)
    {
        // so that it'll save
        _deployerContext.Entry(job).State = EntityState.Modified;
        
        Console.WriteLine($"Processing job {job.ID}");
        var readySteps = job.State.StepState.Where(x => x.Value == "ready").ToList();
        
        if (!readySteps.Any())
        {
            if (job.State.StepState.Any(x => x.Value != "done"))
            {
                // basically in this case there's an error or something weird
                job.State.State = "waiting";
            }
            else
            {
                // yay!
                job.State.State = "done";
            }
            await _deployerContext.SaveChangesAsync();
            return;
        }
        
        job.State.State = "working";
        await _deployerContext.SaveChangesAsync();
        
        foreach (var step in readySteps)
        {
            try
            {
                await ProcessStep(job, step.Key);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                job.State.State = "error";
                job.State.StepState[step.Key] = "error";
                await _deployerContext.SaveChangesAsync();
            }
        }

        // reset to "ready" to process more steps
        job.State.State = "ready";
        
        _deployerContext.Entry(job).State = EntityState.Modified;
        await _deployerContext.SaveChangesAsync();
    }
    
    /// <summary>
    /// Actually do work lol (this is where in the future this will use nomad)
    /// </summary>
    /// <param name="job"></param>
    /// <param name="stepKey"></param>
    private async Task ProcessStep(JobDTO job, string stepKey)
    {
        _deployerContext.Entry(job).State = EntityState.Modified;

        var step = job.Code.Steps.First(x => x.Name == stepKey);
        var flow = job.Code.Flow.First(x => x.Step == stepKey);

        var app = await _deployerContext.Applications.FirstAsync(x => x.ID == job.ApplicationId);
        
        var replacements = new Dictionary<string, string>();
        
        foreach (var variable in job.Code.Variables)
        {
            var v = app.Variables.Get(variable.Name);
            if (v == null)
            {
                Console.WriteLine($"variable {variable.Name} doesn't exist in app");
                job.State.State = "error";
                job.State.StepState[stepKey] = "error";
                await _deployerContext.SaveChangesAsync();
                return;
            }
            
            replacements.Add($"variables.{variable.Name}", v);
        }
        
        foreach (var local in flow.Locals)
        {
            replacements.Add($"locals.{local.Key}", ProcessTemplate(local.Value, replacements));
        }
        
        Console.WriteLine($"Running step {stepKey} from job ${job.ID}");
        var workDirectory = Worker.GetCacheDir($"temp{Guid.NewGuid()}");
        var artifactLocation = Worker.GetCacheDir($"{job.ID}.zip");

        foreach (var action in step.Actions)
        {
            var commandLocation = Worker.GetCacheDir($"{Guid.NewGuid()}.sh");

            await File.WriteAllTextAsync(commandLocation, ProcessTemplate(action.Command, replacements));
        
            ZipFile.ExtractToDirectory(artifactLocation, workDirectory);

            using Process process = new Process();

            // TODO, have multiple terminal emulator support
            process.StartInfo.FileName = "bash";
            process.StartInfo.Arguments = commandLocation;
            process.StartInfo.WorkingDirectory = workDirectory;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.UseShellExecute = false;

            process.Start();
            await process.WaitForExitAsync();
            Console.WriteLine(process.StandardOutput.ReadToEnd());
            File.Delete(commandLocation);
        }
        
        // Wrap it up
        File.Delete(artifactLocation);
        ZipFile.CreateFromDirectory(workDirectory, artifactLocation);
        Directory.Delete(workDirectory, true);
        
        job.State.StepState[stepKey] = "done";
        // TODO: mark dependant steps done here

        var dependantSteps = job
            .Code
            .Flow
            .Where(x => x.DependsOn.Contains(stepKey))
            .Select(x => x.Step);

        foreach (var dependantStep in dependantSteps)
        {
            job.State.StepState[dependantStep] = "ready";
        }
        
        await _deployerContext.SaveChangesAsync();
    }

    private string ProcessTemplate(string template, Dictionary<string, string> replacements)
    {
        foreach (var replacement in replacements)
        {
            template = template.Replace($"${{{replacement.Key}}}", replacement.Value);
        }

        return template;
    }
}