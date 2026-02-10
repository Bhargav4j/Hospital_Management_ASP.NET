using System;
using Xunit;
using DB_Project;
using System.Web.UI.WebControls;

namespace DB_Project.Tests
{
    public class DoctorRegistrationFormTests
    {
        [Fact]
        public void DoctorRegistrationForm_Constructor_ShouldCreateInstance()
        {
            // Act
            var form = new DoctorRegistrationForm();

            // Assert
            Assert.NotNull(form);
            Assert.IsType<DoctorRegistrationForm>(form);
        }

        [Fact]
        public void Page_Load_ShouldExecuteWithoutException()
        {
            // Arrange
            var form = new DoctorRegistrationForm();
            var sender = new object();
            var eventArgs = new EventArgs();

            // Act & Assert
            var exception = Record.Exception(() => form.Page_Load(sender, eventArgs));
            Assert.Null(exception);
        }

        [Fact]
        public void ValidateDoctorEmail_ShouldExist()
        {
            // Arrange
            var form = new DoctorRegistrationForm();

            // Act
            var method = form.GetType().GetMethod("ValidateDoctorEmail",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            // Assert
            Assert.NotNull(method);
        }

        [Fact]
        public void DoctorRegister_ShouldExist()
        {
            // Arrange
            var form = new DoctorRegistrationForm();

            // Act
            var method = form.GetType().GetMethod("DoctorRegister",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            // Assert
            Assert.NotNull(method);
        }

        [Fact]
        public void FlushInformation_ShouldExist()
        {
            // Arrange
            var form = new DoctorRegistrationForm();

            // Act
            var method = form.GetType().GetMethod("flushInformation",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            // Assert
            Assert.NotNull(method);
        }

        [Fact]
        public void DepartmentValidate_ShouldExist()
        {
            // Arrange
            var form = new DoctorRegistrationForm();

            // Act
            var method = form.GetType().GetMethod("DepartmentValidate",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            // Assert
            Assert.NotNull(method);
        }

        [Fact]
        public void DoctorRegistrationForm_InheritsFromPage()
        {
            // Arrange & Act
            var form = new DoctorRegistrationForm();

            // Assert
            Assert.IsAssignableFrom<System.Web.UI.Page>(form);
        }
    }
}
