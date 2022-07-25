namespace isun.Domain.Exceptions;

public class ExternalApiAuthorizationHeaderException : Exception
{
    public ExternalApiAuthorizationHeaderException()
    {
    }

    public ExternalApiAuthorizationHeaderException(string message)
        : base(message)
    {
    }

    public ExternalApiAuthorizationHeaderException(string message, Exception inner)
        : base(message, inner)
    {
    }
}
