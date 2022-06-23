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
        Console.WriteLine($"Pulling job {job.ID}");

        var app = await _deployerContext.Applications.FindAsync(job.ApplicationId);

        if (app == null)
        {
            throw new Exception("how is this possible?");
        }

        var source = app.Source;
        var artifactLocation = Worker.GetCacheDir($"{job.ID}.zip");

        var package = await _github.GetReference(app.Source, job.SourceReference);
        
        await File.WriteAllBytesAsync(artifactLocation, package);
        
        job.Code.Flow.ForEach(x =>
        {
            // set parentless steps to ready
            job.StepState[x.Step] = x.DependsOn.Any() ? "pending" : "ready";
        });

        job.JobState = "ready";
        await _deployerContext.SaveChangesAsync();
        
        // ZipFile.ExtractToDirectory(Worker.GetCacheDir($"{job.ID}.zip"), Worker.GetCacheDir("a"));
        // ZipFile.CreateFromDirectory(Worker.GetCacheDir("a"), Worker.GetCacheDir($"b.zip"));
        // ZipFile.ExtractToDirectory(Worker.GetCacheDir("b.zip"), Worker.GetCacheDir("c"));
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
        job.StepState[stepKey] = "done";
        await _deployerContext.SaveChangesAsync();
    }
}