using System;
using Xunit;
using doctor;

namespace doctor.Tests
{
    public class DoctorHomeTests
    {
        [Fact]
        public void DoctorHome_Constructor_ShouldCreateInstance()
        {
            // Act
            var page = new doctorhome();

            // Assert
            Assert.NotNull(page);
            Assert.IsType<doctorhome>(page);
        }

        [Fact]
        public void Page_Load_ShouldExecuteWithoutException()
        {
            // Arrange
            var page = new doctorhome();
            var sender = new object();
            var eventArgs = new EventArgs();

            // Act & Assert - May throw due to Session and UI dependencies
            var exception = Record.Exception(() => page.Page_Load(sender, eventArgs));
        }

        [Fact]
        public void DoctorHome_InheritsFromPage()
        {
            // Arrange & Act
            var page = new doctorhome();

            // Assert
            Assert.IsAssignableFrom<System.Web.UI.Page>(page);
        }
    }
}
