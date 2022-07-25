namespace isun.Domain.Models.Options;

public class AppOptions
{
    public ExternalWeatherApiOptions ExternalWeatherApi { get; set; } = new();
    public CityOptions City { get; set; } = new();
    public SaveOptions Save { get; set; } = new();
}
