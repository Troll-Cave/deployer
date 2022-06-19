using data;
using scheduler.Logic;

namespace scheduler;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IServiceProvider _provider;
    
    public static string GetCacheDir(string fileName = "")
    {
        if (string.IsNullOrWhiteSpace(fileName))
        {
            return Path.Join(Directory.GetCurrentDirectory(), ".cache");    
        }
        else
        {
            return Path.Join(Directory.GetCurrentDirectory(), ".cache", fileName);
        }
    }

    public Worker(ILogger<Worker> logger, IServiceProvider provider)
    {
        _logger = logger;
        _provider = provider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            if (!Directory.Exists(Worker.GetCacheDir()))
            {
                Directory.CreateDirectory(Worker.GetCacheDir());
            }
            
            // _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            //Console.WriteLine(Directory.GetCurrentDirectory());

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
