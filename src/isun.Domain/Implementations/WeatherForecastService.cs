using isun.Domain.Interfaces;
using isun.Domain.Interfaces.Infrastructure;
using isun.Domain.Models;
using Microsoft.Extensions.Logging;

namespace isun.Domain.Implementations;

public class WeatherForecastService : IWeatherForecastService
{
    private readonly IExternalCityWeatherForecastProvider _external;
    private readonly IWeatherForecastProvider _forecastProvider;
    private readonly ILogger<WeatherForecastService> _logger;
    private readonly ICitiesProvider _citiesProvider;

    public WeatherForecastService(
        IExternalCityWeatherForecastProvider external,
        IWeatherForecastProvider forecastProvider,
        ILogger<WeatherForecastService> logger,
        ICitiesProvider citiesProvider)
    {
        _external = external;
        _forecastProvider = forecastProvider;
        _citiesProvider = citiesProvider;
        _logger = logger;
    }

    public List<CityWeatherForecast> GetWeatherForecast(string[]? args)
    {
        _logger.LogDebug("Getting Weather Forecast with arguments");
        var cities = _citiesProvider.Get(args);
        return cities.Any()
            ? GetWeatherForecast(cities)
            : _forecastProvider.GetMissingArgumentsMessage(args);
    }

    public List<CityWeatherForecast> GetWeatherForecast(List<string> cities)
    {
        _logger.LogDebug("Getting Weather Forecast with cities");
        var forecasts = new List<CityWeatherForecast>();
        foreach (var city in cities)
        {
            var cityWeatherForecast = _external.GetCityWeatherForecast(city);
            if (cityWeatherForecast == null)
            {
                _logger.LogWarning($"City {city} was not found");
                continue;
            }

            forecasts.Add(cityWeatherForecast);
        }

        return forecasts;
    }
}
