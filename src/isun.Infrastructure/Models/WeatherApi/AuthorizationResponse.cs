using Newtonsoft.Json;

namespace isun.Infrastructure.Models.WeatherApi;

public class AuthorizationResponse
{
    [JsonProperty("token")]
    public string Token { get; set; } = null!;
}
