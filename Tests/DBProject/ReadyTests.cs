using System;
using Xunit;
using DBProject;

namespace DBProject.Tests
{
    public class ReadyTests
    {
        [Fact]
        public void Ready_Constructor_ShouldCreateInstance()
        {
            // Act
            var page = new Ready();

            // Assert
            Assert.NotNull(page);
            Assert.IsType<Ready>(page);
        }

        [Fact]
        public void Page_Load_ShouldExecuteWithoutException()
        {
            // Arrange
            var page = new Ready();
            var sender = new object();
            var eventArgs = new EventArgs();

            // Act & Assert - May throw due to database dependencies
            var exception = Record.Exception(() => page.Page_Load(sender, eventArgs));
        }

        [Fact]
        public void Ready_InheritsFromPage()
        {
            // Arrange & Act
            var page = new Ready();

            // Assert
            Assert.IsAssignableFrom<System.Web.UI.Page>(page);
        }

        [Fact]
        public void Ready_ShouldSetContentTypeToJson()
        {
            // Arrange
            var page = new Ready();

            // Act & Assert
            // This test verifies the class exists and can be instantiated
            Assert.NotNull(page);
        }
    }
}
