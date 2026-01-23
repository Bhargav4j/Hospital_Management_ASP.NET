using Xunit;
using ClinicManagement.Domain.Entities;
using System;
using System.Collections.Generic;

namespace ClinicManagement.Domain.Entities.Tests;

/// <summary>
/// Unit tests for Doctor entity
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
        Assert.Equal(string.Empty, doctor.Specialization);
        Assert.Equal(string.Empty, doctor.PhoneNo);
        Assert.Null(doctor.LicenseNumber);
        Assert.Equal(0m, doctor.ConsultationFee);
        Assert.Null(doctor.Experience);
        Assert.Null(doctor.Qualification);
        Assert.Equal("System", doctor.CreatedBy);
        Assert.False(doctor.IsActive);
        Assert.NotNull(doctor.Appointments);
        Assert.NotNull(doctor.TreatmentHistories);
    }

    [Fact]
    public void Doctor_SetProperties_ShouldStoreCorrectValues()
    {
        // Arrange
        var doctor = new Doctor();
        var createdDate = DateTime.UtcNow;

        // Act
        doctor.Id = 1;
        doctor.Name = "Dr. Jane Smith";
        doctor.Email = "jane.smith@hospital.com";
        doctor.PasswordHash = "hashed_password";
        doctor.Specialization = "Cardiology";
        doctor.PhoneNo = "9876543210";
        doctor.LicenseNumber = "LIC123456";
        doctor.ConsultationFee = 150.50m;
        doctor.Experience = "10 years";
        doctor.Qualification = "MBBS, MD";
        doctor.CreatedDate = createdDate;
        doctor.IsActive = true;
        doctor.CreatedBy = "Admin";

        // Assert
        Assert.Equal(1, doctor.Id);
        Assert.Equal("Dr. Jane Smith", doctor.Name);
        Assert.Equal("jane.smith@hospital.com", doctor.Email);
        Assert.Equal("hashed_password", doctor.PasswordHash);
        Assert.Equal("Cardiology", doctor.Specialization);
        Assert.Equal("9876543210", doctor.PhoneNo);
        Assert.Equal("LIC123456", doctor.LicenseNumber);
        Assert.Equal(150.50m, doctor.ConsultationFee);
        Assert.Equal("10 years", doctor.Experience);
        Assert.Equal("MBBS, MD", doctor.Qualification);
        Assert.Equal(createdDate, doctor.CreatedDate);
        Assert.True(doctor.IsActive);
        Assert.Equal("Admin", doctor.CreatedBy);
    }

    [Fact]
    public void Doctor_ConsultationFee_ShouldAcceptDecimalValues()
    {
        // Arrange
        var doctor = new Doctor();

        // Act
        doctor.ConsultationFee = 99.99m;

        // Assert
        Assert.Equal(99.99m, doctor.ConsultationFee);
    }

    [Fact]
    public void Doctor_ConsultationFee_ShouldAcceptZero()
    {
        // Arrange
        var doctor = new Doctor();

        // Act
        doctor.ConsultationFee = 0m;

        // Assert
        Assert.Equal(0m, doctor.ConsultationFee);
    }

    [Fact]
    public void Doctor_LicenseNumber_ShouldBeNullable()
    {
        // Arrange
        var doctor = new Doctor();

        // Act & Assert
        Assert.Null(doctor.LicenseNumber);

        // Act - Set value
        doctor.LicenseNumber = "LIC999";

        // Assert
        Assert.NotNull(doctor.LicenseNumber);
        Assert.Equal("LIC999", doctor.LicenseNumber);
    }

    [Fact]
    public void Doctor_Experience_ShouldBeNullable()
    {
        // Arrange
        var doctor = new Doctor();

        // Act & Assert
        Assert.Null(doctor.Experience);

        // Act - Set value
        doctor.Experience = "5 years";

        // Assert
        Assert.NotNull(doctor.Experience);
        Assert.Equal("5 years", doctor.Experience);
    }

    [Fact]
    public void Doctor_Qualification_ShouldBeNullable()
    {
        // Arrange
        var doctor = new Doctor();

        // Act & Assert
        Assert.Null(doctor.Qualification);

        // Act - Set value
        doctor.Qualification = "MD";

        // Assert
        Assert.NotNull(doctor.Qualification);
        Assert.Equal("MD", doctor.Qualification);
    }

    [Fact]
    public void Doctor_Appointments_ShouldBeEmptyCollection()
    {
        // Arrange & Act
        var doctor = new Doctor();

        // Assert
        Assert.NotNull(doctor.Appointments);
        Assert.Empty(doctor.Appointments);
        Assert.IsAssignableFrom<ICollection<Appointment>>(doctor.Appointments);
    }

    [Fact]
    public void Doctor_TreatmentHistories_ShouldBeEmptyCollection()
    {
        // Arrange & Act
        var doctor = new Doctor();

        // Assert
        Assert.NotNull(doctor.TreatmentHistories);
        Assert.Empty(doctor.TreatmentHistories);
        Assert.IsAssignableFrom<ICollection<TreatmentHistory>>(doctor.TreatmentHistories);
    }

    [Fact]
    public void Doctor_ModifiedDate_ShouldBeNullable()
    {
        // Arrange
        var doctor = new Doctor();

        // Act & Assert
        Assert.Null(doctor.ModifiedDate);

        // Act - Set value
        var modifiedDate = DateTime.UtcNow;
        doctor.ModifiedDate = modifiedDate;

        // Assert
        Assert.NotNull(doctor.ModifiedDate);
        Assert.Equal(modifiedDate, doctor.ModifiedDate);
    }

    [Fact]
    public void Doctor_ModifiedBy_ShouldBeNullable()
    {
        // Arrange
        var doctor = new Doctor();

        // Act & Assert
        Assert.Null(doctor.ModifiedBy);

        // Act - Set value
        doctor.ModifiedBy = "UpdatedByAdmin";

        // Assert
        Assert.NotNull(doctor.ModifiedBy);
        Assert.Equal("UpdatedByAdmin", doctor.ModifiedBy);
    }

    [Theory]
    [InlineData("Cardiology")]
    [InlineData("Dermatology")]
    [InlineData("Neurology")]
    [InlineData("Orthopedics")]
    public void Doctor_Specialization_ShouldAcceptVariousValues(string specialization)
    {
        // Arrange
        var doctor = new Doctor();

        // Act
        doctor.Specialization = specialization;

        // Assert
        Assert.Equal(specialization, doctor.Specialization);
    }

    [Fact]
    public void Doctor_AddAppointment_ShouldAddToCollection()
    {
        // Arrange
        var doctor = new Doctor();
        var appointment = new Appointment { Id = 1, DoctorId = 1 };

        // Act
        doctor.Appointments.Add(appointment);

        // Assert
        Assert.Single(doctor.Appointments);
        Assert.Contains(appointment, doctor.Appointments);
    }

    [Fact]
    public void Doctor_AddTreatmentHistory_ShouldAddToCollection()
    {
        // Arrange
        var doctor = new Doctor();
        var treatmentHistory = new TreatmentHistory { Id = 1, DoctorId = 1 };

        // Act
        doctor.TreatmentHistories.Add(treatmentHistory);

        // Assert
        Assert.Single(doctor.TreatmentHistories);
        Assert.Contains(treatmentHistory, doctor.TreatmentHistories);
    }
}
