using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using HospitalManagement.Web.Pages;

namespace HospitalManagement.Web.Pages.Tests;

public class IndexModelTests
{
    private readonly Mock<ILogger<IndexModel>> _mockLogger;

    public IndexModelTests()
    {
        _mockLogger = new Mock<ILogger<IndexModel>>();
    }

    [Fact]
    public void IndexModel_Constructor_WithValidLogger_CreatesInstance()
    {
        // Arrange & Act
        var model = new IndexModel(_mockLogger.Object);

        // Assert
        Assert.NotNull(model);
    }

    [Fact]
    public void IndexModel_Constructor_WithNullLogger_ThrowsArgumentNullException()
    {
        // Arrange & Act & Assert
        Assert.Throws<ArgumentNullException>(() => new IndexModel(null!));
    }

    [Fact]
    public void OnGet_ExecutesSuccessfully()
    {
        // Arrange
        var model = new IndexModel(_mockLogger.Object);

        // Act
        model.OnGet();

        // Assert - method completes without exception
        Assert.True(true);
    }

    [Fact]
    public void OnGet_LogsInformation()
    {
        // Arrange
        var model = new IndexModel(_mockLogger.Object);

        // Act
        model.OnGet();

        // Assert
        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Home page loaded")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [Fact]
    public void OnGet_CanBeCalledMultipleTimes()
    {
        // Arrange
        var model = new IndexModel(_mockLogger.Object);

        // Act
        model.OnGet();
        model.OnGet();
        model.OnGet();

        // Assert
        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Home page loaded")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Exactly(3));
    }
}
