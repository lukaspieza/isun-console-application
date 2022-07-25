using isun.Domain.Interfaces;
using isun.Interfaces;
using isun.Models;
using Microsoft.Extensions.Options;

namespace isun.Implementations;

public class BackgroundTimer : IBackgroundTimer
{
    public string[]? Arguments { get; set; }
    private readonly IWeatherForecastService _forecastService;
    private readonly CancellationTokenSource _cts = new();
    private readonly BackgroundTimerOptions _options;
    private readonly IConsoleProvider _console;
    private PeriodicTimer _timer = null!;
    private Task? _timerTask;

    public BackgroundTimer(
        IOptions<BackgroundTimerOptions> options,
        IWeatherForecastService forecastService,
        IConsoleProvider console)
    {
        _forecastService = forecastService;
        _options = options.Value;
        _console = console;
    }

    public void Start()
    {
        if (_options.RunEverySeconds <= 1)
            throw new ArgumentException("BackgroundTimerOptions.RunEverySeconds should be bigger than 1 second.");

        if (Arguments is null)
            throw new ArgumentException("Arguments not provided for BackgroundTimer");

        _console.Write("Press any key to stop the task");
        var interval = TimeSpan.FromMilliseconds(1000 * _options.RunEverySeconds);
        _timer = new PeriodicTimer(interval);
        _timerTask = PrintWeatherForecastsTask();
    }

    public async Task StopAsync()
    {
        _console.Write("Stop command received");
        if (_timerTask is null)
        {
            return;
        }

        _cts.Cancel();
        await _timerTask;
        _cts.Dispose();
        _console.Write("Stop executed successfully");
    }

    private async Task PrintWeatherForecastsTask()
    {
        try
        {
            await PrintWeatherForecastsEveryTick();
        }
        catch (OperationCanceledException)
        {
            // Ignore stop of the task Exception
        }
        catch (Exception e)
        {
            _console.Write(e);
            _console.Write("Press any key to stop the task");
        }
    }

    private async Task PrintWeatherForecastsEveryTick()
    {
        do
        {
            PrintWeatherForecasts();
            _console.Write($"Next update in {_options.RunEverySeconds} seconds");
        } while (await _timer.WaitForNextTickAsync(_cts.Token));
    }

    private void PrintWeatherForecasts()
    {
        foreach (var cityWeatherForecast in _forecastService.GetWeatherForecasts(Arguments))
        {
            var forecast = cityWeatherForecast.ToString();
            _console.Write(forecast);
        }
    }
}
