using isun.Domain.Exceptions;
using isun.Domain.Interfaces.Infrastructure;
using isun.Domain.Models;
using isun.Domain.Models.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace isun.Infrastructure.Implementations;

public class CitiesProvider : ICitiesProvider
{
    private readonly IArgumentsOperationsProvider _operations;
    private readonly ILogger<CitiesProvider> _logger;
    private readonly CityOptions _options;

    public CitiesProvider(
        IArgumentsOperationsProvider operations,
        ILogger<CitiesProvider> logger,
        IOptions<AppOptions> options)
    {
        _options = options.Value.City;
        _operations = operations;
        _logger = logger;
    }

    public List<CityWeatherForecast> HandleNoCitiesProvided()
    {
        const string message = "No cities were provided";
        if (_options.ThrowExceptionOnNotFoundCity)
            throw new CityNotProvidedException(message);

        _logger.LogWarning("{message}", message);
        return new List<CityWeatherForecast>();
    }

    public List<string> GetCities(string[]? args)
    {
        _logger.LogDebug("Getting Cities from Arguments");
        if (args == null) 
            throw new CityNotProvidedException("No --cities provided in args=null");

        var argumentValues = args
            .SkipWhile(arg => _operations.VariableNameCities(arg))
            .Skip(VariableName())
            .TakeWhile(arg => _operations.VariableValue(arg));
        var arguments = new List<string>();
        foreach (var splitArguments in argumentValues.Select(arg => _operations.SplitBySeparator(arg)))
            arguments.AddRange(splitArguments.Where(_operations.ArgumentIsNotEmpty));

        if (!arguments.Any())
            throw new CityNotProvidedException($"No --cities provided in args={string.Join(" ", args)}");

        _logger.LogDebug("Returning {arguments.Count}", arguments.Count);
        return arguments;
    }

    private static int VariableName()
    {
        return 1;
    }
}
