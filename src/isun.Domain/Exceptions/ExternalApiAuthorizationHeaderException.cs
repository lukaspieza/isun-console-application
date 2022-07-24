namespace isun.Domain.Exceptions;

public class ExternalApiAuthorizationHeaderException : BaseException
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
