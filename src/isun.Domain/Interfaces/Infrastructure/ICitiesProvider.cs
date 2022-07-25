using isun.Domain.Models;

namespace isun.Domain.Interfaces.Infrastructure;

public interface ICitiesProvider
{
    List<CityWeatherForecast> HandleNoCitiesProvided();
    List<string> GetCities(string[]? args);
}
