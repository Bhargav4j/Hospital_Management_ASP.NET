using System;
using Xunit;
using doctor;

namespace doctor.Tests
{
    public class HistoryUpdateTests
    {
        [Fact]
        public void HistoryUpdate_Constructor_ShouldCreateInstance()
        {
            // Act
            var page = new Historyupdate();

            // Assert
            Assert.NotNull(page);
            Assert.IsType<Historyupdate>(page);
        }

        [Fact]
        public void Page_Load_ShouldExecuteWithoutException()
        {
            // Arrange
            var page = new Historyupdate();
            var sender = new object();
            var eventArgs = new EventArgs();

            // Act & Assert
            var exception = Record.Exception(() => page.Page_Load(sender, eventArgs));
            Assert.Null(exception);
        }

        [Fact]
        public void SaveInDatabase_ShouldExist()
        {
            // Arrange
            var page = new Historyupdate();

            // Act
            var method = page.GetType().GetMethod("saveindatabase");

            // Assert
            Assert.NotNull(method);
        }

        [Fact]
        public void Generate_Bill_ShouldExist()
        {
            // Arrange
            var page = new Historyupdate();

            // Act
            var method = page.GetType().GetMethod("generate_bill");

            // Assert
            Assert.NotNull(method);
        }

        [Fact]
        public void HistoryUpdate_InheritsFromPage()
        {
            // Arrange & Act
            var page = new Historyupdate();

            // Assert
            Assert.IsAssignableFrom<System.Web.UI.Page>(page);
        }
    }
}
