using isun.Domain.Interfaces;
using isun.Domain.Interfaces.Infrastructure;
//using Microsoft.Extensions.Logging;

namespace isun.Domain.Implementations;

public class WeatherForecastService : IWeatherForecastService
{
    //private readonly ILogger<WeatherForecastService> _logger;
    private readonly ICitiesProvider _citiesProvider;

    public WeatherForecastService(
        //ILogger<WeatherForecastService> logger,
        ICitiesProvider citiesProvider)
    {
        _citiesProvider = citiesProvider;
        //_logger = logger;
    }

    public string ShowAndSaveWeatherForecast(string[]? args)
    {
        var cities = _citiesProvider.Get(args);
        if (cities.Any()) 
            return ShowAndSaveWeatherForecast(cities);

        var text = args != null
            ? $"No --cities provided in args={string.Join(" ", args)}"
            : "No --cities provided in args=null";
        return text;
    }

    public string ShowAndSaveWeatherForecast(List<string> cities)
    {
        return string.Empty;
    }

    /*private void ShowWeatherForecast()
    {

    }

    private void SaveWeatherForecast()
    {

    }*/
}
