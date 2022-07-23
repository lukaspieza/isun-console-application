using Newtonsoft.Json;

namespace isun.Infrastructure.Models.ExternalWeatherApi;

public class AuthorizationResponse
{
    [JsonProperty("token")]
    public string Token { get; set; } = null!;
}
