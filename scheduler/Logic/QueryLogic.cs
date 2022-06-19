using data;
using data.DataModels;
using Microsoft.EntityFrameworkCore;

namespace scheduler.Logic;

public class QueryLogic
{
    private readonly DeployerContext _deployerContext;

    public QueryLogic(DeployerContext deployerContext)
    {
        _deployerContext = deployerContext;
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
    }

    private async Task ProcessJob(JobDTO job)
    {
        throw new NotImplementedException();
    }
}