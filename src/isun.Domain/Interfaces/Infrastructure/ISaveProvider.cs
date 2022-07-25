using isun.Domain.Models;

namespace isun.Domain.Interfaces.Infrastructure;

public interface ISaveProvider
{
    void Save(CityWeatherForecast cityWeatherForecast);
}
