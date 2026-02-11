using HospitalManagement.Web.ViewModels;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace Tests.HospitalManagement.Web.ViewModels;

public class LoginViewModelTests
{
    [Fact]
    public void LoginViewModel_Constructor_ShouldInitializeDefaultValues()
    {
        // Arrange & Act
        var viewModel = new LoginViewModel();

        // Assert
        Assert.Equal(string.Empty, viewModel.Email);
        Assert.Equal(string.Empty, viewModel.Password);
    }

    [Fact]
    public void LoginViewModel_SetProperties_ShouldSetValues()
    {
        // Arrange
        var viewModel = new LoginViewModel();

        // Act
        viewModel.Email = "test@test.com";
        viewModel.Password = "password123";

        // Assert
        Assert.Equal("test@test.com", viewModel.Email);
        Assert.Equal("password123", viewModel.Password);
    }

    [Fact]
    public void LoginViewModel_Email_ShouldBeRequired()
    {
        // Arrange
        var viewModel = new LoginViewModel { Email = "", Password = "password" };
        var context = new ValidationContext(viewModel);
        var results = new List<ValidationResult>();

        // Act
        var isValid = Validator.TryValidateObject(viewModel, context, results, true);

        // Assert
        Assert.False(isValid);
        Assert.Contains(results, r => r.MemberNames.Contains("Email"));
    }

    [Fact]
    public void LoginViewModel_Password_ShouldBeRequired()
    {
        // Arrange
        var viewModel = new LoginViewModel { Email = "test@test.com", Password = "" };
        var context = new ValidationContext(viewModel);
        var results = new List<ValidationResult>();

        // Act
        var isValid = Validator.TryValidateObject(viewModel, context, results, true);

        // Assert
        Assert.False(isValid);
        Assert.Contains(results, r => r.MemberNames.Contains("Password"));
    }

    [Fact]
    public void LoginViewModel_Email_ShouldBeValidEmailAddress()
    {
        // Arrange
        var viewModel = new LoginViewModel { Email = "invalid-email", Password = "password" };
        var context = new ValidationContext(viewModel);
        var results = new List<ValidationResult>();

        // Act
        var isValid = Validator.TryValidateObject(viewModel, context, results, true);

        // Assert
        Assert.False(isValid);
        Assert.Contains(results, r => r.MemberNames.Contains("Email"));
    }

    [Fact]
    public void LoginViewModel_ValidData_ShouldPassValidation()
    {
        // Arrange
        var viewModel = new LoginViewModel { Email = "test@test.com", Password = "password123" };
        var context = new ValidationContext(viewModel);
        var results = new List<ValidationResult>();

        // Act
        var isValid = Validator.TryValidateObject(viewModel, context, results, true);

        // Assert
        Assert.True(isValid);
        Assert.Empty(results);
    }
}
