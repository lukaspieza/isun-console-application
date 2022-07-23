using isun.Domain.Models;

namespace isun.Domain.Interfaces;

public interface IWeatherForecastService
{
    List<CityWeatherForecast> GetWeatherForecast(List<string> cities);
    List<CityWeatherForecast> GetWeatherForecast(string[]? args);
}
