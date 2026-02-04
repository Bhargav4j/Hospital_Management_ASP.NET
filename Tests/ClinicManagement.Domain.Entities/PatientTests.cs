using Xunit;
using ClinicManagement.Domain.Entities;
using ClinicManagement.Domain.Enums;
using System;
using System.Collections.Generic;

namespace ClinicManagement.Domain.Entities.Tests;

/// <summary>
/// Test class for Patient entity
/// </summary>
public class PatientTests
{
    [Fact]
    public void Patient_Constructor_ShouldInitializeWithDefaultValues()
    {
        // Arrange & Act
        var patient = new Patient();

        // Assert
        Assert.Equal(0, patient.Id);
        Assert.Equal(string.Empty, patient.Name);
        Assert.Equal(string.Empty, patient.Email);
        Assert.Equal(string.Empty, patient.PasswordHash);
        Assert.Equal(string.Empty, patient.PhoneNumber);
        Assert.Equal(string.Empty, patient.Address);
        Assert.Equal("System", patient.CreatedBy);
        Assert.False(patient.IsActive);
        Assert.NotNull(patient.Appointments);
        Assert.NotNull(patient.Bills);
        Assert.NotNull(patient.Feedbacks);
        Assert.Empty(patient.Appointments);
        Assert.Empty(patient.Bills);
        Assert.Empty(patient.Feedbacks);
    }

    [Fact]
    public void Patient_SetProperties_ShouldStoreValues()
    {
        // Arrange
        var patient = new Patient();
        var expectedId = 1;
        var expectedName = "Jane Doe";
        var expectedEmail = "jane.doe@example.com";
        var expectedPasswordHash = "hashed_password";
        var expectedPhone = "9876543210";
        var expectedBirthDate = new DateTime(1990, 5, 15);
        var expectedGender = Gender.Female;
        var expectedAddress = "123 Main St";
        var expectedCreatedDate = DateTime.UtcNow;
        var expectedModifiedDate = DateTime.UtcNow;
        var expectedIsActive = true;
        var expectedCreatedBy = "Admin";
        var expectedModifiedBy = "Admin";

        // Act
        patient.Id = expectedId;
        patient.Name = expectedName;
        patient.Email = expectedEmail;
        patient.PasswordHash = expectedPasswordHash;
        patient.PhoneNumber = expectedPhone;
        patient.BirthDate = expectedBirthDate;
        patient.Gender = expectedGender;
        patient.Address = expectedAddress;
        patient.CreatedDate = expectedCreatedDate;
        patient.ModifiedDate = expectedModifiedDate;
        patient.IsActive = expectedIsActive;
        patient.CreatedBy = expectedCreatedBy;
        patient.ModifiedBy = expectedModifiedBy;

        // Assert
        Assert.Equal(expectedId, patient.Id);
        Assert.Equal(expectedName, patient.Name);
        Assert.Equal(expectedEmail, patient.Email);
        Assert.Equal(expectedPasswordHash, patient.PasswordHash);
        Assert.Equal(expectedPhone, patient.PhoneNumber);
        Assert.Equal(expectedBirthDate, patient.BirthDate);
        Assert.Equal(expectedGender, patient.Gender);
        Assert.Equal(expectedAddress, patient.Address);
        Assert.Equal(expectedCreatedDate, patient.CreatedDate);
        Assert.Equal(expectedModifiedDate, patient.ModifiedDate);
        Assert.Equal(expectedIsActive, patient.IsActive);
        Assert.Equal(expectedCreatedBy, patient.CreatedBy);
        Assert.Equal(expectedModifiedBy, patient.ModifiedBy);
    }

    [Fact]
    public void Patient_ModifiedDate_ShouldAcceptNull()
    {
        // Arrange
        var patient = new Patient();

        // Act
        patient.ModifiedDate = null;

        // Assert
        Assert.Null(patient.ModifiedDate);
    }

    [Fact]
    public void Patient_ModifiedBy_ShouldAcceptNull()
    {
        // Arrange
        var patient = new Patient();

        // Act
        patient.ModifiedBy = null;

        // Assert
        Assert.Null(patient.ModifiedBy);
    }

