using isun.Domain.Implementations;
using isun.Domain.Interfaces;
using isun.Domain.Interfaces.Infrastructure;
using isun.Domain.Models.Options;
using isun.Infrastructure.Implementations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NLog.Web;

var host = CreateHostBuilder(args).Build();
host.Services.GetRequiredService<IWeatherForecastService>().GetWeatherForecast(args);

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
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.Configure<AppOptions>(options => context.Configuration.GetSection("AppOptions").Bind(options));
            services.AddTransient<IWeatherForecastProvider, WeatherForecastProvider>();
            services.AddTransient<IWeatherForecastService, WeatherForecastService>();
            services.AddTransient<ICitiesProvider, CitiesProvider>();
            services.AddTransient<IArgumentsOperationsProvider, ArgumentsOperationsProvider>();
            services.AddTransient<IConsoleProvider, ConsoleProvider>();
            services.AddTransient<IExternalCityWeatherForecastProvider, ExternalCityWeatherForecastProvider>();
        })
        .UseNLog();

    return hostBuilder;
}
