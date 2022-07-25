using AutoMapper;
using isun.Domain.Exceptions;
using isun.Domain.Interfaces.Infrastructure;
using isun.Domain.Models;
using isun.Domain.Models.Options;
using isun.Infrastructure.Interfaces;
using isun.Infrastructure.Models.ExternalWeatherApi;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RestSharp;

namespace isun.Infrastructure.Implementations;

public class ExternalCityWeatherForecastProvider : IExternalCityWeatherForecastProvider
{
    private readonly ILogger<ExternalCityWeatherForecastProvider> _logger;
    private readonly IExternalCityWeatherForecastOperationsProvider _operations;
    private readonly ExternalWeatherApiOptions _options;
    private readonly IMapper _mapper;

    public ExternalCityWeatherForecastProvider(
        IExternalCityWeatherForecastOperationsProvider operations,
        ILogger<ExternalCityWeatherForecastProvider> logger,
        IOptions<AppOptions> options,
        IMapper mapper)
    {
        _options = options.Value.ExternalWeatherApi;
        _logger = logger;
        _mapper = mapper;
        _operations = operations;
    }

    public CityWeatherForecast? GetCityWeatherForecast(string city)
    {
        _logger.LogDebug("Getting Weather Forecast for City {city}", city);
        if (!_operations.IsCityValid(city))
        {
            _logger.LogDebug("City does not exist returning null");
            return null;
        }

        var resource = string.Format(_options.WeathersResource, city);
        _logger.LogDebug("City Exists, Creating Rest Client for {resource}", resource);
        var restClient = _operations.GetRestClient();
        var request = new RestRequest(resource);
        request = _operations.AddAuthorizationHeader(request);
        var result = restClient.Get(request);
        _logger.LogDebug("Request completed for {resource}", resource);
        if (string.IsNullOrWhiteSpace(result.Content))
        {
            throw new ExternalApiForecastsException(
                _operations.GetExceptionText("No content received from", _options.CitiesResource, result.Content, result.StatusCode.ToString()));
        }

        var forecastResponse = JsonConvert.DeserializeObject<CityWeatherForecastResponse>(result.Content);
        if (forecastResponse == null)
        {
            throw new ExternalApiForecastsException(
                _operations.GetExceptionText("Could not convert to object from", _options.CitiesResource, result.Content, result.StatusCode.ToString()));
        }

        _logger.LogDebug("{resource} received successfully mapping to CityWeatherForecast", resource);
        return _mapper.Map<CityWeatherForecast>(forecastResponse);
    }
}
