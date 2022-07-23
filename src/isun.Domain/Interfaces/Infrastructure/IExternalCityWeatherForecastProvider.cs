using isun.Domain.Models;

namespace isun.Domain.Interfaces.Infrastructure;

public interface IExternalCityWeatherForecastProvider
{
    CityWeatherForecast? GetCityWeatherForecast(string city);
}
