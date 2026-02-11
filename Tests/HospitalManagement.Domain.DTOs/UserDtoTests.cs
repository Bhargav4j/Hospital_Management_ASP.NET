using HospitalManagement.Domain.DTOs;
using HospitalManagement.Domain.Enums;
using Xunit;

namespace Tests.HospitalManagement.Domain.DTOs;

public class UserDtoTests
{
    [Fact]
    public void UserDto_Constructor_ShouldInitializeDefaultValues()
    {
        // Arrange & Act
        var userDto = new UserDto();

        // Assert
        Assert.Equal(0, userDto.Id);
        Assert.Equal(string.Empty, userDto.Name);
        Assert.Equal(string.Empty, userDto.Email);
        Assert.Equal(string.Empty, userDto.PhoneNo);
        Assert.Equal(string.Empty, userDto.Address);
    }

    [Fact]
    public void UserDto_SetProperties_ShouldSetValues()
    {
        // Arrange
        var userDto = new UserDto();
        var birthDate = new DateTime(1990, 1, 1);

        // Act
        userDto.Id = 1;
        userDto.Name = "John Doe";
        userDto.BirthDate = birthDate;
        userDto.Email = "john@test.com";
        userDto.PhoneNo = "1234567890";
        userDto.Gender = Gender.Male;
        userDto.Address = "123 Main St";
        userDto.UserType = UserType.Patient;

        // Assert
        Assert.Equal(1, userDto.Id);
        Assert.Equal("John Doe", userDto.Name);
        Assert.Equal(birthDate, userDto.BirthDate);
        Assert.Equal("john@test.com", userDto.Email);
        Assert.Equal("1234567890", userDto.PhoneNo);
        Assert.Equal(Gender.Male, userDto.Gender);
        Assert.Equal("123 Main St", userDto.Address);
        Assert.Equal(UserType.Patient, userDto.UserType);
    }
}

public class UserCreateDtoTests
{
    [Fact]
    public void UserCreateDto_Constructor_ShouldInitializeDefaultValues()
    {
        // Arrange & Act
        var userCreateDto = new UserCreateDto();

        // Assert
        Assert.Equal(string.Empty, userCreateDto.Name);
        Assert.Equal(string.Empty, userCreateDto.Email);
        Assert.Equal(string.Empty, userCreateDto.Password);
        Assert.Equal(string.Empty, userCreateDto.PhoneNo);
        Assert.Equal(string.Empty, userCreateDto.Address);
    }

    [Fact]
    public void UserCreateDto_SetProperties_ShouldSetValues()
    {
        // Arrange
        var userCreateDto = new UserCreateDto();
        var birthDate = new DateTime(1990, 1, 1);

        // Act
        userCreateDto.Name = "Jane Doe";
        userCreateDto.BirthDate = birthDate;
        userCreateDto.Email = "jane@test.com";
        userCreateDto.Password = "password123";
        userCreateDto.PhoneNo = "9876543210";
        userCreateDto.Gender = Gender.Female;
        userCreateDto.Address = "456 Oak Ave";
        userCreateDto.UserType = UserType.Doctor;

        // Assert
        Assert.Equal("Jane Doe", userCreateDto.Name);
        Assert.Equal(birthDate, userCreateDto.BirthDate);
        Assert.Equal("jane@test.com", userCreateDto.Email);
        Assert.Equal("password123", userCreateDto.Password);
        Assert.Equal("9876543210", userCreateDto.PhoneNo);
        Assert.Equal(Gender.Female, userCreateDto.Gender);
        Assert.Equal("456 Oak Ave", userCreateDto.Address);
        Assert.Equal(UserType.Doctor, userCreateDto.UserType);
    }
}

public class UserUpdateDtoTests
{
    [Fact]
    public void UserUpdateDto_Constructor_ShouldInitializeDefaultValues()
    {
        // Arrange & Act
        var userUpdateDto = new UserUpdateDto();

        // Assert
        Assert.Equal(string.Empty, userUpdateDto.Name);
        Assert.Equal(string.Empty, userUpdateDto.PhoneNo);
        Assert.Equal(string.Empty, userUpdateDto.Address);
    }

    [Fact]
    public void UserUpdateDto_SetProperties_ShouldSetValues()
    {
        // Arrange
        var userUpdateDto = new UserUpdateDto();
        var birthDate = new DateTime(1990, 1, 1);

        // Act
        userUpdateDto.Name = "Updated Name";
        userUpdateDto.BirthDate = birthDate;
        userUpdateDto.PhoneNo = "1112223333";
        userUpdateDto.Gender = Gender.Other;
        userUpdateDto.Address = "789 Pine Rd";

        // Assert
        Assert.Equal("Updated Name", userUpdateDto.Name);
        Assert.Equal(birthDate, userUpdateDto.BirthDate);
        Assert.Equal("1112223333", userUpdateDto.PhoneNo);
        Assert.Equal(Gender.Other, userUpdateDto.Gender);
        Assert.Equal("789 Pine Rd", userUpdateDto.Address);
    }
}

public class LoginDtoTests
{
    [Fact]
    public void LoginDto_Constructor_ShouldInitializeDefaultValues()
    {
        // Arrange & Act
        var loginDto = new LoginDto();

        // Assert
        Assert.Equal(string.Empty, loginDto.Email);
        Assert.Equal(string.Empty, loginDto.Password);
    }

    [Fact]
    public void LoginDto_SetProperties_ShouldSetValues()
    {
        // Arrange
        var loginDto = new LoginDto();

        // Act
        loginDto.Email = "test@test.com";
        loginDto.Password = "testpass";

        // Assert
        Assert.Equal("test@test.com", loginDto.Email);
        Assert.Equal("testpass", loginDto.Password);
    }
}

public class LoginResultDtoTests
{
    [Fact]
    public void LoginResultDto_Constructor_ShouldInitializeDefaultValues()
    {
        // Arrange & Act
        var loginResultDto = new LoginResultDto();

        // Assert
        Assert.False(loginResultDto.Success);
        Assert.Equal(0, loginResultDto.UserId);
        Assert.Equal(string.Empty, loginResultDto.Message);
    }

    [Fact]
    public void LoginResultDto_SetProperties_ShouldSetValues()
    {
        // Arrange
        var loginResultDto = new LoginResultDto();

        // Act
        loginResultDto.Success = true;
        loginResultDto.UserId = 123;
        loginResultDto.UserType = UserType.Admin;
        loginResultDto.Message = "Login successful";

        // Assert
        Assert.True(loginResultDto.Success);
        Assert.Equal(123, loginResultDto.UserId);
        Assert.Equal(UserType.Admin, loginResultDto.UserType);
        Assert.Equal("Login successful", loginResultDto.Message);
    }

    [Fact]
    public void LoginResultDto_Success_DefaultsToFalse()
    {
        // Arrange & Act
        var loginResultDto = new LoginResultDto();

        // Assert
        Assert.False(loginResultDto.Success);
    }
}
