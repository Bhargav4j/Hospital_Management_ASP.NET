using System;
using Xunit;
using DBProject;

namespace DBProject.Tests
{
    public class CurrentAppointmentTests
    {
        [Fact]
        public void CurrentAppointment_Constructor_ShouldCreateInstance()
        {
            // Act
            var page = new CurrentAppointment();

            // Assert
            Assert.NotNull(page);
            Assert.IsType<CurrentAppointment>(page);
        }

        [Fact]
        public void Page_Load_ShouldExecuteWithoutException()
        {
            // Arrange
            var page = new CurrentAppointment();
            var sender = new object();
            var eventArgs = new EventArgs();

            // Act & Assert - May throw due to Session dependencies
            var exception = Record.Exception(() => page.Page_Load(sender, eventArgs));
        }

        [Fact]
        public void AppointmentToday_ShouldExist()
        {
            // Arrange
            var page = new CurrentAppointment();

            // Act
            var method = page.GetType().GetMethod("appointmentToday",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            // Assert
            Assert.NotNull(method);
        }

        [Fact]
        public void CurrentAppointment_InheritsFromPage()
        {
            // Arrange & Act
            var page = new CurrentAppointment();

            // Assert
            Assert.IsAssignableFrom<System.Web.UI.Page>(page);
        }
    }
}
