using isun.Domain.Interfaces;
using isun.Interfaces;
using isun.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace isun.Implementations;

public class BackgroundTimer : IBackgroundTimer
{
    private readonly IWeatherForecastService _forecastService;
    private readonly CancellationTokenSource _cts = new();
    private readonly ILogger<BackgroundTimer> _logger;
    private readonly BackgroundTimerOptions _options;
    private PeriodicTimer _timer = null!;
    private string[]? _arguments;
    private Task? _timerTask;

    public BackgroundTimer(
        IOptions<BackgroundTimerOptions> options,
        IWeatherForecastService forecastService,
        ILogger<BackgroundTimer> logger)
    {
        _forecastService = forecastService;
        _options = options.Value;
        _logger = logger;
    }

    public void Start(string[]? arguments)
    {
        if (_options.RunEverySeconds <= 1)
            throw new Exception("BackgroundTimerOptions.RunEverySeconds should be bigger than 1 second.");

        _logger.LogInformation("Press any key to stop the task");
        _arguments = arguments;
        var interval = TimeSpan.FromMilliseconds(1000 * _options.RunEverySeconds);
        _timer = new PeriodicTimer(interval);
        _timerTask = PrintWeatherForecastsTask();
    }

    public async Task StopAsync()
    {
        _logger.LogInformation("Stop command received");
        if (_timerTask is null)
        {
            return;
        }

        _cts.Cancel();
        await _timerTask;
        _cts.Dispose();
        _logger.LogInformation("Stop executed successfully");
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
            _logger.LogError(e, "");
            _logger.LogInformation("Press any key to stop the task");
        }
    }

    private async Task PrintWeatherForecastsEveryTick()
    {
        do
        {
            PrintWeatherForecasts();
            _logger.LogInformation("Next update in {_options.RunEverySeconds} seconds", _options.RunEverySeconds);
        } while (await _timer.WaitForNextTickAsync(_cts.Token));
    }

    private void PrintWeatherForecasts()
    {
        foreach (var cityWeatherForecast in _forecastService.GetWeatherForecast(_arguments))
        {
            var forecast = cityWeatherForecast.ToString();
            _logger.LogInformation("{forecast}", forecast);
        }
    }
}
