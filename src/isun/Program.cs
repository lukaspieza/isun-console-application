using isun.Domain.Implementations;
using isun.Domain.Interfaces;
using isun.Domain.Interfaces.Infrastructure;
using isun.Infrastructure.Implementations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NLog.Web;

var host = CreateHostBuilder(args).Build();
host.Services.GetRequiredService<IWeatherForecastService>().ShowAndSaveWeatherForecast(args);

static IHostBuilder CreateHostBuilder(string[] args)
{
    var hostBuilder = Host.CreateDefaultBuilder(args)
        .ConfigureAppConfiguration((context, builder) =>
        {
            builder.SetBasePath(Directory.GetCurrentDirectory());
        })
        .ConfigureServices((context, services) =>
        {
            services.AddTransient<IWeatherForecastService, WeatherForecastService>();
            services.AddTransient<ICitiesProvider, CitiesProvider>();
            services.AddTransient<IArgumentsOperationsProvider, ArgumentsOperationsProvider>();
        })
        .UseNLog();

    return hostBuilder;
}
