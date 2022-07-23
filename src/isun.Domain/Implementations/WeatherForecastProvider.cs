using isun.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace isun.Domain.Implementations;

public class WeatherForecastProvider : IWeatherForecastProvider
{
    private readonly ILogger<WeatherForecastProvider> _logger;

    public WeatherForecastProvider(ILogger<WeatherForecastProvider> logger)
    {
        _logger = logger;
    }

    public string GetMissingArgumentsMessage(string[]? args)
    {
        _logger.LogDebug("Getting missing arguments message");
        var message = args != null
            ? $"No --cities provided in args={string.Join(" ", args)}"
            : "No --cities provided in args=null";
        _logger.LogWarning("{message}", message);
        return message;
    }
}
