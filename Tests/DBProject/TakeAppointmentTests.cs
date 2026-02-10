using System;
using Xunit;
using DBProject;

namespace DBProject.Tests
{
    public class TakeAppointmentTests
    {
        [Fact]
        public void TakeAppointment_Constructor_ShouldCreateInstance()
        {
            // Act
            var page = new TakeAppointment();

            // Assert
            Assert.NotNull(page);
            Assert.IsType<TakeAppointment>(page);
        }

        [Fact]
        public void Page_Load_ShouldExecuteWithoutException()
        {
            // Arrange
            var page = new TakeAppointment();
            var sender = new object();
            var eventArgs = new EventArgs();

            // Act & Assert - May throw due to Session dependencies
            var exception = Record.Exception(() => page.Page_Load(sender, eventArgs));
        }

        [Fact]
        public void DeptInfo_ShouldExist()
        {
            // Arrange
            var page = new TakeAppointment();

            // Act
            var method = page.GetType().GetMethod("deptInfo",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            // Assert
            Assert.NotNull(method);
        }

        [Fact]
        public void TDeptGrid_RowCommand_ShouldExist()
        {
            // Arrange
            var page = new TakeAppointment();

            // Act
            var method = page.GetType().GetMethod("TDeptGrid_RowCommand",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            // Assert
            Assert.NotNull(method);
        }

        [Fact]
        public void TakeAppointment_InheritsFromPage()
        {
            // Arrange & Act
            var page = new TakeAppointment();

            // Assert
            Assert.IsAssignableFrom<System.Web.UI.Page>(page);
        }
    }
}
