using isun.Domain.Models;

namespace isun.Domain.Interfaces;

public interface IWeatherForecastService
{
    void PrintWeatherForecasts(List<CityWeatherForecast> weatherForecasts);
    void SaveWeatherForecasts(List<CityWeatherForecast> weatherForecasts);
    List<CityWeatherForecast> GetWeatherForecasts(List<string> cities);
    List<CityWeatherForecast> GetWeatherForecasts(string[]? args);
}
