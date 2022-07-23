using isun.Domain.Interfaces;

namespace isun.Domain.Implementations;

public class WeatherForecastProvider : IWeatherForecastProvider
{
    public WeatherForecastProvider()
    {

    }

    public string GetMissingArgumentsMessage(string[]? args)
    {
        var message = args != null
            ? $"No --cities provided in args={string.Join(" ", args)}"
            : "No --cities provided in args=null";
        return message;

    }
}
