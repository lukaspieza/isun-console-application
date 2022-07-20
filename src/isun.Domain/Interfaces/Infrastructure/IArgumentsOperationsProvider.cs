namespace isun.Domain.Interfaces.Infrastructure;

public interface IArgumentsOperationsProvider
{
    bool ArgumentIsNotEmpty(string argument);
    string[] SplitBySeparator(string argument, string separator = ",");
    bool VariableValue(string argument, string variableNameStartsWith = "--");
    bool VariableNameCities(string argument, string variableName = "--cities");
}
