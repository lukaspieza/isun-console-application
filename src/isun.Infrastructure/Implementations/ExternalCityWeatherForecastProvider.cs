using AutoMapper;
using isun.Domain.Exceptions;
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
        _logger.LogDebug("Getting Weather Forecast for City {city}", city);
        if (!IsCityValid(city))
        {
            _logger.LogDebug("City does not exist returning null");
            return null;
        }

        var resource = string.Format(_options.WeathersResource, city);
        _logger.LogDebug("City Exists, Creating Rest Client for {resource}", resource);
        var restClient = GetRestClient();
        var request = new RestRequest(resource);
        request = AddAuthorizationHeader(request);
        var result = restClient.Get(request);
        _logger.LogDebug("Request completed for {resource}", resource);
        if (string.IsNullOrWhiteSpace(result.Content))
        {
            throw new ExternalApiForecastsException(
                GetExceptionText("No content received from", _options.CitiesResource, result.Content, result.StatusCode.ToString()));
        }

        var forecastResponse = JsonConvert.DeserializeObject<CityWeatherForecastResponse>(result.Content);
        if (forecastResponse == null)
        {
            throw new ExternalApiForecastsException(
                GetExceptionText("Could not convert to object from", _options.CitiesResource, result.Content, result.StatusCode.ToString()));
        }

        _logger.LogDebug("{resource} received successfully mapping to CityWeatherForecast", resource);
        return _mapper.Map<CityWeatherForecast>(forecastResponse);
    }

    private bool IsCityValid(string city)
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

    private RestRequest AddAuthorizationHeader(RestRequest restRequest)
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

    private RestClient GetRestClient()
    {
        return new RestClient(_options.BaseUrl);
    }

    private string GetExceptionText(string message, string resource, string? content, string? statusCode)
    {
        return $"{message} url={_options.BaseUrl}{resource} content={content} statusCode={statusCode}";
    }
}
