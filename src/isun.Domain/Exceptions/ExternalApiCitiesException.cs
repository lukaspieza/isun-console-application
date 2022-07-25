namespace isun.Domain.Exceptions;

public class ExternalApiCitiesException : Exception
{
    public ExternalApiCitiesException()
    {
    }

    public ExternalApiCitiesException(string message)
        : base(message)
    {
    }

    public ExternalApiCitiesException(string message, Exception inner)
        : base(message, inner)
    {
    }
}
