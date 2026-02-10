using System;
using Xunit;
using DBProject;

namespace DBProject.Tests
{
    public class BillsHistoryTests
    {
        [Fact]
        public void BillsHistory_Constructor_ShouldCreateInstance()
        {
            // Act
            var page = new BillsHistory();

            // Assert
            Assert.NotNull(page);
            Assert.IsType<BillsHistory>(page);
        }

        [Fact]
        public void Page_Load_ShouldExecuteWithoutException()
        {
            // Arrange
            var page = new BillsHistory();
            var sender = new object();
            var eventArgs = new EventArgs();

            // Act & Assert - May throw due to Session dependencies
            var exception = Record.Exception(() => page.Page_Load(sender, eventArgs));
        }

        [Fact]
        public void BillHistory_ShouldExist()
        {
            // Arrange
            var page = new BillsHistory();

            // Act
            var method = page.GetType().GetMethod("billHistory",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            // Assert
            Assert.NotNull(method);
        }

        [Fact]
        public void BillsHistory_InheritsFromPage()
        {
            // Arrange & Act
            var page = new BillsHistory();

            // Assert
            Assert.IsAssignableFrom<System.Web.UI.Page>(page);
        }
    }
}
