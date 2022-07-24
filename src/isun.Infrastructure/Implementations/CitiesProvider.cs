using isun.Domain.Interfaces.Infrastructure;
using isun.Domain.Models;
using Microsoft.Extensions.Logging;

namespace isun.Infrastructure.Implementations;

public class CitiesProvider : ICitiesProvider
{
    private readonly IArgumentsOperationsProvider _operations;
    private readonly ILogger<CitiesProvider> _logger;

    public CitiesProvider(
        IArgumentsOperationsProvider operations,
        ILogger<CitiesProvider> logger)
    {
        _operations = operations;
        _logger = logger;
    }

    public List<CityWeatherForecast> HandleNoCitiesProvided(string[]? args)
    {
        _logger.LogDebug("Getting missing arguments message");
        var message = args != null
            ? $"No --cities provided in args={string.Join(" ", args)}"
            : "No --cities provided in args=null";
        _logger.LogWarning("{message}", message);
        return new List<CityWeatherForecast>();
    }

    public List<CityWeatherForecast> HandleNoCitiesProvided()
    {
        _logger.LogWarning("No cities were provided");
        return new List<CityWeatherForecast>();
    }

    public List<string> Get(string[]? args)
    {
        _logger.LogDebug("Getting Cities from Arguments");
        if (args == null)
        {
            _logger.LogWarning("No arguments provided returning empty list");
            return new List<string>();
        }

        var argumentValues = args
            .SkipWhile(arg => _operations.VariableNameCities(arg))
            .Skip(VariableName())
            .TakeWhile(arg => _operations.VariableValue(arg));
        var arguments = new List<string>();
        foreach (var splitArguments in argumentValues.Select(arg => _operations.SplitBySeparator(arg)))
            arguments.AddRange(splitArguments.Where(_operations.ArgumentIsNotEmpty));

        _logger.LogDebug("Returning {arguments.Count}", arguments.Count);
        return arguments;
    }

    private static int VariableName()
    {
        return 1;
    }
}
