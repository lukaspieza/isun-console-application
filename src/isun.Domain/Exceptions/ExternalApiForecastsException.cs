namespace isun.Domain.Exceptions;

public class ExternalApiForecastsException : BaseException
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
