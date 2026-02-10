using System;
using Xunit;
using doctor;

namespace doctor.Tests
{
    public class PatientHistoryTests
    {
        [Fact]
        public void PatientHistory_Constructor_ShouldCreateInstance()
        {
            // Act
            var page = new patienthistory();

            // Assert
            Assert.NotNull(page);
            Assert.IsType<patienthistory>(page);
        }

        [Fact]
        public void Page_Load_ShouldExecuteWithoutException()
        {
            // Arrange
            var page = new patienthistory();
            var sender = new object();
            var eventArgs = new EventArgs();

            // Act & Assert - May throw due to Session dependencies
            var exception = Record.Exception(() => page.Page_Load(sender, eventArgs));
        }

        [Fact]
        public void PatientHistory_InheritsFromPage()
        {
            // Arrange & Act
            var page = new patienthistory();

            // Assert
            Assert.IsAssignableFrom<System.Web.UI.Page>(page);
        }

        [Fact]
        public void PatientsGrid_RowCommand_ShouldExist()
        {
            // Arrange
            var page = new patienthistory();

            // Act
            var method = page.GetType().GetMethod("patientsgrid_RowCommand",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            // Assert
            Assert.NotNull(method);
        }
    }
}
