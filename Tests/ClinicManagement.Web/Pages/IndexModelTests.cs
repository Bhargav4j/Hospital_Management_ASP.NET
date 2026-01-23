using System;
using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using ClinicManagement.Web.Pages;

namespace ClinicManagement.Web.Pages.Tests
{
    public class IndexModelTests
    {
        [Fact]
        public void IndexModel_Constructor_InitializesLogger()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<IndexModel>>();

            // Act
            var model = new IndexModel(mockLogger.Object);

            // Assert
            Assert.NotNull(model);
        }

        [Fact]
        public void OnGet_LogsInformation()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<IndexModel>>();
            var model = new IndexModel(mockLogger.Object);

            // Act
            model.OnGet();

            // Assert
            mockLogger.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Home page accessed")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }

        [Fact]
        public void OnGet_DoesNotThrowException()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<IndexModel>>();
            var model = new IndexModel(mockLogger.Object);

            // Act & Assert
            var exception = Record.Exception(() => model.OnGet());
            Assert.Null(exception);
        }
    }
}
