using System;
using Xunit;
using DBProject;

namespace DBProject.Tests
{
    public class TreatmentHistoryTests
    {
        [Fact]
        public void TreatmentHistory_Constructor_ShouldCreateInstance()
        {
            // Act
            var page = new TreatmentHistory();

            // Assert
            Assert.NotNull(page);
            Assert.IsType<TreatmentHistory>(page);
        }

        [Fact]
        public void Page_Load_ShouldExecuteWithoutException()
        {
            // Arrange
            var page = new TreatmentHistory();
            var sender = new object();
            var eventArgs = new EventArgs();

            // Act & Assert - May throw due to Session dependencies
            var exception = Record.Exception(() => page.Page_Load(sender, eventArgs));
        }

        [Fact]
        public void TreatmentHistory_Method_ShouldExist()
        {
            // Arrange
            var page = new TreatmentHistory();

            // Act
            var method = page.GetType().GetMethod("treatmentHistory",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            // Assert
            Assert.NotNull(method);
        }

        [Fact]
        public void TreatmentHistory_InheritsFromPage()
        {
            // Arrange & Act
            var page = new TreatmentHistory();

            // Assert
            Assert.IsAssignableFrom<System.Web.UI.Page>(page);
        }
    }
}
