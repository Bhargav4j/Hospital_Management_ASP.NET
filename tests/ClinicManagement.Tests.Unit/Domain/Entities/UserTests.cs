using Xunit;
using ClinicManagement.Domain.Entities;
using System;

namespace ClinicManagement.Tests.Unit.Domain.Entities;

public class UserTests
{
    [Fact]
    public void User_Constructor_ShouldInitializeWithDefaultValues()
    {
        // Arrange & Act
        var user = new User();

        // Assert
        Assert.Equal(0, user.Id);
        Assert.Equal(string.Empty, user.Name);
        Assert.Equal(string.Empty, user.Email);
        Assert.Equal(string.Empty, user.PasswordHash);
        Assert.Equal(string.Empty, user.PhoneNumber);
        Assert.Equal(string.Empty, user.Address);
        Assert.Equal(string.Empty, user.Gender);
        Assert.Equal(0, user.Type);
        Assert.Equal(default(DateTime), user.BirthDate);
        Assert.Equal(default(DateTime), user.CreatedAt);
        Assert.Null(user.UpdatedAt);
    }

    [Fact]
    public void User_Id_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var user = new User();
        var expectedId = 123;

        // Act
        user.Id = expectedId;

        // Assert
        Assert.Equal(expectedId, user.Id);
    }

    [Fact]
    public void User_Name_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var user = new User();
        var expectedName = "John Doe";

        // Act
        user.Name = expectedName;

        // Assert
        Assert.Equal(expectedName, user.Name);
    }

    [Fact]
    public void User_Email_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var user = new User();
        var expectedEmail = "john.doe@example.com";

        // Act
        user.Email = expectedEmail;

        // Assert
        Assert.Equal(expectedEmail, user.Email);
    }

    [Fact]
    public void User_PasswordHash_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var user = new User();
        var expectedHash = "hashed_password_123";

        // Act
        user.PasswordHash = expectedHash;

        // Assert
        Assert.Equal(expectedHash, user.PasswordHash);
    }

    [Fact]
    public void User_PhoneNumber_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var user = new User();
        var expectedPhoneNumber = "+1234567890";

        // Act
        user.PhoneNumber = expectedPhoneNumber;

        // Assert
        Assert.Equal(expectedPhoneNumber, user.PhoneNumber);
    }

    [Fact]
    public void User_Address_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var user = new User();
        var expectedAddress = "123 Main St, City, Country";

        // Act
        user.Address = expectedAddress;

        // Assert
        Assert.Equal(expectedAddress, user.Address);
    }

    [Fact]
    public void User_BirthDate_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var user = new User();
        var expectedBirthDate = new DateTime(1990, 5, 15);

        // Act
        user.BirthDate = expectedBirthDate;

        // Assert
        Assert.Equal(expectedBirthDate, user.BirthDate);
    }

    [Fact]
    public void User_Gender_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var user = new User();
        var expectedGender = "Male";

        // Act
        user.Gender = expectedGender;

        // Assert
        Assert.Equal(expectedGender, user.Gender);
    }

    [Fact]
    public void User_Type_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var user = new User();
        var expectedType = 1;

        // Act
        user.Type = expectedType;

        // Assert
        Assert.Equal(expectedType, user.Type);
    }

    [Fact]
    public void User_CreatedAt_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var user = new User();
        var expectedCreatedAt = DateTime.Now;

        // Act
        user.CreatedAt = expectedCreatedAt;

        // Assert
        Assert.Equal(expectedCreatedAt, user.CreatedAt);
    }

    [Fact]
    public void User_UpdatedAt_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var user = new User();
        var expectedUpdatedAt = DateTime.Now;

        // Act
        user.UpdatedAt = expectedUpdatedAt;

        // Assert
        Assert.Equal(expectedUpdatedAt, user.UpdatedAt);
    }

    [Fact]
    public void User_UpdatedAt_ShouldAcceptNullValue()
    {
        // Arrange
        var user = new User();

        // Act
        user.UpdatedAt = null;

        // Assert
        Assert.Null(user.UpdatedAt);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(999999)]
    public void User_Id_ShouldAcceptVariousIdValues(int id)
    {
        // Arrange
        var user = new User();

        // Act
        user.Id = id;

        // Assert
        Assert.Equal(id, user.Id);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("a@b.c")]
    [InlineData("test.email@domain.com")]
    public void User_Email_ShouldAcceptVariousEmailFormats(string email)
    {
        // Arrange
        var user = new User();

        // Act
        user.Email = email;

        // Assert
        Assert.Equal(email, user.Email);
    }

    [Fact]
    public void User_AllProperties_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var user = new User();
        var expectedId = 1;
        var expectedName = "Jane Smith";
        var expectedEmail = "jane@example.com";
        var expectedPasswordHash = "hash123";
        var expectedPhoneNumber = "123456789";
        var expectedAddress = "456 Oak St";
        var expectedBirthDate = new DateTime(1985, 10, 20);
        var expectedGender = "Female";
        var expectedType = 2;
        var expectedCreatedAt = DateTime.Now;
        var expectedUpdatedAt = DateTime.Now.AddHours(1);

        // Act
        user.Id = expectedId;
        user.Name = expectedName;
        user.Email = expectedEmail;
        user.PasswordHash = expectedPasswordHash;
        user.PhoneNumber = expectedPhoneNumber;
        user.Address = expectedAddress;
        user.BirthDate = expectedBirthDate;
        user.Gender = expectedGender;
        user.Type = expectedType;
        user.CreatedAt = expectedCreatedAt;
        user.UpdatedAt = expectedUpdatedAt;

        // Assert
        Assert.Equal(expectedId, user.Id);
        Assert.Equal(expectedName, user.Name);
        Assert.Equal(expectedEmail, user.Email);
        Assert.Equal(expectedPasswordHash, user.PasswordHash);
        Assert.Equal(expectedPhoneNumber, user.PhoneNumber);
        Assert.Equal(expectedAddress, user.Address);
        Assert.Equal(expectedBirthDate, user.BirthDate);
        Assert.Equal(expectedGender, user.Gender);
        Assert.Equal(expectedType, user.Type);
        Assert.Equal(expectedCreatedAt, user.CreatedAt);
        Assert.Equal(expectedUpdatedAt, user.UpdatedAt);
    }
}
