namespace isun.Domain.Interfaces;

public interface IWeatherForecastService
{
    void ShowAndSaveWeatherForecast(List<string> cities);
    void ShowAndSaveWeatherForecast(string[]? args);
}
