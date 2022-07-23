using isun.Domain.Interfaces.Infrastructure;

namespace isun.Infrastructure.Implementations;

public class ConsoleProvider : IConsoleProvider
{
    public void WriteLine(string? value)
    {
        Console.WriteLine(value);
    }
}
