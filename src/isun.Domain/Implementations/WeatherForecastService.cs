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

    public void ShowAndSaveWeatherForecast(string[]? args)
    {
        var cities = _citiesProvider.Get(args);

        if (!cities.Any())
        {
            Console.WriteLine(args != null
                ? $"No --cities provided in args={string.Join(" ", args)}"
                : "No --cities provided in args=null");
            return;
        }

        ShowAndSaveWeatherForecast(cities);
    }

    public void ShowAndSaveWeatherForecast(List<string> cities)
    {

    }

    /*private void ShowWeatherForecast()
    {

    }

    private void SaveWeatherForecast()
    {

    }*/
}
