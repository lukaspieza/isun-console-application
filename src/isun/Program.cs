﻿using isun.Domain.Implementations;
using isun.Domain.Interfaces;
using isun.Domain.Interfaces.Infrastructure;
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
        })
        .ConfigureServices((_, services) =>
        {
            services.AddTransient<IWeatherForecastProvider, WeatherForecastProvider>();
            services.AddTransient<IWeatherForecastService, WeatherForecastService>();
            services.AddTransient<ICitiesProvider, CitiesProvider>();
            services.AddTransient<IArgumentsOperationsProvider, ArgumentsOperationsProvider>();
            services.AddTransient<IConsoleProvider, ConsoleProvider>();
        })
        .UseNLog();

    return hostBuilder;
}
