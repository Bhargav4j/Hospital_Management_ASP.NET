using HospitalManagement.Domain.Entities;
using Xunit;

namespace Tests.HospitalManagement.Domain.Entities;

public class AppointmentTests
{
    [Fact]
    public void Appointment_Constructor_ShouldInitializeDefaultValues()
    {
        // Arrange & Act
        var appointment = new Appointment();

        // Assert
        Assert.Equal(0, appointment.Id);
        Assert.Equal(0, appointment.PatientId);
        Assert.Equal(0, appointment.DoctorId);
        Assert.Equal("Pending", appointment.Status);
        Assert.Equal("System", appointment.CreatedBy);
        Assert.False(appointment.IsActive);
    }

    [Fact]
    public void Appointment_SetProperties_ShouldSetValues()
    {
        // Arrange
        var appointment = new Appointment();
        var appointmentDate = DateTime.UtcNow.AddDays(1);
        var createdDate = DateTime.UtcNow;

        // Act
        appointment.Id = 1;
        appointment.PatientId = 10;
        appointment.DoctorId = 20;
        appointment.AppointmentDate = appointmentDate;
        appointment.Symptoms = "Fever and headache";
        appointment.Status = "Confirmed";
        appointment.CreatedDate = createdDate;
        appointment.IsActive = true;

        // Assert
        Assert.Equal(1, appointment.Id);
        Assert.Equal(10, appointment.PatientId);
        Assert.Equal(20, appointment.DoctorId);
        Assert.Equal(appointmentDate, appointment.AppointmentDate);
        Assert.Equal("Fever and headache", appointment.Symptoms);
        Assert.Equal("Confirmed", appointment.Status);
        Assert.Equal(createdDate, appointment.CreatedDate);
        Assert.True(appointment.IsActive);
    }

    [Fact]
    public void Appointment_Symptoms_CanBeNull()
    {
        // Arrange & Act
        var appointment = new Appointment();

        // Assert
        Assert.Null(appointment.Symptoms);
    }

    [Fact]
    public void Appointment_Status_DefaultsToPending()
    {
        // Arrange & Act
        var appointment = new Appointment();

        // Assert
        Assert.Equal("Pending", appointment.Status);
    }

    [Theory]
    [InlineData("Pending")]
    [InlineData("Confirmed")]
    [InlineData("Cancelled")]
    [InlineData("Completed")]
    public void Appointment_Status_CanBeSetToAnyValue(string status)
    {
        // Arrange
        var appointment = new Appointment();

        // Act
        appointment.Status = status;

        // Assert
        Assert.Equal(status, appointment.Status);
    }

    [Fact]
    public void Appointment_ModifiedDate_CanBeNull()
    {
        // Arrange & Act
        var appointment = new Appointment();

        // Assert
        Assert.Null(appointment.ModifiedDate);
    }

    [Fact]
    public void Appointment_ModifiedDate_CanBeSet()
    {
        // Arrange
        var appointment = new Appointment();
        var modifiedDate = DateTime.UtcNow;

        // Act
        appointment.ModifiedDate = modifiedDate;

        // Assert
        Assert.Equal(modifiedDate, appointment.ModifiedDate);
    }

    [Fact]
    public void Appointment_ModifiedBy_CanBeNull()
    {
        // Arrange & Act
        var appointment = new Appointment();

        // Assert
        Assert.Null(appointment.ModifiedBy);
    }

    [Fact]
    public void Appointment_ModifiedBy_CanBeSet()
    {
        // Arrange
        var appointment = new Appointment();

        // Act
        appointment.ModifiedBy = "Admin";

        // Assert
        Assert.Equal("Admin", appointment.ModifiedBy);
    }

    [Fact]
    public void Appointment_Patient_CanBeSet()
    {
        // Arrange
        var appointment = new Appointment();
        var patient = new User { Id = 10, Name = "Patient Name" };

        // Act
        appointment.Patient = patient;

        // Assert
        Assert.NotNull(appointment.Patient);
        Assert.Equal(10, appointment.Patient.Id);
    }

    [Fact]
    public void Appointment_Doctor_CanBeSet()
    {
        // Arrange
        var appointment = new Appointment();
        var doctor = new User { Id = 20, Name = "Doctor Name" };

        // Act
        appointment.Doctor = doctor;

        // Assert
        Assert.NotNull(appointment.Doctor);
        Assert.Equal(20, appointment.Doctor.Id);
    }

    [Fact]
    public void Appointment_CreatedBy_DefaultsToSystem()
    {
        // Arrange & Act
        var appointment = new Appointment();

        // Assert
        Assert.Equal("System", appointment.CreatedBy);
    }
}
