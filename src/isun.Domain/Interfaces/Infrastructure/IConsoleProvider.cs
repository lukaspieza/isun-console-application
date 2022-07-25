namespace isun.Domain.Interfaces.Infrastructure;

public interface IConsoleProvider
{
    void Write(string message);
    void Write(Exception exception);
}
