using isun.Domain.Interfaces.Infrastructure;

namespace isun.Infrastructure.Implementations;

public class ArgumentsOperationsProvider : IArgumentsOperationsProvider
{
    public bool ArgumentIsNotEmpty(string argument)
    {
        return !string.IsNullOrWhiteSpace(argument);
    }

    public string[] SplitBySeparator(string argument, string separator = ",")
    {
        return argument.Split(separator);
    }

    public bool VariableValue(string argument, string variableNameStartsWith = "--")
    {
        return !argument.StartsWith(variableNameStartsWith);
    }

    public bool VariableNameCities(string argument, string variableName = "--cities")
    {
        return !string.Equals(argument, variableName, StringComparison.CurrentCultureIgnoreCase);
    }
}
