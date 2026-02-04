using Xunit;
using ClinicManagement.Domain.Entities;
using System;

namespace ClinicManagement.Domain.Entities.Tests;

/// <summary>
/// Test class for Staff entity
/// </summary>
public class StaffTests
{
    [Fact]
    public void Staff_Constructor_ShouldInitializeWithDefaultValues()
    {
        // Arrange & Act
        var staff = new Staff();

        // Assert
        Assert.Equal(0, staff.Id);
        Assert.Equal(string.Empty, staff.Name);
        Assert.Equal(string.Empty, staff.Email);
        Assert.Equal(string.Empty, staff.PasswordHash);
        Assert.Equal(string.Empty, staff.PhoneNumber);
        Assert.Equal("Staff", staff.Role);
        Assert.Equal("System", staff.CreatedBy);
        Assert.False(staff.IsActive);
    }

    [Fact]
    public void Staff_SetProperties_ShouldStoreValues()
    {
        // Arrange
        var staff = new Staff();
        var expectedId = 1;
        var expectedName = "Alice Johnson";
        var expectedEmail = "alice.johnson@clinic.com";
        var expectedPasswordHash = "hashed_password";
        var expectedPhone = "555-6789";
        var expectedRole = "Admin";
        var expectedCreatedDate = DateTime.UtcNow;
        var expectedModifiedDate = DateTime.UtcNow;
        var expectedIsActive = true;
        var expectedCreatedBy = "SuperAdmin";
        var expectedModifiedBy = "SuperAdmin";

        // Act
        staff.Id = expectedId;
        staff.Name = expectedName;
        staff.Email = expectedEmail;
        staff.PasswordHash = expectedPasswordHash;
        staff.PhoneNumber = expectedPhone;
        staff.Role = expectedRole;
        staff.CreatedDate = expectedCreatedDate;
        staff.ModifiedDate = expectedModifiedDate;
        staff.IsActive = expectedIsActive;
        staff.CreatedBy = expectedCreatedBy;
        staff.ModifiedBy = expectedModifiedBy;

        // Assert
        Assert.Equal(expectedId, staff.Id);
        Assert.Equal(expectedName, staff.Name);
        Assert.Equal(expectedEmail, staff.Email);
        Assert.Equal(expectedPasswordHash, staff.PasswordHash);
        Assert.Equal(expectedPhone, staff.PhoneNumber);
        Assert.Equal(expectedRole, staff.Role);
        Assert.Equal(expectedCreatedDate, staff.CreatedDate);
        Assert.Equal(expectedModifiedDate, staff.ModifiedDate);
        Assert.Equal(expectedIsActive, staff.IsActive);
        Assert.Equal(expectedCreatedBy, staff.CreatedBy);
        Assert.Equal(expectedModifiedBy, staff.ModifiedBy);
    }

    [Fact]
    public void Staff_ModifiedDate_ShouldAcceptNull()
    {
        // Arrange
        var staff = new Staff();

        // Act
        staff.ModifiedDate = null;

        // Assert
        Assert.Null(staff.ModifiedDate);
    }

    [Fact]
    public void Staff_ModifiedBy_ShouldAcceptNull()
    {
        // Arrange
        var staff = new Staff();

        // Act
        staff.ModifiedBy = null;

        // Assert
        Assert.Null(staff.ModifiedBy);
    }

    [Fact]
    public void Staff_IsActive_ShouldToggleBetweenTrueAndFalse()
    {
        // Arrange
        var staff = new Staff();

        // Act & Assert
        staff.IsActive = true;
        Assert.True(staff.IsActive);

        staff.IsActive = false;
        Assert.False(staff.IsActive);
    }

    [Fact]
    public void Staff_Name_ShouldAcceptEmptyString()
    {
        // Arrange
        var staff = new Staff();

        // Act
        staff.Name = string.Empty;

        // Assert
        Assert.Equal(string.Empty, staff.Name);
    }

    [Fact]
    public void Staff_Email_ShouldStoreValidEmail()
    {
        // Arrange
        var staff = new Staff();
        var email = "test@example.com";

        // Act
        staff.Email = email;

        // Assert
        Assert.Equal(email, staff.Email);
    }

    [Theory]
    [InlineData("Staff")]
    [InlineData("Admin")]
    [InlineData("SuperAdmin")]
    [InlineData("Manager")]
    public void Staff_Role_ShouldStoreRoleValue(string role)
    {
        // Arrange
        var staff = new Staff();

        // Act
        staff.Role = role;

        // Assert
        Assert.Equal(role, staff.Role);
    }

    [Fact]
    public void Staff_PasswordHash_ShouldStoreHashedValue()
    {
        // Arrange
        var staff = new Staff();
        var hashedPassword = "hashed_value_456";

        // Act
        staff.PasswordHash = hashedPassword;

        // Assert
        Assert.Equal(hashedPassword, staff.PasswordHash);
    }

    [Fact]
    public void Staff_PhoneNumber_ShouldStoreValue()
    {
        // Arrange
        var staff = new Staff();
        var phoneNumber = "123-456-7890";

        // Act
        staff.PhoneNumber = phoneNumber;

        // Assert
        Assert.Equal(phoneNumber, staff.PhoneNumber);
    }
}
