using System;
using Xunit;
using DBProject;

namespace DBProject.Tests
{
    public class AppointmentRequestSentTests
    {
        [Fact]
        public void AppointmentRequestSent_Constructor_ShouldCreateInstance()
        {
            // Act
            var page = new AppointmentNotificationSent();

            // Assert
            Assert.NotNull(page);
            Assert.IsType<AppointmentNotificationSent>(page);
        }

        [Fact]
        public void Page_Load_ShouldExecuteWithoutException()
        {
            // Arrange
            var page = new AppointmentNotificationSent();
            var sender = new object();
            var eventArgs = new EventArgs();

            // Act & Assert
            var exception = Record.Exception(() => page.Page_Load(sender, eventArgs));
            Assert.Null(exception);
        }

        [Fact]
        public void SendARequest_ShouldExist()
        {
            // Arrange
            var page = new AppointmentNotificationSent();

            // Act
            var method = page.GetType().GetMethod("sendARequest",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            // Assert
            Assert.NotNull(method);
        }

        [Fact]
        public void AppointmentRequestSent_InheritsFromPage()
        {
            // Arrange & Act
            var page = new AppointmentNotificationSent();

            // Assert
            Assert.IsAssignableFrom<System.Web.UI.Page>(page);
        }
    }
}
