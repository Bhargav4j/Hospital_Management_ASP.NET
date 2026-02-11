using HospitalManagement.Domain.Entities;
using HospitalManagement.Domain.Enums;
using Xunit;

namespace Tests.HospitalManagement.Domain.Entities;

public class UserTests
{
    [Fact]
    public void User_Constructor_ShouldInitializeDefaultValues()
    {
        // Arrange & Act
        var user = new User();

        // Assert
        Assert.Equal(0, user.Id);
        Assert.Equal(string.Empty, user.Name);
        Assert.Equal(string.Empty, user.Email);
        Assert.Equal(string.Empty, user.Password);
        Assert.Equal(string.Empty, user.PhoneNo);
        Assert.Equal(string.Empty, user.Address);
        Assert.Equal("System", user.CreatedBy);
        Assert.False(user.IsActive);
        Assert.NotNull(user.AppointmentsAsPatient);
        Assert.NotNull(user.AppointmentsAsDoctor);
        Assert.NotNull(user.Bills);
        Assert.NotNull(user.Feedbacks);
        Assert.NotNull(user.MedicalHistories);
    }

    [Fact]
    public void User_SetProperties_ShouldSetValues()
    {
        // Arrange
        var user = new User();
        var birthDate = new DateTime(1990, 1, 1);
        var createdDate = DateTime.UtcNow;

        // Act
        user.Id = 1;
        user.Name = "John Doe";
        user.BirthDate = birthDate;
        user.Email = "john@test.com";
        user.Password = "password123";
        user.PhoneNo = "1234567890";
        user.Gender = Gender.Male;
        user.Address = "123 Main St";
        user.UserType = UserType.Patient;
        user.CreatedDate = createdDate;
        user.IsActive = true;

        // Assert
        Assert.Equal(1, user.Id);
        Assert.Equal("John Doe", user.Name);
        Assert.Equal(birthDate, user.BirthDate);
        Assert.Equal("john@test.com", user.Email);
        Assert.Equal("password123", user.Password);
        Assert.Equal("1234567890", user.PhoneNo);
        Assert.Equal(Gender.Male, user.Gender);
        Assert.Equal("123 Main St", user.Address);
        Assert.Equal(UserType.Patient, user.UserType);
        Assert.Equal(createdDate, user.CreatedDate);
        Assert.True(user.IsActive);
    }

    [Fact]
    public void User_ModifiedDate_CanBeNull()
    {
        // Arrange & Act
        var user = new User();

        // Assert
        Assert.Null(user.ModifiedDate);
    }

    [Fact]
    public void User_ModifiedDate_CanBeSet()
    {
        // Arrange
        var user = new User();
        var modifiedDate = DateTime.UtcNow;

        // Act
        user.ModifiedDate = modifiedDate;

        // Assert
        Assert.Equal(modifiedDate, user.ModifiedDate);
    }

    [Fact]
    public void User_ModifiedBy_CanBeNull()
    {
        // Arrange & Act
        var user = new User();

        // Assert
        Assert.Null(user.ModifiedBy);
    }

    [Fact]
    public void User_ModifiedBy_CanBeSet()
    {
        // Arrange
        var user = new User();

        // Act
        user.ModifiedBy = "Admin";

        // Assert
        Assert.Equal("Admin", user.ModifiedBy);
    }

    [Theory]
    [InlineData(UserType.Patient)]
    [InlineData(UserType.Doctor)]
    [InlineData(UserType.Admin)]
    public void User_UserType_CanBeSetToAnyType(UserType userType)
    {
        // Arrange
        var user = new User();

        // Act
        user.UserType = userType;

        // Assert
        Assert.Equal(userType, user.UserType);
    }

    [Theory]
    [InlineData(Gender.Male)]
    [InlineData(Gender.Female)]
    [InlineData(Gender.Other)]
    public void User_Gender_CanBeSetToAnyGender(Gender gender)
    {
        // Arrange
        var user = new User();

        // Act
        user.Gender = gender;

        // Assert
        Assert.Equal(gender, user.Gender);
    }

    [Fact]
    public void User_AppointmentsAsPatient_ShouldBeEmptyList()
    {
        // Arrange & Act
        var user = new User();

        // Assert
        Assert.Empty(user.AppointmentsAsPatient);
    }

    [Fact]
    public void User_AppointmentsAsDoctor_ShouldBeEmptyList()
    {
        // Arrange & Act
        var user = new User();

        // Assert
        Assert.Empty(user.AppointmentsAsDoctor);
    }

    [Fact]
    public void User_Bills_ShouldBeEmptyList()
    {
        // Arrange & Act
        var user = new User();

        // Assert
        Assert.Empty(user.Bills);
    }

    [Fact]
    public void User_Feedbacks_ShouldBeEmptyList()
    {
        // Arrange & Act
        var user = new User();

        // Assert
        Assert.Empty(user.Feedbacks);
    }

    [Fact]
    public void User_MedicalHistories_ShouldBeEmptyList()
    {
        // Arrange & Act
        var user = new User();

        // Assert
        Assert.Empty(user.MedicalHistories);
    }

    [Fact]
    public void User_CreatedBy_DefaultsToSystem()
    {
        // Arrange & Act
        var user = new User();

        // Assert
        Assert.Equal("System", user.CreatedBy);
    }
}
