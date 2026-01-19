using Xunit;
using ClinicManagement.Application.DTOs;
using System;

namespace ClinicManagement.Tests.Unit.Application.DTOs;

public class UserDtoTests
{
    [Fact]
    public void UserDto_Constructor_ShouldInitializeWithDefaultValues()
    {
        // Arrange & Act
        var userDto = new UserDto();

        // Assert
        Assert.Equal(0, userDto.Id);
        Assert.Equal(string.Empty, userDto.Name);
        Assert.Equal(string.Empty, userDto.Email);
        Assert.Equal(string.Empty, userDto.PhoneNumber);
        Assert.Equal(string.Empty, userDto.Address);
        Assert.Equal(default(DateTime), userDto.BirthDate);
        Assert.Equal(string.Empty, userDto.Gender);
        Assert.Equal(0, userDto.Type);
    }

    [Fact]
    public void UserDto_AllProperties_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var userDto = new UserDto();
        var expectedId = 1;
        var expectedName = "John Doe";
        var expectedEmail = "john@example.com";
        var expectedPhoneNumber = "1234567890";
        var expectedAddress = "123 Main St";
        var expectedBirthDate = new DateTime(1990, 5, 15);
        var expectedGender = "Male";
        var expectedType = 1;

        // Act
        userDto.Id = expectedId;
        userDto.Name = expectedName;
        userDto.Email = expectedEmail;
        userDto.PhoneNumber = expectedPhoneNumber;
        userDto.Address = expectedAddress;
        userDto.BirthDate = expectedBirthDate;
        userDto.Gender = expectedGender;
        userDto.Type = expectedType;

        // Assert
        Assert.Equal(expectedId, userDto.Id);
        Assert.Equal(expectedName, userDto.Name);
        Assert.Equal(expectedEmail, userDto.Email);
        Assert.Equal(expectedPhoneNumber, userDto.PhoneNumber);
        Assert.Equal(expectedAddress, userDto.Address);
        Assert.Equal(expectedBirthDate, userDto.BirthDate);
        Assert.Equal(expectedGender, userDto.Gender);
        Assert.Equal(expectedType, userDto.Type);
    }

    [Theory]
    [InlineData("")]
    [InlineData("test@example.com")]
    [InlineData("user@domain.co.uk")]
    public void UserDto_Email_ShouldAcceptVariousFormats(string email)
    {
        // Arrange
        var userDto = new UserDto();

        // Act
        userDto.Email = email;

        // Assert
        Assert.Equal(email, userDto.Email);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public void UserDto_Type_ShouldAcceptVariousValues(int type)
    {
        // Arrange
        var userDto = new UserDto();

        // Act
        userDto.Type = type;

        // Assert
        Assert.Equal(type, userDto.Type);
    }
}

public class CreateUserDtoTests
{
    [Fact]
    public void CreateUserDto_Constructor_ShouldInitializeWithDefaultValues()
    {
        // Arrange & Act
        var createUserDto = new CreateUserDto();

        // Assert
        Assert.Equal(string.Empty, createUserDto.Name);
        Assert.Equal(string.Empty, createUserDto.Email);
        Assert.Equal(string.Empty, createUserDto.Password);
        Assert.Equal(string.Empty, createUserDto.PhoneNumber);
        Assert.Equal(string.Empty, createUserDto.Address);
        Assert.Equal(default(DateTime), createUserDto.BirthDate);
        Assert.Equal(string.Empty, createUserDto.Gender);
        Assert.Equal(0, createUserDto.Type);
    }

    [Fact]
    public void CreateUserDto_AllProperties_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var createUserDto = new CreateUserDto();
        var expectedName = "Jane Smith";
        var expectedEmail = "jane@example.com";
        var expectedPassword = "SecurePass123!";
        var expectedPhoneNumber = "9876543210";
        var expectedAddress = "456 Oak Ave";
        var expectedBirthDate = new DateTime(1985, 10, 20);
        var expectedGender = "Female";
        var expectedType = 2;

        // Act
        createUserDto.Name = expectedName;
        createUserDto.Email = expectedEmail;
        createUserDto.Password = expectedPassword;
        createUserDto.PhoneNumber = expectedPhoneNumber;
        createUserDto.Address = expectedAddress;
        createUserDto.BirthDate = expectedBirthDate;
        createUserDto.Gender = expectedGender;
        createUserDto.Type = expectedType;

        // Assert
        Assert.Equal(expectedName, createUserDto.Name);
        Assert.Equal(expectedEmail, createUserDto.Email);
        Assert.Equal(expectedPassword, createUserDto.Password);
        Assert.Equal(expectedPhoneNumber, createUserDto.PhoneNumber);
        Assert.Equal(expectedAddress, createUserDto.Address);
        Assert.Equal(expectedBirthDate, createUserDto.BirthDate);
        Assert.Equal(expectedGender, createUserDto.Gender);
        Assert.Equal(expectedType, createUserDto.Type);
    }

    [Theory]
    [InlineData("")]
    [InlineData("password")]
    [InlineData("P@ssw0rd!")]
    [InlineData("VeryLongAndComplexPassword123456!@#")]
    public void CreateUserDto_Password_ShouldAcceptVariousFormats(string password)
    {
        // Arrange
        var createUserDto = new CreateUserDto();

        // Act
        createUserDto.Password = password;

        // Assert
        Assert.Equal(password, createUserDto.Password);
    }

    [Fact]
    public void CreateUserDto_BirthDate_ShouldHandleVariousDates()
    {
        // Arrange
        var createUserDto = new CreateUserDto();
        var birthDate = new DateTime(2000, 1, 1);

        // Act
        createUserDto.BirthDate = birthDate;

        // Assert
        Assert.Equal(birthDate, createUserDto.BirthDate);
    }
}

