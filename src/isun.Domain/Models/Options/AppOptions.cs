namespace isun.Domain.Models.Options;

public class AppOptions
{
    public ExternalWeatherApiOptions ExternalWeatherApi { get; set; } = new ExternalWeatherApiOptions();
    public CityOptions City { get; set; } = new CityOptions();
}
