using Logic.Core;
using Logic.Interfaces.Logic;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Ui.Console;

var builder = Host.CreateDefaultBuilder(args)
    .ConfigureServices(
        (_, services) =>
        {
            // TODO add your service dependencies here
            services.AddSingleton<App>();
            services.AddTransient<IOperatingSystemLogic, OperatingSystemLogic>();
            services.AddTransient<IPythonDetectionLogic, PythonDetectionLogic>();
        });
var app = builder.Build();
return await app.Services.GetRequiredService<App>()
    .StartAsync(Environment.GetCommandLineArgs());