public class LoginDtoTests
{
    [Fact]
    public void LoginDto_Constructor_ShouldInitializeWithDefaultValues()
    {
        // Arrange & Act
        var loginDto = new LoginDto();

        // Assert
        Assert.Equal(string.Empty, loginDto.Email);
        Assert.Equal(string.Empty, loginDto.Password);
    }

    [Fact]
    public void LoginDto_Email_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var loginDto = new LoginDto();
        var expectedEmail = "user@example.com";

        // Act
        loginDto.Email = expectedEmail;

        // Assert
        Assert.Equal(expectedEmail, loginDto.Email);
    }

    [Fact]
    public void LoginDto_Password_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var loginDto = new LoginDto();
        var expectedPassword = "MyPassword123";

        // Act
        loginDto.Password = expectedPassword;

        // Assert
        Assert.Equal(expectedPassword, loginDto.Password);
    }

    [Fact]
    public void LoginDto_AllProperties_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var loginDto = new LoginDto();
        var expectedEmail = "admin@example.com";
        var expectedPassword = "AdminPass456";

        // Act
        loginDto.Email = expectedEmail;
        loginDto.Password = expectedPassword;

        // Assert
        Assert.Equal(expectedEmail, loginDto.Email);
        Assert.Equal(expectedPassword, loginDto.Password);
    }

    [Theory]
    [InlineData("", "")]
    [InlineData("test@example.com", "password")]
    [InlineData("user@domain.com", "P@ssw0rd!")]
    public void LoginDto_ShouldAcceptVariousCombinations(string email, string password)
    {
        // Arrange
        var loginDto = new LoginDto();

        // Act
        loginDto.Email = email;
        loginDto.Password = password;

        // Assert
        Assert.Equal(email, loginDto.Email);
        Assert.Equal(password, loginDto.Password);
    }
}

public class LoginResponseDtoTests
{
    [Fact]
    public void LoginResponseDto_Constructor_ShouldInitializeWithDefaultValues()
    {
        // Arrange & Act
        var loginResponseDto = new LoginResponseDto();

        // Assert
        Assert.Equal(0, loginResponseDto.UserId);
        Assert.Equal(0, loginResponseDto.UserType);
        Assert.Equal(string.Empty, loginResponseDto.Token);
        Assert.False(loginResponseDto.Success);
    }

    [Fact]
    public void LoginResponseDto_UserId_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var loginResponseDto = new LoginResponseDto();
        var expectedUserId = 123;

        // Act
        loginResponseDto.UserId = expectedUserId;

        // Assert
        Assert.Equal(expectedUserId, loginResponseDto.UserId);
    }

    [Fact]
    public void LoginResponseDto_UserType_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var loginResponseDto = new LoginResponseDto();
        var expectedUserType = 2;

        // Act
        loginResponseDto.UserType = expectedUserType;

        // Assert
        Assert.Equal(expectedUserType, loginResponseDto.UserType);
    }

    [Fact]
    public void LoginResponseDto_Token_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var loginResponseDto = new LoginResponseDto();
        var expectedToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9";

        // Act
        loginResponseDto.Token = expectedToken;

        // Assert
        Assert.Equal(expectedToken, loginResponseDto.Token);
    }

    [Fact]
    public void LoginResponseDto_Success_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var loginResponseDto = new LoginResponseDto();

        // Act
        loginResponseDto.Success = true;

        // Assert
        Assert.True(loginResponseDto.Success);
    }

    [Fact]
    public void LoginResponseDto_AllProperties_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var loginResponseDto = new LoginResponseDto();
        var expectedUserId = 456;
        var expectedUserType = 3;
        var expectedToken = "abc123token";
        var expectedSuccess = true;

        // Act
        loginResponseDto.UserId = expectedUserId;
        loginResponseDto.UserType = expectedUserType;
        loginResponseDto.Token = expectedToken;
        loginResponseDto.Success = expectedSuccess;

        // Assert
        Assert.Equal(expectedUserId, loginResponseDto.UserId);
        Assert.Equal(expectedUserType, loginResponseDto.UserType);
        Assert.Equal(expectedToken, loginResponseDto.Token);
        Assert.Equal(expectedSuccess, loginResponseDto.Success);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void LoginResponseDto_Success_ShouldAcceptBooleanValues(bool success)
    {
        // Arrange
        var loginResponseDto = new LoginResponseDto();

        // Act
        loginResponseDto.Success = success;

        // Assert
        Assert.Equal(success, loginResponseDto.Success);
    }

    [Fact]
    public void LoginResponseDto_SuccessfulLogin_ShouldHaveTokenAndTrueSuccess()
    {
        // Arrange
        var loginResponseDto = new LoginResponseDto();

        // Act
        loginResponseDto.UserId = 100;
        loginResponseDto.UserType = 1;
        loginResponseDto.Token = "validToken123";
        loginResponseDto.Success = true;

        // Assert
        Assert.True(loginResponseDto.Success);
        Assert.NotEmpty(loginResponseDto.Token);
        Assert.NotEqual(0, loginResponseDto.UserId);
    }

    [Fact]
    public void LoginResponseDto_FailedLogin_ShouldHaveEmptyTokenAndFalseSuccess()
    {
        // Arrange
        var loginResponseDto = new LoginResponseDto();

        // Act
        loginResponseDto.Success = false;

        // Assert
        Assert.False(loginResponseDto.Success);
        Assert.Equal(string.Empty, loginResponseDto.Token);
    }
}
