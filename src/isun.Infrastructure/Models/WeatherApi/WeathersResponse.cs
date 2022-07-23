using Newtonsoft.Json;

namespace isun.Infrastructure.Models.WeatherApi;

public class WeathersResponse
{
    [JsonProperty("city")]
    public string City { get; set; } = null!;
    [JsonProperty("temperature")]
    public string Temperature { get; set; } = null!;
    [JsonProperty("precipitation")]
    public string Precipitation { get; set; } = null!;
    [JsonProperty("windSpeed")]
    public string WindSpeed { get; set; } = null!;
    [JsonProperty("summary")]
    public string Summary { get; set; } = null!;
}
