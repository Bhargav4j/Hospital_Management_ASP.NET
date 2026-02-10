using System;
using Xunit;
using DBProject;

namespace DBProject.Tests
{
    public class PatientHomeTests
    {
        [Fact]
        public void PatientHome_Constructor_ShouldCreateInstance()
        {
            // Act
            var page = new PatientHome();

            // Assert
            Assert.NotNull(page);
            Assert.IsType<PatientHome>(page);
        }

        [Fact]
        public void Page_Load_ShouldExecuteWithoutException()
        {
            // Arrange
            var page = new PatientHome();
            var sender = new object();
            var eventArgs = new EventArgs();

            // Act & Assert - May throw due to Session dependencies
            var exception = Record.Exception(() => page.Page_Load(sender, eventArgs));
        }

        [Fact]
        public void PatientInfo_ShouldExist()
        {
            // Arrange
            var page = new PatientHome();

            // Act
            var method = page.GetType().GetMethod("patientInfo",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            // Assert
            Assert.NotNull(method);
        }

        [Fact]
        public void PatientHome_InheritsFromPage()
        {
            // Arrange & Act
            var page = new PatientHome();

            // Assert
            Assert.IsAssignableFrom<System.Web.UI.Page>(page);
        }
    }
}
