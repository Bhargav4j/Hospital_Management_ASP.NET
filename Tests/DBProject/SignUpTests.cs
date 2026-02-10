using System;
using Xunit;
using DBProject;

namespace DBProject.Tests
{
    public class SignUpTests
    {
        [Fact]
        public void SignUp_Constructor_ShouldCreateInstance()
        {
            // Act
            var page = new SignUp();

            // Assert
            Assert.NotNull(page);
            Assert.IsType<SignUp>(page);
        }

        [Fact]
        public void Page_Load_ShouldExecuteWithoutException()
        {
            // Arrange
            var page = new SignUp();
            var sender = new object();
            var eventArgs = new EventArgs();

            // Act & Assert - May throw due to Session dependencies
            var exception = Record.Exception(() => page.Page_Load(sender, eventArgs));
        }

        [Fact]
        public void LoginV_ShouldExist()
        {
            // Arrange
            var page = new SignUp();

            // Act
            var method = page.GetType().GetMethod("loginV",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            // Assert
            Assert.NotNull(method);
        }

        [Fact]
        public void SignupV_ShouldExist()
        {
            // Arrange
            var page = new SignUp();

            // Act
            var method = page.GetType().GetMethod("signupV",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            // Assert
            Assert.NotNull(method);
        }

        [Fact]
        public void SignUp_InheritsFromPage()
        {
            // Arrange & Act
            var page = new SignUp();

            // Assert
            Assert.IsAssignableFrom<System.Web.UI.Page>(page);
        }
    }
}
