using System;
using Xunit;
using doctor;

namespace doctor.Tests
{
    public class DoctorMasterTests
    {
        [Fact]
        public void DoctorMaster_Constructor_ShouldCreateInstance()
        {
            // Act
            var master = new doctormaster();

            // Assert
            Assert.NotNull(master);
            Assert.IsType<doctormaster>(master);
        }

        [Fact]
        public void Page_Load_ShouldExecuteWithoutException()
        {
            // Arrange
            var master = new doctormaster();
            var sender = new object();
            var eventArgs = new EventArgs();

            // Act & Assert
            var exception = Record.Exception(() => master.Page_Load(sender, eventArgs));
            Assert.Null(exception);
        }

        [Fact]
        public void DoctorMaster_InheritsFromMasterPage()
        {
            // Arrange & Act
            var master = new doctormaster();

            // Assert
            Assert.IsAssignableFrom<System.Web.UI.MasterPage>(master);
        }
    }
}
