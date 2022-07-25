using isun.Domain.Exceptions;
using isun.Domain.Interfaces.Infrastructure;
using isun.Domain.Models.Options;
using isun.Infrastructure.Implementations;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace isun.Infrastructure.Tests;

public class CitiesProviderTests
{
    private Mock<IArgumentsOperationsProvider> _mockProvider = null!;
    private Mock<ILogger<CitiesProvider>> _mockLogger = null!;
    private Mock<IOptions<AppOptions>> _mockOptions = null!;
    private string[]? _arguments;
    private const string ExpectedArgument = "Expected";
    private const string ExpectedCityOne = "Vilnius";
    private const string ExpectedCityTwo = "Kaunas";

    [SetUp]
    public void Setup()
    {
        _mockProvider = new Mock<IArgumentsOperationsProvider>();
        _mockLogger = new Mock<ILogger<CitiesProvider>>();
        _mockOptions = new Mock<IOptions<AppOptions>>();
        _mockOptions.Setup(a => a.Value).Returns(new AppOptions());
        _arguments = new[] { ExpectedArgument };
    }

    [Test]
    public void NullArgumentsProvided_ExpectCityNotProvidedException()
    {
        // Arrange
        _arguments = null;

        // Act
        try
        {
            GetCitiesProvider().GetCities(_arguments);
        }
        catch (CityNotProvidedException)
        {
            // Assert
            Assert.Pass();
        }

        Assert.Fail("Expected ExternalApiCitiesException");
    }

    [Test]
    public void VariableNameSkipped_Expected()
    {
        // Arrange
        const int expectedCount = 1;
        const string notExpectedVariableName = "--cities";
        SetupFullArgumentsOperationsProviderFlow();
        _arguments = new[] { notExpectedVariableName, "Vilnius" };

        // Act
        var actual = GetCitiesProvider().GetCities(_arguments);

        // Assert
        Assert.That(actual, Has.Count.EqualTo(expectedCount));
        CollectionAssert.DoesNotContain(actual, notExpectedVariableName);
        _mockProvider.Verify(a => a.VariableNameCities(It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(expectedCount));
    }

    [Test]
    public void TakeUntilNextVariableValue_ExpectedOneCity()
    {
        // Arrange
        const int expectedCount = 1;
        SetupFullArgumentsOperationsProviderFlow();
        _arguments = new[] { "--cities", ExpectedCityOne };

        // Act
        var actual = GetCitiesProvider().GetCities(_arguments);

        // Assert
        Assert.That(actual, Has.Count.EqualTo(expectedCount));
        CollectionAssert.Contains(actual, ExpectedCityOne);
        _mockProvider.Verify(a => a.VariableValue(It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(expectedCount));
    }

    [Test]
    public void TakeUntilNextVariableValue_ExpectedOneCity_WhenOtherVariableAtEnd()
    {
        // Arrange
        const int expectedCount = 1;
        SetupFullArgumentsOperationsProviderFlow();
        _arguments = new[] { "--cities", ExpectedCityOne, "--other", "otherValue" };

        // Act
        var actual = GetCitiesProvider().GetCities(_arguments);

        // Assert
        Assert.That(actual, Has.Count.EqualTo(expectedCount));
        CollectionAssert.Contains(actual, ExpectedCityOne);
        _mockProvider.Verify(a => a.VariableValue(It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(expectedCount + 1));
    }

    [Test]
    public void TakeUntilNextVariableValue_ExpectedMultipleCities()
    {
        // Arrange
        const int expectedCount = 2;
        SetupFullArgumentsOperationsProviderFlow();
        _arguments = new[] { "--cities", ExpectedCityOne, ExpectedCityTwo };

        // Act
        var actual = GetCitiesProvider().GetCities(_arguments);

        // Assert
        Assert.That(actual, Has.Count.EqualTo(expectedCount));
        CollectionAssert.Contains(actual, ExpectedCityOne);
        CollectionAssert.Contains(actual, ExpectedCityTwo);
        _mockProvider.Verify(a => a.VariableValue(It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(expectedCount));
    }

    [Test]
    public void TakeUntilNextVariableValue_ExpectedMultipleCities_WhenOtherVariableAtEnd()
    {
        // Arrange
        const int expectedCount = 2;
        SetupFullArgumentsOperationsProviderFlow();
        _arguments = new[] { "--cities", ExpectedCityOne, ExpectedCityTwo, "--other", "otherValue" };

        // Act
        var actual = GetCitiesProvider().GetCities(_arguments);

        // Assert
        Assert.That(actual, Has.Count.EqualTo(expectedCount));
        CollectionAssert.Contains(actual, ExpectedCityOne);
        CollectionAssert.Contains(actual, ExpectedCityTwo);
        _mockProvider.Verify(a => a.VariableValue(It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(expectedCount + 1));
    }

    [Test]
    public void SplitBySeparator_ExpectedCitiesSeparatedBySeparatorWithoutSpace()
    {
        // Arrange
        const int expectedCount = 2;
        SetupFullArgumentsOperationsProviderFlow();
        _arguments = new[] { "--cities", $"{ExpectedCityOne},{ExpectedCityTwo}", "--other", "otherValue" };

        // Act
        var actual = GetCitiesProvider().GetCities(_arguments);

        // Assert
        Assert.That(actual, Has.Count.EqualTo(expectedCount));
        CollectionAssert.Contains(actual, ExpectedCityOne);
        CollectionAssert.Contains(actual, ExpectedCityTwo);
        _mockProvider.Verify(a => a.SplitBySeparator(It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(expectedCount));
    }

    [Test]
    public void SplitBySeparator_ExpectedCitiesSeparatedBySeparatorWithSpace()
    {
        // Arrange
        const int expectedCount = 2;
        SetupFullArgumentsOperationsProviderFlow();
        _arguments = new[] { "--cities", $"{ExpectedCityOne},", ExpectedCityTwo, "--other", "otherValue" };

        // Act
        var actual = GetCitiesProvider().GetCities(_arguments);

        // Assert
        Assert.That(actual, Has.Count.EqualTo(expectedCount));
        CollectionAssert.Contains(actual, ExpectedCityOne);
        CollectionAssert.Contains(actual, ExpectedCityTwo);
        _mockProvider.Verify(a => a.SplitBySeparator(It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(expectedCount));
    }

    [Test]
    public void ArgumentIsNotEmpty_CalledMultipleTimes()
    {
        // Arrange
        SetupFullArgumentsOperationsProviderFlow();
        _arguments = new[] { "--cities", "Vilnius,", "Kaunas", "--other", "otherValue" };

        // Act
        GetCitiesProvider().GetCities(_arguments);

        // Assert
        _mockProvider.Verify(a => a.ArgumentIsNotEmpty(It.IsAny<string>()), Times.Exactly(3));
    }

    private void SetupFullArgumentsOperationsProviderFlow()
    {
        _mockProvider.Setup(a => a.VariableNameCities(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(false);
        _mockProvider.Setup(a => a.VariableValue(It.IsAny<string>(), It.IsAny<string>()))
            .Returns((string argument, string _) => !argument.StartsWith("--"));
        _mockProvider.Setup(a => a.SplitBySeparator(It.IsAny<string>(), It.IsAny<string>()))
            .Returns((string argument, string _) => argument.Split(","));
        _mockProvider.Setup(a => a.ArgumentIsNotEmpty(It.IsAny<string>()))
            .Returns((string argument) => !string.IsNullOrWhiteSpace(argument));
    }

    private CitiesProvider GetCitiesProvider()
    {
        return new CitiesProvider(_mockProvider.Object, _mockLogger.Object, _mockOptions.Object);
    }
}
