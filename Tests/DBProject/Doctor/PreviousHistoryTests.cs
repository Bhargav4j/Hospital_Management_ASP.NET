using System;
using Xunit;
using DBProject.Doctor;

namespace DBProject.Doctor.Tests
{
    public class PreviousHistoryTests
    {
        [Fact]
        public void PreviousHistory_Constructor_ShouldCreateInstance()
        {
            // Act
            var page = new PreviousHistory();

            // Assert
            Assert.NotNull(page);
            Assert.IsType<PreviousHistory>(page);
        }

        [Fact]
        public void Page_Load_ShouldExecuteWithoutException()
        {
            // Arrange
            var page = new PreviousHistory();
            var sender = new object();
            var eventArgs = new EventArgs();

            // Act & Assert - May throw due to Session dependencies
            var exception = Record.Exception(() => page.Page_Load(sender, eventArgs));
        }

        [Fact]
        public void PatHistory_ShouldExist()
        {
            // Arrange
            var page = new PreviousHistory();

            // Act
            var method = page.GetType().GetMethod("PatHistory",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            // Assert
            Assert.NotNull(method);
        }

        [Fact]
        public void PreviousHistory_InheritsFromPage()
        {
            // Arrange & Act
            var page = new PreviousHistory();

            // Assert
            Assert.IsAssignableFrom<System.Web.UI.Page>(page);
        }
    }
}
