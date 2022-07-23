namespace isun.Domain.Models.Options;

public class ExternalWeatherApiOptions
{
    public string BaseUrl { get; set; } = "https://weather-api.isun.ch";
    public string AuthorizationResource { get; set; } = "/api/authorize";
    public string AuthorizationUserName { get; set; } = null!;
    public string AuthorizationPassword { get; set; } = null!;
    public string CitiesResource { get; set; } = "/api/cities";
    public string WeathersResource { get; set; } = "/api/weathers/{0}";
}
