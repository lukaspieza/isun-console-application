using Newtonsoft.Json;

namespace isun.Infrastructure.Models.WeatherApi;

public class AuthorizationRequest
{
    [JsonProperty("username")]
    public string UserName { get; set; } = null!;
    [JsonProperty("password")]
    public string Password { get; set; } = null!;
}
