namespace isun.Interfaces;

public interface IBackgroundTimer
{
    void Start(string[]? arguments);
    Task StopAsync();
}
