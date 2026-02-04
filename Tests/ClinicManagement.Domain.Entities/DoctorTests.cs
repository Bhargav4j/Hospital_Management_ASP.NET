using Xunit;
using ClinicManagement.Domain.Entities;
using ClinicManagement.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ClinicManagement.Domain.Entities.Tests;

/// <summary>
/// Test class for Doctor entity
/// </summary>
public class DoctorTests
{
    [Fact]
    public void Doctor_Constructor_ShouldInitializeWithDefaultValues()
    {
        // Arrange & Act
        var doctor = new Doctor();

        // Assert
        Assert.Equal(0, doctor.Id);
        Assert.Equal(string.Empty, doctor.Name);
        Assert.Equal(string.Empty, doctor.Email);
        Assert.Equal(string.Empty, doctor.PasswordHash);
        Assert.Equal(string.Empty, doctor.PhoneNumber);
        Assert.Equal(string.Empty, doctor.Specialization);
        Assert.Equal(string.Empty, doctor.Qualification);
        Assert.Equal("System", doctor.CreatedBy);
        Assert.False(doctor.IsActive);
        Assert.NotNull(doctor.Appointments);
        Assert.Empty(doctor.Appointments);
    }

    [Fact]
    public void Doctor_SetProperties_ShouldStoreValues()
    {
        // Arrange
        var doctor = new Doctor();
        var expectedId = 1;
        var expectedName = "Dr. John Smith";
        var expectedEmail = "john.smith@clinic.com";
        var expectedPasswordHash = "hashed_password";
        var expectedPhone = "1234567890";
        var expectedSpecialization = "Cardiology";
        var expectedQualification = "MD";
        var expectedGender = Gender.Male;
        var expectedClinicId = 10;
        var expectedCreatedDate = DateTime.UtcNow;
        var expectedModifiedDate = DateTime.UtcNow;
        var expectedIsActive = true;
        var expectedCreatedBy = "Admin";
        var expectedModifiedBy = "Admin";

        // Act
        doctor.Id = expectedId;
        doctor.Name = expectedName;
        doctor.Email = expectedEmail;
        doctor.PasswordHash = expectedPasswordHash;
        doctor.PhoneNumber = expectedPhone;
        doctor.Specialization = expectedSpecialization;
        doctor.Qualification = expectedQualification;
        doctor.Gender = expectedGender;
        doctor.ClinicId = expectedClinicId;
        doctor.CreatedDate = expectedCreatedDate;
        doctor.ModifiedDate = expectedModifiedDate;
        doctor.IsActive = expectedIsActive;
        doctor.CreatedBy = expectedCreatedBy;
        doctor.ModifiedBy = expectedModifiedBy;

        // Assert
        Assert.Equal(expectedId, doctor.Id);
        Assert.Equal(expectedName, doctor.Name);
        Assert.Equal(expectedEmail, doctor.Email);
        Assert.Equal(expectedPasswordHash, doctor.PasswordHash);
        Assert.Equal(expectedPhone, doctor.PhoneNumber);
        Assert.Equal(expectedSpecialization, doctor.Specialization);
        Assert.Equal(expectedQualification, doctor.Qualification);
        Assert.Equal(expectedGender, doctor.Gender);
        Assert.Equal(expectedClinicId, doctor.ClinicId);
        Assert.Equal(expectedCreatedDate, doctor.CreatedDate);
        Assert.Equal(expectedModifiedDate, doctor.ModifiedDate);
        Assert.Equal(expectedIsActive, doctor.IsActive);
        Assert.Equal(expectedCreatedBy, doctor.CreatedBy);
        Assert.Equal(expectedModifiedBy, doctor.ModifiedBy);
    }

    [Fact]
    public void Doctor_ClinicId_ShouldAcceptNull()
    {
        // Arrange
        var doctor = new Doctor();

        // Act
        doctor.ClinicId = null;

        // Assert
        Assert.Null(doctor.ClinicId);
    }

    [Fact]
    public void Doctor_ModifiedDate_ShouldAcceptNull()
    {
        // Arrange
        var doctor = new Doctor();

        // Act
        doctor.ModifiedDate = null;

        // Assert
        Assert.Null(doctor.ModifiedDate);
    }

    [Fact]
    public void Doctor_ModifiedBy_ShouldAcceptNull()
    {
        // Arrange
        var doctor = new Doctor();

        // Act
        doctor.ModifiedBy = null;

        // Assert
        Assert.Null(doctor.ModifiedBy);
    }

    [Fact]
    public void Doctor_Clinic_ShouldAcceptNull()
    {
        // Arrange
        var doctor = new Doctor();

        // Act
        doctor.Clinic = null;

        // Assert
        Assert.Null(doctor.Clinic);
    }

    [Fact]
    public void Doctor_Clinic_ShouldStoreClinicReference()
    {
        // Arrange
        var doctor = new Doctor();
        var clinic = new Clinic { Id = 1, Name = "Test Clinic" };

        // Act
        doctor.Clinic = clinic;

        // Assert
        Assert.NotNull(doctor.Clinic);
        Assert.Equal(clinic.Id, doctor.Clinic.Id);
        Assert.Equal(clinic.Name, doctor.Clinic.Name);
    }

    [Fact]
    public void Doctor_Appointments_ShouldInitializeAsEmptyList()
    {
        // Arrange & Act
        var doctor = new Doctor();

        // Assert
        Assert.NotNull(doctor.Appointments);
        Assert.IsAssignableFrom<ICollection<Appointment>>(doctor.Appointments);
        Assert.Empty(doctor.Appointments);
    }

    [Fact]
    public void Doctor_Appointments_ShouldStoreMultipleAppointments()
    {
        // Arrange
        var doctor = new Doctor { Id = 1 };
        var appointment1 = new Appointment { Id = 1, DoctorId = 1 };
        var appointment2 = new Appointment { Id = 2, DoctorId = 1 };

        // Act
        doctor.Appointments.Add(appointment1);
        doctor.Appointments.Add(appointment2);

        // Assert
        Assert.Equal(2, doctor.Appointments.Count);
        Assert.Contains(appointment1, doctor.Appointments);
        Assert.Contains(appointment2, doctor.Appointments);
    }

    [Theory]
    [InlineData(Gender.Male)]
    [InlineData(Gender.Female)]
    [InlineData(Gender.Other)]
    public void Doctor_Gender_ShouldAcceptAllGenderValues(Gender gender)
    {
        // Arrange
        var doctor = new Doctor();

        // Act
        doctor.Gender = gender;

        // Assert
        Assert.Equal(gender, doctor.Gender);
    }

    [Fact]
    public void Doctor_IsActive_ShouldToggleBetweenTrueAndFalse()
    {
        // Arrange
        var doctor = new Doctor();

        // Act & Assert
        doctor.IsActive = true;
        Assert.True(doctor.IsActive);

        doctor.IsActive = false;
        Assert.False(doctor.IsActive);
    }

    [Fact]
    public void Doctor_Email_ShouldAcceptEmptyString()
    {
        // Arrange
        var doctor = new Doctor();

        // Act
        doctor.Email = string.Empty;

        // Assert
        Assert.Equal(string.Empty, doctor.Email);
    }

    [Fact]
    public void Doctor_PasswordHash_ShouldStoreHashedValue()
    {
        // Arrange
        var doctor = new Doctor();
        var hashedPassword = "hashed_value_123";

        // Act
        doctor.PasswordHash = hashedPassword;

        // Assert
        Assert.Equal(hashedPassword, doctor.PasswordHash);
    }
}
