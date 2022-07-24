using RestSharp;

namespace isun.Infrastructure.Interfaces;

public interface IExternalCityWeatherForecastOperationsProvider
{
    string GetExceptionText(string message, string resource, string? content, string? statusCode);
    RestRequest AddAuthorizationHeader(RestRequest restRequest);
    bool IsCityValid(string city);
    RestClient GetRestClient();
}
