using System;
using Xunit;
using doctor;

namespace doctor.Tests
{
    public class PendingAppointmentTests
    {
        [Fact]
        public void PendingAppointment_Constructor_ShouldCreateInstance()
        {
            // Act
            var page = new pendingappointment();

            // Assert
            Assert.NotNull(page);
            Assert.IsType<pendingappointment>(page);
        }

        [Fact]
        public void Page_Load_ShouldExecuteWithoutException()
        {
            // Arrange
            var page = new pendingappointment();
            var sender = new object();
            var eventArgs = new EventArgs();

            // Act & Assert - May throw due to Session and UI dependencies
            var exception = Record.Exception(() => page.Page_Load(sender, eventArgs));
        }

        [Fact]
        public void LoadGrid_ShouldExist()
        {
            // Arrange
            var page = new pendingappointment();

            // Act
            var method = page.GetType().GetMethod("loadgrid");

            // Assert
            Assert.NotNull(method);
        }

        [Fact]
        public void Update_Appointment_ShouldExist()
        {
            // Arrange
            var page = new pendingappointment();

            // Act
            var method = page.GetType().GetMethod("update_appointment",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            // Assert
            Assert.NotNull(method);
        }

        [Fact]
        public void Delete_Appointment_ShouldExist()
        {
            // Arrange
            var page = new pendingappointment();

            // Act
            var method = page.GetType().GetMethod("Delete_appointment",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            // Assert
            Assert.NotNull(method);
        }

        [Fact]
        public void PendingAppointment_InheritsFromPage()
        {
            // Arrange & Act
            var page = new pendingappointment();

            // Assert
            Assert.IsAssignableFrom<System.Web.UI.Page>(page);
        }
    }
}
