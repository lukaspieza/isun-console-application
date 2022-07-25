namespace isun.Domain.Exceptions;

public class CityNotProvidedException : Exception
{
    public CityNotProvidedException()
    {
    }

    public CityNotProvidedException(string message)
        : base(message)
    {
    }

    public CityNotProvidedException(string message, Exception inner)
        : base(message, inner)
    {
    }
}
