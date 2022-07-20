namespace isun.Domain.Interfaces.Infrastructure;

public interface ICitiesProvider
{
    List<string> Get(string[]? args);
}
