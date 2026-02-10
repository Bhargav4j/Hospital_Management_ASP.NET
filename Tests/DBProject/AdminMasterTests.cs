using System;
using Xunit;
using DBProject;

namespace DBProject.Tests
{
    public class AdminMasterTests
    {
        [Fact]
        public void Admin_Constructor_ShouldCreateInstance()
        {
            // Act
            var admin = new Admin();

            // Assert
            Assert.NotNull(admin);
            Assert.IsType<Admin>(admin);
        }

        [Fact]
        public void Page_Load_ShouldExecuteWithoutException()
        {
            // Arrange
            var admin = new Admin();
            var sender = new object();
            var eventArgs = new EventArgs();

            // Act & Assert
            var exception = Record.Exception(() => admin.Page_Load(sender, eventArgs));
            Assert.Null(exception);
        }

        [Fact]
        public void Admin_InheritsFromMasterPage()
        {
            // Arrange & Act
            var admin = new Admin();

            // Assert
            Assert.IsAssignableFrom<System.Web.UI.MasterPage>(admin);
        }
    }
}
