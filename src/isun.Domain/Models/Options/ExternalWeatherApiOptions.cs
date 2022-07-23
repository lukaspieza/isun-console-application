namespace isun.Domain.Models.Options;

public class ExternalWeatherApiOptions
{
    public string BaseUrl { get; set; } = null!;
    public string AuthorizationResource { get; set; } = null!;
    public string AuthorizationUserName { get; set; } = null!;
    public string AuthorizationPassword { get; set; } = null!;
    public string CitiesResource { get; set; } = null!;
    public string WeathersResource { get; set; } = null!;
}
