using isun.Domain.Interfaces;
using isun.Domain.Models;
using Microsoft.Extensions.Logging;

namespace isun.Domain.Implementations;

public class WeatherForecastProvider : IWeatherForecastProvider
{
    private readonly ILogger<WeatherForecastProvider> _logger;

    public WeatherForecastProvider(ILogger<WeatherForecastProvider> logger)
    {
        _logger = logger;
    }

    public List<CityWeatherForecast> GetMissingArgumentsMessage(string[]? args)
    {
        _logger.LogDebug("Getting missing arguments message");
        var message = args != null
            ? $"No --cities provided in args={string.Join(" ", args)}"
            : "No --cities provided in args=null";
        _logger.LogWarning("{message}", message);
        return new List<CityWeatherForecast>();
    }
}
