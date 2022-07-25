using isun.Implementations;
using isun.Interfaces;
using isun.Models;
using isun.Startup;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;

try
{
    var host = CreateHostBuilder(args).Build();
    var backgroundTimer = host.Services.GetRequiredService<IBackgroundTimer>();
    backgroundTimer.Arguments = args;
    backgroundTimer.Start();
    Console.ReadKey(true);
    await backgroundTimer.StopAsync();
}
catch (Exception e)
{
    Console.WriteLine(e);
    Console.ReadKey(true);
}

static IHostBuilder CreateHostBuilder(string[] args)
{
    var hostBuilder = Host.CreateDefaultBuilder(args)
        .ConfigureAppConfiguration((_, builder) =>
        {
            builder.SetBasePath(Directory.GetCurrentDirectory());
            builder.AddJsonFile("appsettings.json", false, false);
        })
        .ConfigureServices((context, services) =>
        {
            services.Configure<BackgroundTimerOptions>(options => context.Configuration.GetSection("BackgroundTimerOptions").Bind(options));
            services.AddTransient<IBackgroundTimer, BackgroundTimer>();
            services.AddTransient<IConsoleProvider, ConsoleProvider>();
            services.AddLogging(builder => builder.ClearProviders());
            services.ConfigureSun(context.Configuration);
        })
        .UseNLog();

    return hostBuilder;
}
