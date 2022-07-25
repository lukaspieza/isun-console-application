using isun.Domain.Implementations;
using isun.Domain.Interfaces.Infrastructure;
using Microsoft.Extensions.Logging;

namespace isun.Domain.Tests;

public class WeatherForecastServiceTests
{
    private Mock<IExternalCityWeatherForecastProvider> _weatherForecastProviderMock = null!;
    private Mock<ILogger<WeatherForecastService>> _mockLogger = null!;
    private Mock<ICitiesProvider> _mockCitiesProvider = null!;
    private Mock<ISaveProvider> _mockSaveProvider = null!;
    private Mock<IConsoleProvider> _mockConsole = null!;
    private string[]? _arguments;

    [SetUp]
    public void Setup()
    {
        _weatherForecastProviderMock = new Mock<IExternalCityWeatherForecastProvider>();
        _mockLogger = new Mock<ILogger<WeatherForecastService>>();
        _mockCitiesProvider = new Mock<ICitiesProvider>();
        _mockSaveProvider = new Mock<ISaveProvider>();
        _mockConsole = new Mock<IConsoleProvider>();
        _mockCitiesProvider.Setup(a => a.GetCities(It.IsAny<string[]?>()))
            .Returns(new List<string>());
        _arguments = null;
    }

    [Test]
    public void CitiesProvider_CalledOnce()
    {
        // Arrange

        // Act
        GetWeatherForecastService().GetWeatherForecasts(_arguments);

        // Assert
        _mockCitiesProvider.Verify(a => a.GetCities(It.IsAny<string[]?>()), Times.Once);
    }

    [Test]
    public void CitiesProvider_CalledWithProvidedArguments()
    {
        // Arrange

        // Act
        GetWeatherForecastService().GetWeatherForecasts(_arguments);

        // Assert
        _mockCitiesProvider.Verify(a => a.GetCities(It.Is<string[]?>(b => b == _arguments)));
    }

    private WeatherForecastService GetWeatherForecastService()
    {
        return new WeatherForecastService(_weatherForecastProviderMock.Object,
            _mockLogger.Object,
            _mockCitiesProvider.Object,
            _mockSaveProvider.Object,
            _mockConsole.Object);
    }
}
