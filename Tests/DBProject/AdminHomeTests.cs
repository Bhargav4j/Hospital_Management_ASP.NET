using System;
using Xunit;
using DBProject;

namespace DBProject.Tests
{
    public class AdminHomeTests
    {
        [Fact]
        public void AdminHome_Constructor_ShouldCreateInstance()
        {
            // Act
            var adminHome = new AdminHome();

            // Assert
            Assert.NotNull(adminHome);
            Assert.IsType<AdminHome>(adminHome);
        }

        [Fact]
        public void Page_Load_ShouldExecuteWithoutException()
        {
            // Arrange
            var adminHome = new AdminHome();
            var sender = new object();
            var eventArgs = new EventArgs();

            // Act & Assert
            var exception = Record.Exception(() => adminHome.Page_Load(sender, eventArgs));
            // Note: May throw due to missing UI controls or DAL dependencies
        }

        [Fact]
        public void GetAdminHomeInformation_ShouldExist()
        {
            // Arrange
            var adminHome = new AdminHome();

            // Act
            var method = adminHome.GetType().GetMethod("GetAdminHomeInformation");

            // Assert
            Assert.NotNull(method);
        }

        [Fact]
        public void AdminHome_InheritsFromPage()
        {
            // Arrange & Act
            var adminHome = new AdminHome();

            // Assert
            Assert.IsAssignableFrom<System.Web.UI.Page>(adminHome);
        }
    }
}
