namespace isun.Domain.Exceptions;

public class ExternalApiForecastsException : Exception
{
    public ExternalApiForecastsException()
    {
    }

    public ExternalApiForecastsException(string message)
        : base(message)
    {
    }

    public ExternalApiForecastsException(string message, Exception inner)
        : base(message, inner)
    {
    }
}
