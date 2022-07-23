using isun.Domain.Implementations;
using isun.Domain.Interfaces;
using isun.Domain.Interfaces.Infrastructure;

namespace isun.Domain.Tests;

public class WeatherForecastServiceTests
{
    private Mock<IWeatherForecastProvider> _mockWeatherForecastProvider = null!;
    private Mock<ICitiesProvider> _mockCitiesProvider = null!;
    private string[]? _arguments;

    [SetUp]
    public void Setup()
    {
        _mockWeatherForecastProvider = new Mock<IWeatherForecastProvider>();
        _mockCitiesProvider = new Mock<ICitiesProvider>();
        _mockCitiesProvider.Setup(a => a.Get(It.IsAny<string[]?>()))
            .Returns(new List<string>());
        _arguments = null;
    }

    [Test]
    public void CitiesProvider_CalledOnce()
    {
        // Arrange

        // Act
        GetWeatherForecastService().GetWeatherForecast(_arguments);

        // Assert
        _mockCitiesProvider.Verify(a => a.Get(It.IsAny<string[]?>()), Times.Once);
    }

    [Test]
    public void CitiesProvider_CalledWithProvidedArguments()
    {
        // Arrange

        // Act
        GetWeatherForecastService().GetWeatherForecast(_arguments);

        // Assert
        _mockCitiesProvider.Verify(a => a.Get(It.Is<string[]?>(b => b == _arguments)));
    }

    [Test]
    public void EmptyArgumentsProvidedGetMissingArgumentsMessage_CalledOnce()
    {
        // Arrange

        // Act
        GetWeatherForecastService().GetWeatherForecast(_arguments);

        // Assert
        _mockWeatherForecastProvider.Verify(a => a.GetMissingArgumentsMessage(It.IsAny<string[]?>()), Times.Once);
    }

    [Test]
    public void EmptyArgumentsProvidedGetMissingArgumentsMessage_CalledWithArguments()
    {
        // Arrange

        // Act
        GetWeatherForecastService().GetWeatherForecast(_arguments);

        // Assert
        _mockWeatherForecastProvider.Verify(a => a.GetMissingArgumentsMessage(It.Is<string[]?>(b => b == _arguments)));
    }

    private WeatherForecastService GetWeatherForecastService()
    {
        return new WeatherForecastService(_mockWeatherForecastProvider.Object, _mockCitiesProvider.Object);
    }
}
