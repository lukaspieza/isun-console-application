using isun.Domain.Implementations;
using isun.Domain.Interfaces.Infrastructure;
using isun.Domain.Models;
using Microsoft.Extensions.Logging;

namespace isun.Domain.Tests;

public class WeatherForecastServiceTests
{
    private Mock<IExternalCityWeatherForecastProvider> _mockWeatherForecastProvider = null!;
    private Mock<ILogger<WeatherForecastService>> _mockLogger = null!;
    private Mock<ICitiesProvider> _mockCitiesProvider = null!;
    private Mock<ISaveProvider> _mockSaveProvider = null!;
    private Mock<IConsoleProvider> _mockConsole = null!;
    private string[]? _arguments;

    [SetUp]
    public void Setup()
    {
        _mockWeatherForecastProvider = new Mock<IExternalCityWeatherForecastProvider>();
        _mockLogger = new Mock<ILogger<WeatherForecastService>>();
        _mockCitiesProvider = new Mock<ICitiesProvider>();
        _mockSaveProvider = new Mock<ISaveProvider>();
        _mockConsole = new Mock<IConsoleProvider>();
        _mockWeatherForecastProvider.Setup(a => a.GetCityWeatherForecast(It.IsAny<string>()))
            .Returns((string city) => new CityWeatherForecast { City = city });
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

    [Test]
    public void PrintWeatherForecastsOneCity_ConsoleProvider_CalledOnce()
    {
        // Arrange

        // Act
        GetWeatherForecastService().PrintWeatherForecasts(new List<CityWeatherForecast> { new() });

        // Assert
        _mockConsole.Verify(a => a.Write(It.IsAny<string>()), Times.Once);
    }

    [Test]
    public void PrintWeatherForecastsOneCity_ConsoleProvider_CalledMultipleTimes()
    {
        // Arrange

        // Act
        GetWeatherForecastService().PrintWeatherForecasts(new List<CityWeatherForecast> { new(), new() });

        // Assert
        _mockConsole.Verify(a => a.Write(It.IsAny<string>()), Times.Exactly(2));
    }

    [Test]
    public void PrintWeatherForecastsOneCity_ConsoleProvider_CalledWithCityWeatherForecastToString()
    {
        // Arrange
        var expected = new CityWeatherForecast().ToString();

        // Act
        GetWeatherForecastService().PrintWeatherForecasts(new List<CityWeatherForecast> { new() });

        // Assert
        _mockConsole.Verify(a => a.Write(It.Is<string>(b => b == expected)));
    }

    [Test]
    public void PrintWeatherForecastsOneCity_ConsoleProvider_CityNameIsWritten()
    {
        // Arrange
        const string expected = "Vilnius";
        var cityWeatherForecast = new CityWeatherForecast { City = expected };

        // Act
        GetWeatherForecastService().PrintWeatherForecasts(new List<CityWeatherForecast> { cityWeatherForecast });

        // Assert
        _mockConsole.Verify(a => a.Write(It.Is<string>(b => b.Contains(expected))));
    }

    [Test]
    public void PrintWeatherForecastsOneCity_ConsoleProvider_TemperatureIsWritten()
    {
        // Arrange
        const string expected = "99";
        var cityWeatherForecast = new CityWeatherForecast { Temperature = expected };

        // Act
        GetWeatherForecastService().PrintWeatherForecasts(new List<CityWeatherForecast> { cityWeatherForecast });

        // Assert
        _mockConsole.Verify(a => a.Write(It.Is<string>(b => b.Contains(expected))));
    }

    [Test]
    public void PrintWeatherForecastsOneCity_ConsoleProvider_WindSpeedIsWritten()
    {
        // Arrange
        const string expected = "10";
        var cityWeatherForecast = new CityWeatherForecast { WindSpeed = expected };

        // Act
        GetWeatherForecastService().PrintWeatherForecasts(new List<CityWeatherForecast> { cityWeatherForecast });

        // Assert
        _mockConsole.Verify(a => a.Write(It.Is<string>(b => b.Contains(expected))));
    }

    [Test]
    public void PrintWeatherForecastsOneCity_ConsoleProvider_SummaryIsWritten()
    {
        // Arrange
        const string expected = "Mild";
        var cityWeatherForecast = new CityWeatherForecast { Summary = expected };

        // Act
        GetWeatherForecastService().PrintWeatherForecasts(new List<CityWeatherForecast> { cityWeatherForecast });

        // Assert
        _mockConsole.Verify(a => a.Write(It.Is<string>(b => b.Contains(expected))));
    }

    [Test]
    public void SaveWeatherForecasts_CalledOnce()
    {
        // Arrange

        // Act
        GetWeatherForecastService().SaveWeatherForecasts(new List<CityWeatherForecast> { new() });

        // Assert
        _mockSaveProvider.Verify(a => a.Save(It.IsAny<CityWeatherForecast>()), Times.Once);
    }

    [Test]
    public void SaveWeatherForecasts_CalledMultipleTimes()
    {
        // Arrange

        // Act
        GetWeatherForecastService().SaveWeatherForecasts(new List<CityWeatherForecast> { new(), new() });

        // Assert
        _mockSaveProvider.Verify(a => a.Save(It.IsAny<CityWeatherForecast>()), Times.Exactly(2));
    }

    [Test]
    public void SaveWeatherForecasts_CalledProvidedCityWeatherForecast()
    {
        // Arrange
        var expected = new CityWeatherForecast();

        // Act
        GetWeatherForecastService().SaveWeatherForecasts(new List<CityWeatherForecast> { expected });

        // Assert
        _mockSaveProvider.Verify(a => a.Save(It.Is<CityWeatherForecast>(b => b == expected)));
    }

    [Test]
    public void GetWeatherForecasts_NoCitiesProvided_HandleNoCitiesProvidedCalledOnce()
    {
        // Arrange
        const int expectedCount = 0;
        _mockCitiesProvider.Setup(a => a.HandleNoCitiesProvided())
            .Returns(new List<CityWeatherForecast>());

        // Act
        var actual = GetWeatherForecastService().GetWeatherForecasts(new List<string>());

        // Assert
        _mockCitiesProvider.Verify(a => a.HandleNoCitiesProvided(), Times.Once);
        Assert.That(actual, Has.Count.EqualTo(expectedCount));
    }

    [Test]
    public void GetWeatherForecasts_OneCityProvided_GetCityWeatherForecastCalledOnce()
    {
        // Arrange
        const int expectedCount = 1;

        // Act
        var actual = GetWeatherForecastService().GetWeatherForecasts(new List<string> { "Vilnius" });

        // Assert
        _mockWeatherForecastProvider.Verify(a => a.GetCityWeatherForecast(It.IsAny<string>()), Times.Once);
        Assert.That(actual, Has.Count.EqualTo(expectedCount));
    }

    [Test]
    public void GetWeatherForecasts_OneCityProvided_GetCityWeatherForecastCalledMultipleTimes()
    {
        // Arrange
        const int expectedCount = 2;

        // Act
        var actual = GetWeatherForecastService().GetWeatherForecasts(new List<string> { "Vilnius", "Kaunas" });

        // Assert
        _mockWeatherForecastProvider.Verify(a => a.GetCityWeatherForecast(It.IsAny<string>()), Times.Exactly(expectedCount));
        Assert.That(actual, Has.Count.EqualTo(expectedCount));
    }

    [Test]
    public void GetWeatherForecasts_OneCityProvided_GetCityWeatherForecastCalledWithProvidedCity()
    {
        // Arrange
        const int expectedCount = 1;
        const string expected = "Vilnius";

        // Act
        var actual = GetWeatherForecastService().GetWeatherForecasts(new List<string> { expected });

        // Assert
        _mockWeatherForecastProvider.Verify(a => a.GetCityWeatherForecast(It.Is<string>(b => b == expected)));
        Assert.That(actual, Has.Count.EqualTo(expectedCount));
    }

    [Test]
    public void GetWeatherForecasts_OneCityProvided_GetCityWeatherForecastCityNotFoundEmptyList()
    {
        // Arrange
        const int expectedCount = 0;
        const string expected = "Kosmos";
        _mockWeatherForecastProvider = new Mock<IExternalCityWeatherForecastProvider>();

        // Act
        var actual = GetWeatherForecastService().GetWeatherForecasts(new List<string> { expected });

        // Assert
        _mockWeatherForecastProvider.Verify(a => a.GetCityWeatherForecast(It.Is<string>(b => b == expected)));
        Assert.That(actual, Has.Count.EqualTo(expectedCount));
    }

    private WeatherForecastService GetWeatherForecastService()
    {
        return new WeatherForecastService(_mockWeatherForecastProvider.Object,
            _mockLogger.Object,
            _mockCitiesProvider.Object,
            _mockSaveProvider.Object,
            _mockConsole.Object);
    }
}
