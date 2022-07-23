using isun.Domain.Models;

namespace isun.Domain.Interfaces;

public interface IWeatherForecastProvider
{
    List<CityWeatherForecast> GetMissingArgumentsMessage(string[]? args);
}
