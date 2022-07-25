using AutoMapper;
using isun.Domain.Exceptions;
using isun.Domain.Models.Options;
using isun.Infrastructure.Interfaces;
using isun.Infrastructure.Models.ExternalWeatherApi;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RestSharp;

namespace isun.Infrastructure.Implementations;

public class ExternalCityWeatherForecastOperationsProvider : IExternalCityWeatherForecastOperationsProvider
{
    private readonly ILogger<ExternalCityWeatherForecastOperationsProvider> _logger;
    private readonly ExternalWeatherApiOptions _options;
    private readonly IMapper _mapper;
    private string? _authorizationToken;
    private List<string>? _validCities;


    public ExternalCityWeatherForecastOperationsProvider(
        ILogger<ExternalCityWeatherForecastOperationsProvider> logger,
        IOptions<AppOptions> options,
        IMapper mapper)
    {
        _options = options.Value.ExternalWeatherApi;
        _logger = logger;
        _mapper = mapper;
    }
    public bool IsCityValid(string city)
    {
        if (_validCities != null && _validCities.Any())
        {
            _logger.LogDebug("Using Cached Valid Cities {_validCities.Count}", _validCities.Count);
            return _validCities.Contains(city);
        }

        _logger.LogDebug("Creating Rest Client for {_options.CitiesResource}", _options.CitiesResource);
        var restClient = GetRestClient();
        var request = new RestRequest(_options.CitiesResource);
        request = AddAuthorizationHeader(request);
        var result = restClient.Get(request);
        _logger.LogDebug("Request completed for {_options.CitiesResource}", _options.CitiesResource);
        if (string.IsNullOrWhiteSpace(result.Content))
        {
            throw new ExternalApiCitiesException(
                GetExceptionText("No content received from", _options.CitiesResource, result.Content, result.StatusCode.ToString()));
        }

        var validCities = JsonConvert.DeserializeObject<List<string>>(result.Content);
        if (validCities == null)
        {
            throw new ExternalApiCitiesException(
                GetExceptionText("Could not convert to object from", _options.CitiesResource, result.Content, result.StatusCode.ToString()));
        }

        _validCities = validCities;
        _logger.LogDebug("Caching Valid Cities {_validCities.Count} in Memory", _validCities.Count);
        return _validCities.Contains(city);
    }

    public RestRequest AddAuthorizationHeader(RestRequest restRequest)
    {
        if (!string.IsNullOrWhiteSpace(_authorizationToken))
        {
            _logger.LogDebug("Adding Cached Authorization Header to Request");
            restRequest.AddHeader("Authorization", _authorizationToken);
            return restRequest;
        }

        _logger.LogDebug("Creating Rest Client for {_options.AuthorizationResource}", _options.AuthorizationResource);
        var body = _mapper.Map<AuthorizationRequest>(_options);
        var bodyString = JsonConvert.SerializeObject(body);
        var restClient = GetRestClient();
        var request = new RestRequest(_options.AuthorizationResource);
        request.AddParameter("application/json", bodyString, ParameterType.RequestBody);
        var result = restClient.Post(request);
        _logger.LogDebug("Request completed for {_options.AuthorizationResource}", _options.AuthorizationResource);
        if (string.IsNullOrWhiteSpace(result.Content))
        {
            throw new ExternalApiAuthorizationHeaderException(
                GetExceptionText("No content received from", _options.AuthorizationResource, result.Content, result.StatusCode.ToString()));
        }

        var authorizationResponse = JsonConvert.DeserializeObject<AuthorizationResponse>(result.Content);
        if (authorizationResponse == null)
        {
            throw new ExternalApiAuthorizationHeaderException(
                GetExceptionText("Could not convert to object from", _options.AuthorizationResource, result.Content, result.StatusCode.ToString()));
        }

        _authorizationToken = $"Bearer {authorizationResponse.Token}";
        _logger.LogDebug("Caching Authorization Header in Memory");
        _logger.LogDebug("Adding Cached Authorization Header to Request");
        restRequest.AddHeader("Authorization", _authorizationToken);
        return restRequest;
    }

    public RestClient GetRestClient()
    {
        return new RestClient(_options.BaseUrl);
    }

    public string GetExceptionText(string message, string resource, string? content, string? statusCode)
    {
        return $"{message} url={_options.BaseUrl}{resource} content={content} statusCode={statusCode}";
    }
}
