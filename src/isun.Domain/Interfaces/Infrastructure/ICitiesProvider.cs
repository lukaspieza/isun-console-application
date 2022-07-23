using isun.Domain.Models;

namespace isun.Domain.Interfaces.Infrastructure;

public interface ICitiesProvider
{
    List<CityWeatherForecast> HandleNoCitiesProvided(string[]? args);
    List<CityWeatherForecast> HandleNoCitiesProvided();
    List<string> Get(string[]? args);
}
