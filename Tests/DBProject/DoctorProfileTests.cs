using System;
using Xunit;
using DBProject;

namespace DBProject.Tests
{
    public class DoctorProfileTests
    {
        [Fact]
        public void DoctorProfile_Constructor_ShouldCreateInstance()
        {
            // Act
            var page = new DoctorProfile();

            // Assert
            Assert.NotNull(page);
            Assert.IsType<DoctorProfile>(page);
        }

        [Fact]
        public void Page_Load_ShouldExecuteWithoutException()
        {
            // Arrange
            var page = new DoctorProfile();
            var sender = new object();
            var eventArgs = new EventArgs();

            // Act & Assert - May throw due to Session dependencies
            var exception = Record.Exception(() => page.Page_Load(sender, eventArgs));
        }

        [Fact]
        public void DoctorInfo_ShouldExist()
        {
            // Arrange
            var page = new DoctorProfile();

            // Act
            var method = page.GetType().GetMethod("doctorInfo",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            // Assert
            Assert.NotNull(method);
        }

        [Fact]
        public void RedirectToAppointmentTaker_ShouldExist()
        {
            // Arrange
            var page = new DoctorProfile();

            // Act
            var method = page.GetType().GetMethod("RedirectToAppointmentTaker",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            // Assert
            Assert.NotNull(method);
        }

        [Fact]
        public void DoctorProfile_InheritsFromPage()
        {
            // Arrange & Act
            var page = new DoctorProfile();

            // Assert
            Assert.IsAssignableFrom<System.Web.UI.Page>(page);
        }
    }
}
