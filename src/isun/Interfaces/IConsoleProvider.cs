namespace isun.Interfaces;

public interface IConsoleProvider
{
    void Write(string message);
    void Write(Exception exception);
}
