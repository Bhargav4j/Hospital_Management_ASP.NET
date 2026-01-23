using Xunit;
using ClinicManagement.Domain.Entities;
using System;

namespace ClinicManagement.Domain.Entities.Tests;

/// <summary>
/// Unit tests for Appointment entity
/// </summary>
public class AppointmentTests
{
    [Fact]
    public void Appointment_Constructor_ShouldInitializeWithDefaultValues()
    {
        // Arrange & Act
        var appointment = new Appointment();

        // Assert
        Assert.Equal(0, appointment.Id);
        Assert.Equal(0, appointment.PatientId);
        Assert.Equal(0, appointment.DoctorId);
        Assert.Equal(string.Empty, appointment.TimeSlot);
        Assert.Equal("Pending", appointment.Status);
        Assert.Null(appointment.Symptoms);
        Assert.Null(appointment.Notes);
        Assert.Equal("System", appointment.CreatedBy);
        Assert.False(appointment.IsActive);
    }

    [Fact]
    public void Appointment_SetProperties_ShouldStoreCorrectValues()
    {
        // Arrange
        var appointment = new Appointment();
        var appointmentDate = new DateTime(2023, 12, 25, 10, 30, 0);
        var createdDate = DateTime.UtcNow;

        // Act
        appointment.Id = 1;
        appointment.PatientId = 5;
        appointment.DoctorId = 3;
        appointment.AppointmentDate = appointmentDate;
        appointment.TimeSlot = "10:30 AM - 11:00 AM";
        appointment.Status = "Confirmed";
        appointment.Symptoms = "Fever, Headache";
        appointment.Notes = "Patient needs urgent care";
        appointment.CreatedDate = createdDate;
        appointment.IsActive = true;
        appointment.CreatedBy = "Receptionist";

        // Assert
        Assert.Equal(1, appointment.Id);
        Assert.Equal(5, appointment.PatientId);
        Assert.Equal(3, appointment.DoctorId);
        Assert.Equal(appointmentDate, appointment.AppointmentDate);
        Assert.Equal("10:30 AM - 11:00 AM", appointment.TimeSlot);
        Assert.Equal("Confirmed", appointment.Status);
        Assert.Equal("Fever, Headache", appointment.Symptoms);
        Assert.Equal("Patient needs urgent care", appointment.Notes);
        Assert.Equal(createdDate, appointment.CreatedDate);
        Assert.True(appointment.IsActive);
        Assert.Equal("Receptionist", appointment.CreatedBy);
    }

    [Theory]
    [InlineData("Pending")]
    [InlineData("Confirmed")]
    [InlineData("Completed")]
    [InlineData("Cancelled")]
    public void Appointment_Status_ShouldAcceptValidValues(string status)
    {
        // Arrange
        var appointment = new Appointment();

        // Act
        appointment.Status = status;

        // Assert
        Assert.Equal(status, appointment.Status);
    }

    [Fact]
    public void Appointment_Symptoms_ShouldBeNullable()
    {
        // Arrange
        var appointment = new Appointment();

        // Act & Assert
        Assert.Null(appointment.Symptoms);

        // Act - Set value
        appointment.Symptoms = "Cough, Cold";

        // Assert
        Assert.NotNull(appointment.Symptoms);
        Assert.Equal("Cough, Cold", appointment.Symptoms);
    }

    [Fact]
    public void Appointment_Notes_ShouldBeNullable()
    {
        // Arrange
        var appointment = new Appointment();

        // Act & Assert
        Assert.Null(appointment.Notes);

        // Act - Set value
        appointment.Notes = "Follow-up required";

        // Assert
        Assert.NotNull(appointment.Notes);
        Assert.Equal("Follow-up required", appointment.Notes);
    }

    [Fact]
    public void Appointment_ModifiedDate_ShouldBeNullable()
    {
        // Arrange
        var appointment = new Appointment();

        // Act & Assert
        Assert.Null(appointment.ModifiedDate);

        // Act - Set value
        var modifiedDate = DateTime.UtcNow;
        appointment.ModifiedDate = modifiedDate;

        // Assert
        Assert.NotNull(appointment.ModifiedDate);
        Assert.Equal(modifiedDate, appointment.ModifiedDate);
    }

    [Fact]
    public void Appointment_ModifiedBy_ShouldBeNullable()
    {
        // Arrange
        var appointment = new Appointment();

        // Act & Assert
        Assert.Null(appointment.ModifiedBy);

        // Act - Set value
        appointment.ModifiedBy = "UpdatedByAdmin";

        // Assert
        Assert.NotNull(appointment.ModifiedBy);
        Assert.Equal("UpdatedByAdmin", appointment.ModifiedBy);
    }

    [Fact]
    public void Appointment_NavigationProperties_ShouldBeInitializable()
    {
        // Arrange
        var appointment = new Appointment();
        var patient = new Patient { Id = 1, Name = "John Doe" };
        var doctor = new Doctor { Id = 2, Name = "Dr. Smith" };
        var treatmentHistory = new TreatmentHistory { Id = 1 };

        // Act
        appointment.Patient = patient;
        appointment.Doctor = doctor;
        appointment.TreatmentHistory = treatmentHistory;

        // Assert
        Assert.NotNull(appointment.Patient);
        Assert.NotNull(appointment.Doctor);
        Assert.NotNull(appointment.TreatmentHistory);
        Assert.Equal(patient, appointment.Patient);
        Assert.Equal(doctor, appointment.Doctor);
        Assert.Equal(treatmentHistory, appointment.TreatmentHistory);
    }

    [Fact]
    public void Appointment_TreatmentHistory_ShouldBeNullable()
    {
        // Arrange & Act
        var appointment = new Appointment();

        // Assert
        Assert.Null(appointment.TreatmentHistory);
    }

    [Fact]
    public void Appointment_DefaultStatus_ShouldBePending()
    {
        // Arrange & Act
        var appointment = new Appointment();

        // Assert
        Assert.Equal("Pending", appointment.Status);
    }
}
