using isun.Domain.Models;

namespace isun.Domain.Interfaces;

public interface IWeatherForecastService
{
    List<CityWeatherForecast> GetWeatherForecasts(List<string> cities);
    List<CityWeatherForecast> GetWeatherForecasts(string[]? args);
}
