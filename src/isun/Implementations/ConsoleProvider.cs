using isun.Interfaces;
using Microsoft.Extensions.Logging;

namespace isun.Implementations;

public class ConsoleProvider : IConsoleProvider
{
    private readonly ILogger<ConsoleProvider> _logger;

    public ConsoleProvider(ILogger<ConsoleProvider> logger)
    {
        _logger = logger;
    }

    public void Write(string message)
    {
        _logger.LogInformation("{message}", message);
    }

    public void Write(Exception exception)
    {
        _logger.LogError(exception, "");
    }
}
