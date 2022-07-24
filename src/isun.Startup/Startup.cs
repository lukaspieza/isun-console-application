﻿using isun.Domain.Implementations;
using isun.Domain.Interfaces;
using isun.Domain.Interfaces.Infrastructure;
using isun.Domain.Models.Options;
using isun.Infrastructure.Implementations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace isun.Startup;

public static class Startup
{
    public static IServiceCollection ConfigureSun(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        services.Configure<AppOptions>(options => configuration.GetSection("AppOptions").Bind(options));
        services.AddTransient<IWeatherForecastService, WeatherForecastService>();
        services.AddTransient<ICitiesProvider, CitiesProvider>();
        services.AddTransient<IArgumentsOperationsProvider, ArgumentsOperationsProvider>();
        services.AddTransient<IExternalCityWeatherForecastProvider, ExternalCityWeatherForecastProvider>();
        return services;
    }
}