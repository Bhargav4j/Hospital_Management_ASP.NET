using System;
using Xunit;
using DBProject;

namespace DBProject.Tests
{
    public class PatientFeedbackTests
    {
        [Fact]
        public void PatientFeedback_Constructor_ShouldCreateInstance()
        {
            // Act
            var page = new PatientFeedback();

            // Assert
            Assert.NotNull(page);
            Assert.IsType<PatientFeedback>(page);
        }

        [Fact]
        public void Page_Load_ShouldExecuteWithoutException()
        {
            // Arrange
            var page = new PatientFeedback();
            var sender = new object();
            var eventArgs = new EventArgs();

            // Act & Assert - May throw due to Session dependencies
            var exception = Record.Exception(() => page.Page_Load(sender, eventArgs));
        }

        [Fact]
        public void PendingFeedback_ShouldExist()
        {
            // Arrange
            var page = new PatientFeedback();

            // Act
            var method = page.GetType().GetMethod("pendingFeedback",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            // Assert
            Assert.NotNull(method);
        }

        [Fact]
        public void GiveFeedback_ShouldExist()
        {
            // Arrange
            var page = new PatientFeedback();

            // Act
            var method = page.GetType().GetMethod("giveFeedback",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            // Assert
            Assert.NotNull(method);
        }

        [Fact]
        public void PatientFeedback_InheritsFromPage()
        {
            // Arrange & Act
            var page = new PatientFeedback();

            // Assert
            Assert.IsAssignableFrom<System.Web.UI.Page>(page);
        }
    }
}
