using data;
using scheduler;
using scheduler.Logic;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddDbContext<DeployerContext>();
        services.AddTransient<QueryLogic>();
        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();
