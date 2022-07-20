using isun.Domain.Interfaces.Infrastructure;

namespace isun.Infrastructure.Implementations;

public class CitiesProvider : ICitiesProvider
{
    private readonly IArgumentsOperationsProvider _operations;

    public CitiesProvider(IArgumentsOperationsProvider operations)
    {
        _operations = operations;
    }

    public List<string> Get(string[]? args)
    {
        if (args == null)
            return new List<string>();

        var argumentValues = args
            .SkipWhile(arg => _operations.VariableNameCities(arg))
            .Skip(VariableName())
            .TakeWhile(arg => _operations.VariableValue(arg));

        var arguments = new List<string>();
        foreach (var splitArguments in argumentValues.Select(arg => _operations.SplitBySeparator(arg)))
        {
            arguments.AddRange(splitArguments.Where(_operations.ArgumentIsNotEmpty));
        }

        return arguments;
    }

    public int VariableName()
    {
        return 1;
    }
}
