namespace isun.Domain.Interfaces;

public interface IWeatherForecastService
{
    string ShowAndSaveWeatherForecast(List<string> cities);
    string ShowAndSaveWeatherForecast(string[]? args);
}
