using isun.Domain.Interfaces;
using isun.Domain.Interfaces.Infrastructure;
using Microsoft.Extensions.Logging;

namespace isun.Domain.Implementations;

public class WeatherForecastService : IWeatherForecastService
{
    private readonly IWeatherForecastProvider _forecastProvider;
    private readonly ILogger<WeatherForecastService> _logger;
    private readonly ICitiesProvider _citiesProvider;

    public WeatherForecastService(
        IWeatherForecastProvider forecastProvider,
        ILogger<WeatherForecastService> logger,
        ICitiesProvider citiesProvider)
    {
        _forecastProvider = forecastProvider;
        _citiesProvider = citiesProvider;
        _logger = logger;
    }

    public string GetWeatherForecast(string[]? args)
    {
        _logger.LogDebug("Getting Weather Forecast with arguments");
        var cities = _citiesProvider.Get(args);
        return cities.Any()
            ? GetWeatherForecast(cities)
            : _forecastProvider.GetMissingArgumentsMessage(args);
    }

    public string GetWeatherForecast(List<string> cities)
    {
        _logger.LogDebug("Getting Weather Forecast with cities");
        return string.Empty;
    }

    /*private void ShowWeatherForecast()
    {

    }

    private void SaveWeatherForecast()
    {

    }*/
}
