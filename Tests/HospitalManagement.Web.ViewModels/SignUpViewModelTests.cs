using HospitalManagement.Domain.Enums;
using HospitalManagement.Web.ViewModels;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace Tests.HospitalManagement.Web.ViewModels;

public class SignUpViewModelTests
{
    [Fact]
    public void SignUpViewModel_Constructor_ShouldInitializeDefaultValues()
    {
        // Arrange & Act
        var viewModel = new SignUpViewModel();

        // Assert
        Assert.Equal(string.Empty, viewModel.Name);
        Assert.Equal(string.Empty, viewModel.Email);
        Assert.Equal(string.Empty, viewModel.Password);
        Assert.Equal(string.Empty, viewModel.PhoneNo);
        Assert.Equal(string.Empty, viewModel.Address);
    }

    [Fact]
    public void SignUpViewModel_SetProperties_ShouldSetValues()
    {
        // Arrange
        var viewModel = new SignUpViewModel();
        var birthDate = new DateTime(1990, 1, 1);

        // Act
        viewModel.Name = "John Doe";
        viewModel.BirthDate = birthDate;
        viewModel.Email = "john@test.com";
        viewModel.Password = "password123";
        viewModel.PhoneNo = "1234567890";
        viewModel.Gender = Gender.Male;
        viewModel.Address = "123 Main St";

        // Assert
        Assert.Equal("John Doe", viewModel.Name);
        Assert.Equal(birthDate, viewModel.BirthDate);
        Assert.Equal("john@test.com", viewModel.Email);
        Assert.Equal("password123", viewModel.Password);
        Assert.Equal("1234567890", viewModel.PhoneNo);
        Assert.Equal(Gender.Male, viewModel.Gender);
        Assert.Equal("123 Main St", viewModel.Address);
    }

    [Fact]
    public void SignUpViewModel_Name_ShouldBeRequired()
    {
        // Arrange
        var viewModel = new SignUpViewModel
        {
            Name = "",
            Email = "test@test.com",
            Password = "password123",
            PhoneNo = "1234567890",
            Address = "123 St"
        };
        var context = new ValidationContext(viewModel);
        var results = new List<ValidationResult>();

        // Act
        var isValid = Validator.TryValidateObject(viewModel, context, results, true);

        // Assert
        Assert.False(isValid);
        Assert.Contains(results, r => r.MemberNames.Contains("Name"));
    }

    [Fact]
    public void SignUpViewModel_Email_ShouldBeRequired()
    {
        // Arrange
        var viewModel = new SignUpViewModel
        {
            Name = "John Doe",
            Email = "",
            Password = "password123",
            PhoneNo = "1234567890",
            Address = "123 St"
        };
        var context = new ValidationContext(viewModel);
        var results = new List<ValidationResult>();

        // Act
        var isValid = Validator.TryValidateObject(viewModel, context, results, true);

        // Assert
        Assert.False(isValid);
        Assert.Contains(results, r => r.MemberNames.Contains("Email"));
    }

    [Fact]
    public void SignUpViewModel_Password_ShouldBeRequired()
    {
        // Arrange
        var viewModel = new SignUpViewModel
        {
            Name = "John Doe",
            Email = "test@test.com",
            Password = "",
            PhoneNo = "1234567890",
            Address = "123 St"
        };
        var context = new ValidationContext(viewModel);
        var results = new List<ValidationResult>();

        // Act
        var isValid = Validator.TryValidateObject(viewModel, context, results, true);

        // Assert
        Assert.False(isValid);
        Assert.Contains(results, r => r.MemberNames.Contains("Password"));
    }

    [Fact]
    public void SignUpViewModel_Password_ShouldHaveMinimumLength()
    {
        // Arrange
        var viewModel = new SignUpViewModel
        {
            Name = "John Doe",
            Email = "test@test.com",
            Password = "12345",
            PhoneNo = "1234567890",
            Address = "123 St"
        };
        var context = new ValidationContext(viewModel);
        var results = new List<ValidationResult>();

        // Act
        var isValid = Validator.TryValidateObject(viewModel, context, results, true);

        // Assert
        Assert.False(isValid);
        Assert.Contains(results, r => r.MemberNames.Contains("Password"));
    }

    [Fact]
    public void SignUpViewModel_Email_ShouldBeValidEmailAddress()
    {
        // Arrange
        var viewModel = new SignUpViewModel
        {
            Name = "John Doe",
            Email = "invalid-email",
            Password = "password123",
            PhoneNo = "1234567890",
            Address = "123 St"
        };
        var context = new ValidationContext(viewModel);
        var results = new List<ValidationResult>();

        // Act
        var isValid = Validator.TryValidateObject(viewModel, context, results, true);

        // Assert
        Assert.False(isValid);
        Assert.Contains(results, r => r.MemberNames.Contains("Email"));
    }

    [Fact]
    public void SignUpViewModel_Name_ShouldNotExceedMaxLength()
    {
        // Arrange
        var viewModel = new SignUpViewModel
        {
            Name = new string('a', 101),
            Email = "test@test.com",
            Password = "password123",
            PhoneNo = "1234567890",
            Address = "123 St"
        };
        var context = new ValidationContext(viewModel);
        var results = new List<ValidationResult>();

        // Act
        var isValid = Validator.TryValidateObject(viewModel, context, results, true);

        // Assert
        Assert.False(isValid);
        Assert.Contains(results, r => r.MemberNames.Contains("Name"));
    }

    [Fact]
    public void SignUpViewModel_Address_ShouldNotExceedMaxLength()
    {
        // Arrange
        var viewModel = new SignUpViewModel
        {
            Name = "John Doe",
            Email = "test@test.com",
            Password = "password123",
            PhoneNo = "1234567890",
            Address = new string('a', 501)
        };
        var context = new ValidationContext(viewModel);
        var results = new List<ValidationResult>();

        // Act
        var isValid = Validator.TryValidateObject(viewModel, context, results, true);

        // Assert
        Assert.False(isValid);
        Assert.Contains(results, r => r.MemberNames.Contains("Address"));
    }
}
