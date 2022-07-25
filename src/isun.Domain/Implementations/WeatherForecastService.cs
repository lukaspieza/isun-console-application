using isun.Domain.Interfaces;
using isun.Domain.Interfaces.Infrastructure;
using isun.Domain.Models;
using Microsoft.Extensions.Logging;

namespace isun.Domain.Implementations;

public class WeatherForecastService : IWeatherForecastService
{
    private readonly IExternalCityWeatherForecastProvider _external;
    private readonly ILogger<WeatherForecastService> _logger;
    private readonly ICitiesProvider _citiesProvider;
    private readonly ISaveProvider _saveProvider;
    private readonly IConsoleProvider _console;

    public WeatherForecastService(
        IExternalCityWeatherForecastProvider external,
        ILogger<WeatherForecastService> logger,
        ICitiesProvider citiesProvider,
        ISaveProvider saveProvider,
        IConsoleProvider console)
    {
        _citiesProvider = citiesProvider;
        _saveProvider = saveProvider;
        _external = external;
        _console = console;
        _logger = logger;
    }

    public void PrintWeatherForecasts(List<CityWeatherForecast> weatherForecasts)
    {
        foreach (var cityWeatherForecast in weatherForecasts)
        {
            var forecast = cityWeatherForecast.ToString();
            _console.Write(forecast);
        }
    }

    public void SaveWeatherForecasts(List<CityWeatherForecast> weatherForecasts)
    {
        foreach (var cityWeatherForecast in weatherForecasts)
            _saveProvider.Save(cityWeatherForecast);
    }

    public List<CityWeatherForecast> GetWeatherForecasts(string[]? args)
    {
        _logger.LogDebug("Getting Weather Forecast with arguments");
        var cities = _citiesProvider.GetCities(args);
        return GetWeatherForecasts(cities);
    }

    public List<CityWeatherForecast> GetWeatherForecasts(List<string> cities)
    {
        _logger.LogDebug("Getting Weather Forecast with cities");
        if (!cities.Any())
            return _citiesProvider.HandleNoCitiesProvided();

        var forecasts = new List<CityWeatherForecast>();
        foreach (var city in cities)
        {
            var cityWeatherForecast = _external.GetCityWeatherForecast(city);
            if (cityWeatherForecast == null)
            {
                _logger.LogWarning("City {city} was not found", city);
                continue;
            }

            forecasts.Add(cityWeatherForecast);
        }

        return forecasts;
    }
}
