using System;
using Xunit;
using DBProject;

namespace DBProject.Tests
{
    public class PatientMasterTests
    {
        [Fact]
        public void PatientMaster_Constructor_ShouldCreateInstance()
        {
            // Act
            var master = new PatientMaster();

            // Assert
            Assert.NotNull(master);
            Assert.IsType<PatientMaster>(master);
        }

        [Fact]
        public void Page_Load_ShouldExecuteWithoutException()
        {
            // Arrange
            var master = new PatientMaster();
            var sender = new object();
            var eventArgs = new EventArgs();

            // Act & Assert
            var exception = Record.Exception(() => master.Page_Load(sender, eventArgs));
            Assert.Null(exception);
        }

        [Fact]
        public void PatientMaster_InheritsFromMasterPage()
        {
            // Arrange & Act
            var master = new PatientMaster();

            // Assert
            Assert.IsAssignableFrom<System.Web.UI.MasterPage>(master);
        }
    }
}
