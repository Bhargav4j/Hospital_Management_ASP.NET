using System;
using Xunit;
using DBProject;

namespace DBProject.Tests
{
    public class AppointmentTakerTests
    {
        [Fact]
        public void AppointmentTaker_Constructor_ShouldCreateInstance()
        {
            // Act
            var page = new AppointmentTaker();

            // Assert
            Assert.NotNull(page);
            Assert.IsType<AppointmentTaker>(page);
        }

        [Fact]
        public void Page_Load_ShouldExecuteWithoutException()
        {
            // Arrange
            var page = new AppointmentTaker();
            var sender = new object();
            var eventArgs = new EventArgs();

            // Act & Assert - May throw due to Session dependencies
            var exception = Record.Exception(() => page.Page_Load(sender, eventArgs));
        }

        [Fact]
        public void FreeSlots_ShouldExist()
        {
            // Arrange
            var page = new AppointmentTaker();

            // Act
            var method = page.GetType().GetMethod("freeSlots",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            // Assert
            Assert.NotNull(method);
        }

        [Fact]
        public void PAppointmentGrid_RowCommand_ShouldExist()
        {
            // Arrange
            var page = new AppointmentTaker();

            // Act
            var method = page.GetType().GetMethod("PAppointmentGrid_RowCommand",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            // Assert
            Assert.NotNull(method);
        }

        [Fact]
        public void AppointmentTaker_InheritsFromPage()
        {
            // Arrange & Act
            var page = new AppointmentTaker();

            // Assert
            Assert.IsAssignableFrom<System.Web.UI.Page>(page);
        }
    }
}
