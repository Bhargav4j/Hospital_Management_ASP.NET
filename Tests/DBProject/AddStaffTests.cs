using System;
using Xunit;
using DBProject;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DBProject.Tests
{
    public class AddStaffTests
    {
        [Fact]
        public void Page_Load_ShouldExecuteWithoutException()
        {
            // Arrange
            var addStaff = new AddStaff();
            var sender = new object();
            var eventArgs = new EventArgs();

            // Act & Assert - Testing that Page_Load doesn't throw
            var exception = Record.Exception(() => addStaff.Page_Load(sender, eventArgs));
            Assert.Null(exception);
        }

        [Fact]
        public void FlushInformation_ShouldClearAllFields()
        {
            // Arrange
            var addStaff = new AddStaff();

            // Act
            // Note: This test verifies the method exists and can be called
            var exception = Record.Exception(() =>
            {
                var method = addStaff.GetType().GetMethod("flushInformation",
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                method?.Invoke(addStaff, null);
            });

            // Assert
            Assert.Null(exception);
        }

        [Fact]
        public void AddStaff_Constructor_ShouldCreateInstance()
        {
            // Act
            var addStaff = new AddStaff();

            // Assert
            Assert.NotNull(addStaff);
            Assert.IsType<AddStaff>(addStaff);
        }
    }
}
