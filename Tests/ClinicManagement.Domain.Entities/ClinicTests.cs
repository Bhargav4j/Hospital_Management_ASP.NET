using Xunit;
using ClinicManagement.Domain.Entities;
using System;
using System.Collections.Generic;

namespace ClinicManagement.Domain.Entities.Tests;

/// <summary>
/// Test class for Clinic entity
/// </summary>
public class ClinicTests
{
    [Fact]
    public void Clinic_Constructor_ShouldInitializeWithDefaultValues()
    {
        // Arrange & Act
        var clinic = new Clinic();

        // Assert
        Assert.Equal(0, clinic.Id);
        Assert.Equal(string.Empty, clinic.Name);
        Assert.Equal(string.Empty, clinic.Address);
        Assert.Equal(string.Empty, clinic.PhoneNumber);
        Assert.Equal("System", clinic.CreatedBy);
        Assert.False(clinic.IsActive);
        Assert.NotNull(clinic.Doctors);
        Assert.Empty(clinic.Doctors);
    }

    [Fact]
    public void Clinic_SetProperties_ShouldStoreValues()
    {
        // Arrange
        var clinic = new Clinic();
        var expectedId = 1;
        var expectedName = "City Health Clinic";
        var expectedAddress = "456 Main Ave";
        var expectedPhoneNumber = "555-1234";
        var expectedEmail = "info@cityclinic.com";
        var expectedCreatedDate = DateTime.UtcNow;
        var expectedModifiedDate = DateTime.UtcNow;
        var expectedIsActive = true;
        var expectedCreatedBy = "Admin";
        var expectedModifiedBy = "Admin";

        // Act
        clinic.Id = expectedId;
        clinic.Name = expectedName;
        clinic.Address = expectedAddress;
        clinic.PhoneNumber = expectedPhoneNumber;
        clinic.Email = expectedEmail;
        clinic.CreatedDate = expectedCreatedDate;
        clinic.ModifiedDate = expectedModifiedDate;
        clinic.IsActive = expectedIsActive;
        clinic.CreatedBy = expectedCreatedBy;
        clinic.ModifiedBy = expectedModifiedBy;

        // Assert
        Assert.Equal(expectedId, clinic.Id);
        Assert.Equal(expectedName, clinic.Name);
        Assert.Equal(expectedAddress, clinic.Address);
        Assert.Equal(expectedPhoneNumber, clinic.PhoneNumber);
        Assert.Equal(expectedEmail, clinic.Email);
        Assert.Equal(expectedCreatedDate, clinic.CreatedDate);
        Assert.Equal(expectedModifiedDate, clinic.ModifiedDate);
        Assert.Equal(expectedIsActive, clinic.IsActive);
        Assert.Equal(expectedCreatedBy, clinic.CreatedBy);
        Assert.Equal(expectedModifiedBy, clinic.ModifiedBy);
    }

    [Fact]
    public void Clinic_Email_ShouldAcceptNull()
    {
        // Arrange
        var clinic = new Clinic();

        // Act
        clinic.Email = null;

        // Assert
        Assert.Null(clinic.Email);
    }

    [Fact]
    public void Clinic_ModifiedDate_ShouldAcceptNull()
    {
        // Arrange
        var clinic = new Clinic();

        // Act
        clinic.ModifiedDate = null;

        // Assert
        Assert.Null(clinic.ModifiedDate);
    }

    [Fact]
    public void Clinic_ModifiedBy_ShouldAcceptNull()
    {
        // Arrange
        var clinic = new Clinic();

        // Act
        clinic.ModifiedBy = null;

        // Assert
        Assert.Null(clinic.ModifiedBy);
    }

    [Fact]
    public void Clinic_Doctors_ShouldInitializeAsEmptyList()
    {
        // Arrange & Act
        var clinic = new Clinic();

        // Assert
        Assert.NotNull(clinic.Doctors);
        Assert.IsAssignableFrom<ICollection<Doctor>>(clinic.Doctors);
        Assert.Empty(clinic.Doctors);
    }

    [Fact]
    public void Clinic_Doctors_ShouldStoreMultipleDoctors()
    {
        // Arrange
        var clinic = new Clinic { Id = 1 };
        var doctor1 = new Doctor { Id = 1, ClinicId = 1, Name = "Dr. Smith" };
        var doctor2 = new Doctor { Id = 2, ClinicId = 1, Name = "Dr. Jones" };

        // Act
        clinic.Doctors.Add(doctor1);
        clinic.Doctors.Add(doctor2);

        // Assert
        Assert.Equal(2, clinic.Doctors.Count);
        Assert.Contains(doctor1, clinic.Doctors);
        Assert.Contains(doctor2, clinic.Doctors);
    }

    [Fact]
    public void Clinic_IsActive_ShouldToggleBetweenTrueAndFalse()
    {
        // Arrange
        var clinic = new Clinic();

        // Act & Assert
        clinic.IsActive = true;
        Assert.True(clinic.IsActive);

        clinic.IsActive = false;
        Assert.False(clinic.IsActive);
    }

    [Fact]
    public void Clinic_Name_ShouldAcceptEmptyString()
    {
        // Arrange
        var clinic = new Clinic();

        // Act
        clinic.Name = string.Empty;

        // Assert
        Assert.Equal(string.Empty, clinic.Name);
    }

    [Fact]
    public void Clinic_Address_ShouldAcceptEmptyString()
    {
        // Arrange
        var clinic = new Clinic();

        // Act
        clinic.Address = string.Empty;

        // Assert
        Assert.Equal(string.Empty, clinic.Address);
    }

    [Fact]
    public void Clinic_PhoneNumber_ShouldStoreValue()
    {
        // Arrange
        var clinic = new Clinic();
        var phoneNumber = "555-9876";

        // Act
        clinic.PhoneNumber = phoneNumber;

        // Assert
        Assert.Equal(phoneNumber, clinic.PhoneNumber);
    }
}
