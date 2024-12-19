using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Ui.Console;

var builder = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        // TODO add your service dependencies here
        services.AddSingleton<App>();
    });
var app = builder.Build();
return await app.Services.GetRequiredService<App>().StartAsync(Environment.GetCommandLineArgs());