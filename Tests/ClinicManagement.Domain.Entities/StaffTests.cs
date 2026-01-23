using Xunit;
using ClinicManagement.Domain.Entities;
using System;

namespace ClinicManagement.Domain.Entities.Tests;

/// <summary>
/// Unit tests for Staff entity
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
        Assert.Equal(string.Empty, staff.Role);
        Assert.Equal(string.Empty, staff.PhoneNo);
        Assert.Null(staff.Department);
        Assert.Equal("System", staff.CreatedBy);
        Assert.False(staff.IsActive);
    }

    [Fact]
    public void Staff_SetProperties_ShouldStoreCorrectValues()
    {
        // Arrange
        var staff = new Staff();
        var createdDate = DateTime.UtcNow;

        // Act
        staff.Id = 1;
        staff.Name = "Alice Johnson";
        staff.Email = "alice.johnson@hospital.com";
        staff.PasswordHash = "hashed_password";
        staff.Role = "Receptionist";
        staff.PhoneNo = "5551234567";
        staff.Department = "Administration";
        staff.CreatedDate = createdDate;
        staff.IsActive = true;
        staff.CreatedBy = "Admin";

        // Assert
        Assert.Equal(1, staff.Id);
        Assert.Equal("Alice Johnson", staff.Name);
        Assert.Equal("alice.johnson@hospital.com", staff.Email);
        Assert.Equal("hashed_password", staff.PasswordHash);
        Assert.Equal("Receptionist", staff.Role);
        Assert.Equal("5551234567", staff.PhoneNo);
        Assert.Equal("Administration", staff.Department);
        Assert.Equal(createdDate, staff.CreatedDate);
        Assert.True(staff.IsActive);
        Assert.Equal("Admin", staff.CreatedBy);
    }

    [Fact]
    public void Staff_Department_ShouldBeNullable()
    {
        // Arrange
        var staff = new Staff();

        // Act & Assert
        Assert.Null(staff.Department);

        // Act - Set value
        staff.Department = "IT";

        // Assert
        Assert.NotNull(staff.Department);
        Assert.Equal("IT", staff.Department);
    }

    [Fact]
    public void Staff_ModifiedDate_ShouldBeNullable()
    {
        // Arrange
        var staff = new Staff();

        // Act & Assert
        Assert.Null(staff.ModifiedDate);

        // Act - Set value
        var modifiedDate = DateTime.UtcNow;
        staff.ModifiedDate = modifiedDate;

        // Assert
        Assert.NotNull(staff.ModifiedDate);
        Assert.Equal(modifiedDate, staff.ModifiedDate);
    }

    [Fact]
    public void Staff_ModifiedBy_ShouldBeNullable()
    {
        // Arrange
        var staff = new Staff();

        // Act & Assert
        Assert.Null(staff.ModifiedBy);

        // Act - Set value
        staff.ModifiedBy = "UpdatedByManager";

        // Assert
        Assert.NotNull(staff.ModifiedBy);
        Assert.Equal("UpdatedByManager", staff.ModifiedBy);
    }

    [Theory]
    [InlineData("Receptionist")]
    [InlineData("Nurse")]
    [InlineData("Administrator")]
    [InlineData("Manager")]
    public void Staff_Role_ShouldAcceptVariousValues(string role)
    {
        // Arrange
        var staff = new Staff();

        // Act
        staff.Role = role;

        // Assert
        Assert.Equal(role, staff.Role);
    }

    [Theory]
    [InlineData("HR")]
    [InlineData("Finance")]
    [InlineData("Operations")]
    [InlineData("IT")]
    public void Staff_Department_ShouldAcceptVariousValues(string department)
    {
        // Arrange
        var staff = new Staff();

        // Act
        staff.Department = department;

        // Assert
        Assert.Equal(department, staff.Department);
    }

    [Fact]
    public void Staff_IsActive_DefaultValue_ShouldBeFalse()
    {
        // Arrange & Act
        var staff = new Staff();

        // Assert
        Assert.False(staff.IsActive);
    }

    [Fact]
    public void Staff_CreatedBy_DefaultValue_ShouldBeSystem()
    {
        // Arrange & Act
        var staff = new Staff();

        // Assert
        Assert.Equal("System", staff.CreatedBy);
    }

    [Fact]
    public void Staff_Email_ShouldStoreValidEmail()
    {
        // Arrange
        var staff = new Staff();

        // Act
        staff.Email = "staff@example.com";

        // Assert
        Assert.Equal("staff@example.com", staff.Email);
    }

    [Fact]
    public void Staff_PhoneNo_ShouldStorePhoneNumber()
    {
        // Arrange
        var staff = new Staff();

        // Act
        staff.PhoneNo = "1234567890";

        // Assert
        Assert.Equal("1234567890", staff.PhoneNo);
    }
}
