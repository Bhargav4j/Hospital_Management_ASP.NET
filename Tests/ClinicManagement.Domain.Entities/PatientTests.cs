using Xunit;
using ClinicManagement.Domain.Entities;
using System;
using System.Collections.Generic;

namespace ClinicManagement.Domain.Entities.Tests;

/// <summary>
/// Unit tests for Patient entity
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
        Assert.Equal(string.Empty, patient.PhoneNo);
        Assert.Equal(string.Empty, patient.Gender);
        Assert.Equal(string.Empty, patient.Address);
        Assert.Equal("System", patient.CreatedBy);
        Assert.False(patient.IsActive);
        Assert.NotNull(patient.Appointments);
        Assert.NotNull(patient.Bills);
        Assert.NotNull(patient.Feedbacks);
        Assert.NotNull(patient.Notifications);
    }

    [Fact]
    public void Patient_SetProperties_ShouldStoreCorrectValues()
    {
        // Arrange
        var patient = new Patient();
        var birthDate = new DateTime(1990, 5, 15);
        var createdDate = DateTime.UtcNow;

        // Act
        patient.Id = 1;
        patient.Name = "John Doe";
        patient.BirthDate = birthDate;
        patient.Email = "john.doe@example.com";
        patient.PasswordHash = "hashed_password";
        patient.PhoneNo = "1234567890";
        patient.Gender = "Male";
        patient.Address = "123 Main St";
        patient.CreatedDate = createdDate;
        patient.IsActive = true;
        patient.CreatedBy = "Admin";

        // Assert
        Assert.Equal(1, patient.Id);
        Assert.Equal("John Doe", patient.Name);
        Assert.Equal(birthDate, patient.BirthDate);
        Assert.Equal("john.doe@example.com", patient.Email);
        Assert.Equal("hashed_password", patient.PasswordHash);
        Assert.Equal("1234567890", patient.PhoneNo);
        Assert.Equal("Male", patient.Gender);
        Assert.Equal("123 Main St", patient.Address);
        Assert.Equal(createdDate, patient.CreatedDate);
        Assert.True(patient.IsActive);
        Assert.Equal("Admin", patient.CreatedBy);
    }

    [Fact]
    public void Patient_ModifiedDate_ShouldBeNullable()
    {
        // Arrange
        var patient = new Patient();

        // Act & Assert
        Assert.Null(patient.ModifiedDate);

        // Act - Set value
        var modifiedDate = DateTime.UtcNow;
        patient.ModifiedDate = modifiedDate;

        // Assert
        Assert.NotNull(patient.ModifiedDate);
        Assert.Equal(modifiedDate, patient.ModifiedDate);
    }

    [Fact]
    public void Patient_ModifiedBy_ShouldBeNullable()
    {
        // Arrange
        var patient = new Patient();

        // Act & Assert
        Assert.Null(patient.ModifiedBy);

        // Act - Set value
        patient.ModifiedBy = "UpdatedByAdmin";

        // Assert
        Assert.NotNull(patient.ModifiedBy);
        Assert.Equal("UpdatedByAdmin", patient.ModifiedBy);
    }

    [Fact]
    public void Patient_Appointments_ShouldBeEmptyCollection()
    {
        // Arrange & Act
        var patient = new Patient();

        // Assert
        Assert.NotNull(patient.Appointments);
        Assert.Empty(patient.Appointments);
        Assert.IsAssignableFrom<ICollection<Appointment>>(patient.Appointments);
    }

    [Fact]
    public void Patient_Bills_ShouldBeEmptyCollection()
    {
        // Arrange & Act
        var patient = new Patient();

        // Assert
        Assert.NotNull(patient.Bills);
        Assert.Empty(patient.Bills);
        Assert.IsAssignableFrom<ICollection<Bill>>(patient.Bills);
    }

    [Fact]
    public void Patient_Feedbacks_ShouldBeEmptyCollection()
    {
        // Arrange & Act
        var patient = new Patient();

        // Assert
        Assert.NotNull(patient.Feedbacks);
        Assert.Empty(patient.Feedbacks);
        Assert.IsAssignableFrom<ICollection<Feedback>>(patient.Feedbacks);
    }

    [Fact]
    public void Patient_Notifications_ShouldBeEmptyCollection()
    {
        // Arrange & Act
        var patient = new Patient();

        // Assert
        Assert.NotNull(patient.Notifications);
        Assert.Empty(patient.Notifications);
        Assert.IsAssignableFrom<ICollection<Notification>>(patient.Notifications);
    }

    [Fact]
    public void Patient_AddAppointment_ShouldAddToCollection()
    {
        // Arrange
        var patient = new Patient();
        var appointment = new Appointment { Id = 1, PatientId = 1 };

        // Act
        patient.Appointments.Add(appointment);

        // Assert
        Assert.Single(patient.Appointments);
        Assert.Contains(appointment, patient.Appointments);
    }

    [Fact]
    public void Patient_AddBill_ShouldAddToCollection()
    {
        // Arrange
        var patient = new Patient();
        var bill = new Bill { Id = 1, PatientId = 1 };

        // Act
        patient.Bills.Add(bill);

        // Assert
        Assert.Single(patient.Bills);
        Assert.Contains(bill, patient.Bills);
    }

    [Fact]
    public void Patient_IsActive_DefaultValue_ShouldBeFalse()
    {
        // Arrange & Act
        var patient = new Patient();

        // Assert
        Assert.False(patient.IsActive);
    }

    [Fact]
    public void Patient_CreatedBy_DefaultValue_ShouldBeSystem()
    {
        // Arrange & Act
        var patient = new Patient();

        // Assert
        Assert.Equal("System", patient.CreatedBy);
    }

    [Theory]
    [InlineData("")]
    [InlineData("test@example.com")]
    [InlineData("another.email@test.com")]
    public void Patient_Email_ShouldAcceptVariousFormats(string email)
    {
        // Arrange
        var patient = new Patient();

        // Act
        patient.Email = email;

        // Assert
        Assert.Equal(email, patient.Email);
    }

    [Theory]
    [InlineData("Male")]
    [InlineData("Female")]
    [InlineData("Other")]
    public void Patient_Gender_ShouldAcceptVariousValues(string gender)
    {
        // Arrange
        var patient = new Patient();

        // Act
        patient.Gender = gender;

        // Assert
        Assert.Equal(gender, patient.Gender);
    }
}
