using DuplicatesRemover.Data;
using DuplicatesRemover.MainService;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.AddSingleton<IMainService, MainService>();
        services.AddSingleton<SalesService>();
    })
    .Build();


var myService = host.Services.GetRequiredService<IMainService>();
await myService.Run();

