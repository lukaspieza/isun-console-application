namespace isun.Interfaces;

public interface IBackgroundTimer
{
    string[]? Arguments { get; set; }
    void Start();
    Task StopAsync();
}
