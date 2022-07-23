using AutoMapper;
using isun.Domain.Interfaces.Infrastructure;
using isun.Domain.Models;
using isun.Domain.Models.Options;
using isun.Infrastructure.Models.ExternalWeatherApi;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RestSharp;

namespace isun.Infrastructure.Implementations;

public class ExternalCityWeatherForecastProvider : IExternalCityWeatherForecastProvider
{
    private readonly ILogger<IExternalCityWeatherForecastProvider> _logger;
    private readonly ExternalWeatherApiOptions _options;
    private readonly IMapper _mapper;
    private string? _authorizationToken;
    private List<string>? _validCities;


    public ExternalCityWeatherForecastProvider(
        ILogger<IExternalCityWeatherForecastProvider> logger,
        IOptions<AppOptions> options,
        IMapper mapper)
    {
        _options = options.Value.ExternalWeatherApi;
        _logger = logger;
        _mapper = mapper;
    }

    public CityWeatherForecast? GetCityWeatherForecast(string city)
    {
        if (!IsCityValid(city))
        {
            return null;
        }

        var restClient = GetRestClient();
        var request = new RestRequest(string.Format(_options.WeathersResource, city));
        request = AddAuthorizationHeader(request);
        var result = restClient.Get(request);
        if (string.IsNullOrWhiteSpace(result.Content))
        {
            // Todo: Throw exception
            throw new Exception();
        }

        var forecastResponse = JsonConvert.DeserializeObject<CityWeatherForecastResponse>(result.Content);
        if (forecastResponse == null)
        {
            // Todo: Throw exception
            throw new Exception();
        }

        return _mapper.Map<CityWeatherForecast>(forecastResponse);
    }

    private bool IsCityValid(string city)
    {
        if (_validCities != null && _validCities.Any())
        {
            return _validCities.Contains(city);
        }

        var restClient = GetRestClient();
        var request = new RestRequest(_options.CitiesResource);
        request = AddAuthorizationHeader(request);
        var result = restClient.Get(request);
        if (string.IsNullOrWhiteSpace(result.Content))
        {
            // Todo: Throw exception
            throw new Exception();
        }

        var validCities = JsonConvert.DeserializeObject<List<string>>(result.Content);
        if (validCities == null)
        {
            // Todo: Throw exception
            throw new Exception();
        }

        _validCities = validCities;
        return _validCities.Contains(city);
    }

    private RestRequest AddAuthorizationHeader(RestRequest restRequest)
    {
        if (!string.IsNullOrWhiteSpace(_authorizationToken))
        {
            restRequest.AddHeader("Authorization", _authorizationToken);
            return restRequest;
        }

        var body = _mapper.Map<AuthorizationRequest>(_options);
        var bodyString = JsonConvert.SerializeObject(body);
        var restClient = GetRestClient();
        var request = new RestRequest(_options.AuthorizationResource);
        request.AddParameter("application/json", bodyString, ParameterType.RequestBody);
        var result = restClient.Post(request);
        if (string.IsNullOrWhiteSpace(result.Content))
        {
            // Todo: Throw exception
            throw new Exception();
        }

        var authorizationResponse = JsonConvert.DeserializeObject<AuthorizationResponse>(result.Content);
        if (authorizationResponse == null)
        {
            // Todo: Throw exception
            throw new Exception();
        }

        _authorizationToken = $"Bearer {authorizationResponse.Token}";
        restRequest.AddHeader("Authorization", _authorizationToken);
        return restRequest;
    }

    private RestClient GetRestClient()
    {
        return new RestClient(_options.BaseUrl);
    }
}
