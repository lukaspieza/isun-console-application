using isun.Domain.Interfaces.Infrastructure;
using isun.Domain.Models;
using isun.Domain.Models.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace isun.Infrastructure.Implementations;

public class SaveFileProvider : ISaveProvider
{
    private readonly ILogger<SaveFileProvider> _logger;
    private readonly SaveOptions _options;

    public SaveFileProvider(
        ILogger<SaveFileProvider> logger,
        IOptions<AppOptions> options)
    {
        _options = options.Value.Save;
        _logger = logger;
    }

    public void Save(CityWeatherForecast cityWeatherForecast)
    {
        if (!Directory.Exists(_options.Directory))
        {
            _logger.LogInformation("Creating directory {_options.Directory}", _options.Directory);
            Directory.CreateDirectory(_options.Directory);
        }

        var fileName = Path.Combine(_options.Directory, $"{cityWeatherForecast.City}_{DateTime.Now:yyyyMMddHHmmssfff}.json");
        var content = JsonConvert.SerializeObject(cityWeatherForecast);
        File.WriteAllText(fileName, content);
    }
}
