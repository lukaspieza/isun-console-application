using isun.Domain.Implementations;
using isun.Domain.Interfaces.Infrastructure;

namespace isun.Domain.Tests;

public class WeatherForecastServiceTests
{
    private Mock<ICitiesProvider> _mockCitiesProvider = null!;
    private string[]? _arguments;

    [SetUp]
    public void Setup()
    {
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
        GetWeatherForecastService().ShowAndSaveWeatherForecast(_arguments);

        // Assert
        _mockCitiesProvider.Verify(a => a.Get(It.IsAny<string[]?>()), Times.Once);
    }

    [Test]
    public void CitiesProvider_CalledWithProvidedArguments()
    {
        // Arrange

        // Act
        GetWeatherForecastService().ShowAndSaveWeatherForecast(_arguments);

        // Assert
        _mockCitiesProvider.Verify(a => a.Get(It.Is<string[]?>(b => b == _arguments)));
    }

    [Test]
    public void EmptyArgumentsProvided_StatedNoCitiesProvided()
    {
        // Arrange
        const string expectedStartsWith = "No --cities provided in";

        // Act
        var actual = GetWeatherForecastService().ShowAndSaveWeatherForecast(_arguments);

        // Assert
        StringAssert.StartsWith(expectedStartsWith, actual);
    }

    private WeatherForecastService GetWeatherForecastService()
    {
        return new WeatherForecastService(_mockCitiesProvider.Object);
    }
}
