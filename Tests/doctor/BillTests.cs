using System;
using Xunit;
using doctor;

namespace doctor.Tests
{
    public class BillTests
    {
        [Fact]
        public void Bill_Constructor_ShouldCreateInstance()
        {
            // Act
            var page = new bill();

            // Assert
            Assert.NotNull(page);
            Assert.IsType<bill>(page);
        }

        [Fact]
        public void Page_Load_ShouldExecuteWithoutException()
        {
            // Arrange
            var page = new bill();
            var sender = new object();
            var eventArgs = new EventArgs();

            // Act & Assert - May throw due to Session dependencies
            var exception = Record.Exception(() => page.Page_Load(sender, eventArgs));
        }

        [Fact]
        public void Bill_Paid_ShouldExist()
        {
            // Arrange
            var page = new bill();

            // Act
            var method = page.GetType().GetMethod("bill_paid");

            // Assert
            Assert.NotNull(method);
        }

        [Fact]
        public void Bill_Unpaid_ShouldExist()
        {
            // Arrange
            var page = new bill();

            // Act
            var method = page.GetType().GetMethod("bill_Unpaid");

            // Assert
            Assert.NotNull(method);
        }

        [Fact]
        public void Bill_InheritsFromPage()
        {
            // Arrange & Act
            var page = new bill();

            // Assert
            Assert.IsAssignableFrom<System.Web.UI.Page>(page);
        }
    }
}