    [Fact]
    public void Patient_Appointments_ShouldInitializeAsEmptyList()
    {
        // Arrange & Act
        var patient = new Patient();

        // Assert
        Assert.NotNull(patient.Appointments);
        Assert.IsAssignableFrom<ICollection<Appointment>>(patient.Appointments);
        Assert.Empty(patient.Appointments);
    }

    [Fact]
    public void Patient_Appointments_ShouldStoreMultipleAppointments()
    {
        // Arrange
        var patient = new Patient { Id = 1 };
        var appointment1 = new Appointment { Id = 1, PatientId = 1 };
        var appointment2 = new Appointment { Id = 2, PatientId = 1 };

        // Act
        patient.Appointments.Add(appointment1);
        patient.Appointments.Add(appointment2);

        // Assert
        Assert.Equal(2, patient.Appointments.Count);
        Assert.Contains(appointment1, patient.Appointments);
        Assert.Contains(appointment2, patient.Appointments);
    }

    [Fact]
    public void Patient_Bills_ShouldInitializeAsEmptyList()
    {
        // Arrange & Act
        var patient = new Patient();

        // Assert
        Assert.NotNull(patient.Bills);
        Assert.IsAssignableFrom<ICollection<Bill>>(patient.Bills);
        Assert.Empty(patient.Bills);
    }

    [Fact]
    public void Patient_Bills_ShouldStoreMultipleBills()
    {
        // Arrange
        var patient = new Patient { Id = 1 };
        var bill1 = new Bill { Id = 1, PatientId = 1 };
        var bill2 = new Bill { Id = 2, PatientId = 1 };

        // Act
        patient.Bills.Add(bill1);
        patient.Bills.Add(bill2);

        // Assert
        Assert.Equal(2, patient.Bills.Count);
        Assert.Contains(bill1, patient.Bills);
        Assert.Contains(bill2, patient.Bills);
    }

    [Fact]
    public void Patient_Feedbacks_ShouldInitializeAsEmptyList()
    {
        // Arrange & Act
        var patient = new Patient();

        // Assert
        Assert.NotNull(patient.Feedbacks);
        Assert.IsAssignableFrom<ICollection<Feedback>>(patient.Feedbacks);
        Assert.Empty(patient.Feedbacks);
    }

    [Fact]
    public void Patient_Feedbacks_ShouldStoreMultipleFeedbacks()
    {
        // Arrange
        var patient = new Patient { Id = 1 };
        var feedback1 = new Feedback { Id = 1, PatientId = 1 };
        var feedback2 = new Feedback { Id = 2, PatientId = 1 };

        // Act
        patient.Feedbacks.Add(feedback1);
        patient.Feedbacks.Add(feedback2);

        // Assert
        Assert.Equal(2, patient.Feedbacks.Count);
        Assert.Contains(feedback1, patient.Feedbacks);
        Assert.Contains(feedback2, patient.Feedbacks);
    }

    [Theory]
    [InlineData(Gender.Male)]
    [InlineData(Gender.Female)]
    [InlineData(Gender.Other)]
    public void Patient_Gender_ShouldAcceptAllGenderValues(Gender gender)
    {
        // Arrange
        var patient = new Patient();

        // Act
        patient.Gender = gender;

        // Assert
        Assert.Equal(gender, patient.Gender);
    }

    [Fact]
    public void Patient_IsActive_ShouldToggleBetweenTrueAndFalse()
    {
        // Arrange
        var patient = new Patient();

        // Act & Assert
        patient.IsActive = true;
        Assert.True(patient.IsActive);

        patient.IsActive = false;
        Assert.False(patient.IsActive);
    }

    [Fact]
    public void Patient_BirthDate_ShouldStoreDateTimeValue()
    {
        // Arrange
        var patient = new Patient();
        var birthDate = new DateTime(1985, 3, 20);

        // Act
        patient.BirthDate = birthDate;

        // Assert
        Assert.Equal(birthDate, patient.BirthDate);
    }

    [Fact]
    public void Patient_Address_ShouldAcceptEmptyString()
    {
        // Arrange
        var patient = new Patient();

        // Act
        patient.Address = string.Empty;

        // Assert
        Assert.Equal(string.Empty, patient.Address);
    }
}
