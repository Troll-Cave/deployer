using data;
using Microsoft.EntityFrameworkCore;
using scheduler.Logic;

namespace scheduler;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IServiceProvider _provider;
    private readonly DeployerContext _deployerContext;

    public Worker(ILogger<Worker> logger, IServiceProvider provider)
    {
        _logger = logger;
        _provider = provider;
        _deployerContext = new DeployerContext();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

            // This is a gigantic pain
            await using (var scope = _provider.CreateAsyncScope())
            {
                var logic = scope.ServiceProvider.GetRequiredService<QueryLogic>();
                await logic.RunQueries(stoppingToken);
            }
            
            await Task.Delay(1000, stoppingToken);
        }
    }
}
