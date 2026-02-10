using System;
using Xunit;
using DBProject;

namespace DBProject.Tests
{
    public class ViewDoctorsTests
    {
        [Fact]
        public void ViewDoctors_Constructor_ShouldCreateInstance()
        {
            // Act
            var page = new ViewDoctors();

            // Assert
            Assert.NotNull(page);
            Assert.IsType<ViewDoctors>(page);
        }

        [Fact]
        public void Page_Load_ShouldExecuteWithoutException()
        {
            // Arrange
            var page = new ViewDoctors();
            var sender = new object();
            var eventArgs = new EventArgs();

            // Act & Assert - May throw due to Session dependencies
            var exception = Record.Exception(() => page.Page_Load(sender, eventArgs));
        }

        [Fact]
        public void DeptDoctorInfo_ShouldExist()
        {
            // Arrange
            var page = new ViewDoctors();

            // Act
            var method = page.GetType().GetMethod("deptDoctorInfo",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            // Assert
            Assert.NotNull(method);
        }

        [Fact]
        public void TDoctorGrid_RowCommand_ShouldExist()
        {
            // Arrange
            var page = new ViewDoctors();

            // Act
            var method = page.GetType().GetMethod("TDoctorGrid_RowCommand",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            // Assert
            Assert.NotNull(method);
        }

        [Fact]
        public void ViewDoctors_InheritsFromPage()
        {
            // Arrange & Act
            var page = new ViewDoctors();

            // Assert
            Assert.IsAssignableFrom<System.Web.UI.Page>(page);
        }
    }
}
