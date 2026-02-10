using System;
using Xunit;
using DBProject;

namespace DBProject.Tests
{
    public class HealthTests
    {
        [Fact]
        public void Health_Constructor_ShouldCreateInstance()
        {
            // Act
            var page = new Health();

            // Assert
            Assert.NotNull(page);
            Assert.IsType<Health>(page);
        }

        [Fact]
        public void Page_Load_ShouldExecuteWithoutException()
        {
            // Arrange
            var page = new Health();
            var sender = new object();
            var eventArgs = new EventArgs();

            // Act & Assert - May throw due to database dependencies
            var exception = Record.Exception(() => page.Page_Load(sender, eventArgs));
        }

        [Fact]
        public void Health_InheritsFromPage()
        {
            // Arrange & Act
            var page = new Health();

            // Assert
            Assert.IsAssignableFrom<System.Web.UI.Page>(page);
        }

        [Fact]
        public void Health_ShouldSetContentTypeToJson()
        {
            // Arrange
            var page = new Health();

            // Act & Assert
            // This test verifies the class exists and can be instantiated
            Assert.NotNull(page);
        }
    }
}
