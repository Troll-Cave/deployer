using data;
using scheduler;
using scheduler.Logic;
using shared.Services;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddDbContext<DeployerContext>();
        services.AddTransient<QueryLogic>();
        services.AddTransient<Github>();
        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();
