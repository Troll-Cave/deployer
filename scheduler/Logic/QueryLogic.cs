using System.IO.Compression;
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
            .Where(x => x.JobState == "ready")
            .ToListAsync(stoppingToken);

        foreach (var job in jobs)
        {
            await ProcessJob(job);
        }
        
        var pendingJobs = await _deployerContext.Jobs
            .Where(x => x.JobState == "pending")
            .ToListAsync(stoppingToken);
        
        foreach (var job in pendingJobs)
        {
            await ProcessPending(job);
        }
    }

    private async Task ProcessPending(JobDTO job)
    {
        var tempArtifactLocation = Worker.GetCacheDir($"temp{Guid.NewGuid()}.zip");
        var tempExtractLocation = Worker.GetCacheDir($"temp{Guid.NewGuid()}");

        Console.WriteLine($"Pulling job {job.ID}");

        var app = await _deployerContext.Applications.FindAsync(job.ApplicationId);

        if (app == null)
        {
            throw new Exception("how is this possible?");
        }

        var source = app.Source;
        var artifactLocation = Worker.GetCacheDir($"{job.ID}.zip");

        var package = await _github.GetReference(app.Source, job.SourceReference);
        
        // Write to temp location
        await File.WriteAllBytesAsync(tempArtifactLocation, package);
        
        // Extract to temp location
        ZipFile.ExtractToDirectory(tempArtifactLocation, tempExtractLocation);
        
        // Get actually artifact folder
        var artifactFolder = Directory.GetDirectories(tempExtractLocation)[0];
        
        // Compress to job artifact
        ZipFile.CreateFromDirectory(artifactFolder, artifactLocation);
        
        // cleanup on aisle 5
        Directory.Delete(tempExtractLocation, true);
        File.Delete(tempArtifactLocation);

        job.Code.Flow.ForEach(x =>
        {
            // set parentless steps to ready
            job.StepState[x.Step] = x.DependsOn.Any() ? "pending" : "ready";
        });

        job.JobState = "ready";
        await _deployerContext.SaveChangesAsync();
    }

    private async Task ProcessJob(JobDTO job)
    {
        Console.WriteLine($"Processing job {job.ID}");
        var readySteps = job.StepState.Where(x => x.Value == "ready").ToList();

        if (!readySteps.Any())
        {
            if (job.StepState.Any(x => x.Value != "done"))
            {
                // basically in this case there's an error or something weird
                job.JobState = "waiting";
            }
            else
            {
                // yay!
                job.JobState = "done";
            }
            await _deployerContext.SaveChangesAsync();
            return;
        }

        job.JobState = "working";
        await _deployerContext.SaveChangesAsync();

        foreach (var step in readySteps)
        {
            try
            {
                await ProcessStep(job, step.Key);
            }
            catch (Exception e)
            {
                job.JobState = "error";
                job.StepState[step.Key] = "error";
                await _deployerContext.SaveChangesAsync();
            }
        }
    }
    
    /// <summary>
    /// Actually do work lol (this is where in the future this will use nomad)
    /// </summary>
    /// <param name="job"></param>
    /// <param name="stepKey"></param>
    private async Task ProcessStep(JobDTO job, string stepKey)
    {
        var workDirectory = Worker.GetCacheDir($"temp{Guid.NewGuid()}");
        var artifactLocation = Worker.GetCacheDir($"{job.ID}.zip");
        ZipFile.ExtractToDirectory(artifactLocation, workDirectory);
        
        // Do work
        
        // Wrap it up
        File.Delete(artifactLocation);
        ZipFile.CreateFromDirectory(workDirectory, artifactLocation);
        Directory.Delete(workDirectory, true);
        
        job.StepState[stepKey] = "done";
        await _deployerContext.SaveChangesAsync();
    }
}