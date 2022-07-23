namespace isun.Domain.Interfaces;

public interface IWeatherForecastService
{
    string GetWeatherForecast(List<string> cities);
    string GetWeatherForecast(string[]? args);
}
