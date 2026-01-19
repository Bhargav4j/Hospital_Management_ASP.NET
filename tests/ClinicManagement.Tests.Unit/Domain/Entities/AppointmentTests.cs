using Xunit;
using ClinicManagement.Domain.Entities;
using System;

namespace ClinicManagement.Tests.Unit.Domain.Entities;

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
        Assert.Null(appointment.Patient);
        Assert.Equal(0, appointment.DoctorId);
        Assert.Null(appointment.Doctor);
        Assert.Equal(default(DateTime), appointment.AppointmentDate);
        Assert.Equal(string.Empty, appointment.Status);
        Assert.Equal(string.Empty, appointment.Notes);
        Assert.Equal(default(DateTime), appointment.CreatedAt);
        Assert.Null(appointment.UpdatedAt);
    }

    [Fact]
    public void Appointment_Id_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var appointment = new Appointment();
        var expectedId = 300;

        // Act
        appointment.Id = expectedId;

        // Assert
        Assert.Equal(expectedId, appointment.Id);
    }

    [Fact]
    public void Appointment_PatientId_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var appointment = new Appointment();
        var expectedPatientId = 100;

        // Act
        appointment.PatientId = expectedPatientId;

        // Assert
        Assert.Equal(expectedPatientId, appointment.PatientId);
    }

    [Fact]
    public void Appointment_Patient_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var appointment = new Appointment();
        var expectedPatient = new Patient { Id = 1, UserId = 10 };

        // Act
        appointment.Patient = expectedPatient;

        // Assert
        Assert.Equal(expectedPatient, appointment.Patient);
        Assert.Equal(1, appointment.Patient.Id);
        Assert.Equal(10, appointment.Patient.UserId);
    }

    [Fact]
    public void Appointment_DoctorId_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var appointment = new Appointment();
        var expectedDoctorId = 200;

        // Act
        appointment.DoctorId = expectedDoctorId;

        // Assert
        Assert.Equal(expectedDoctorId, appointment.DoctorId);
    }

    [Fact]
    public void Appointment_Doctor_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var appointment = new Appointment();
        var expectedDoctor = new Doctor { Id = 2, Specialization = "Cardiology" };

        // Act
        appointment.Doctor = expectedDoctor;

        // Assert
        Assert.Equal(expectedDoctor, appointment.Doctor);
        Assert.Equal(2, appointment.Doctor.Id);
        Assert.Equal("Cardiology", appointment.Doctor.Specialization);
    }

    [Fact]
    public void Appointment_AppointmentDate_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var appointment = new Appointment();
        var expectedDate = new DateTime(2024, 12, 25, 10, 30, 0);

        // Act
        appointment.AppointmentDate = expectedDate;

        // Assert
        Assert.Equal(expectedDate, appointment.AppointmentDate);
    }

    [Fact]
    public void Appointment_Status_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var appointment = new Appointment();
        var expectedStatus = "Confirmed";

        // Act
        appointment.Status = expectedStatus;

        // Assert
        Assert.Equal(expectedStatus, appointment.Status);
    }

    [Fact]
    public void Appointment_Notes_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var appointment = new Appointment();
        var expectedNotes = "Patient needs follow-up";

        // Act
        appointment.Notes = expectedNotes;

        // Assert
        Assert.Equal(expectedNotes, appointment.Notes);
    }

    [Fact]
    public void Appointment_CreatedAt_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var appointment = new Appointment();
        var expectedCreatedAt = DateTime.Now;

        // Act
        appointment.CreatedAt = expectedCreatedAt;

        // Assert
        Assert.Equal(expectedCreatedAt, appointment.CreatedAt);
    }

    [Fact]
    public void Appointment_UpdatedAt_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var appointment = new Appointment();
        var expectedUpdatedAt = DateTime.Now;

        // Act
        appointment.UpdatedAt = expectedUpdatedAt;

        // Assert
        Assert.Equal(expectedUpdatedAt, appointment.UpdatedAt);
    }

    [Fact]
    public void Appointment_UpdatedAt_ShouldAcceptNullValue()
    {
        // Arrange
        var appointment = new Appointment();

        // Act
        appointment.UpdatedAt = null;

        // Assert
        Assert.Null(appointment.UpdatedAt);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(999)]
    [InlineData(-1)]
    public void Appointment_Id_ShouldAcceptVariousValues(int id)
    {
        // Arrange
        var appointment = new Appointment();

        // Act
        appointment.Id = id;

        // Assert
        Assert.Equal(id, appointment.Id);
    }

    [Theory]
    [InlineData("")]
    [InlineData("Pending")]
    [InlineData("Confirmed")]
    [InlineData("Cancelled")]
    [InlineData("Completed")]
    public void Appointment_Status_ShouldAcceptVariousValues(string status)
    {
        // Arrange
        var appointment = new Appointment();

        // Act
        appointment.Status = status;

        // Assert
        Assert.Equal(status, appointment.Status);
    }

    [Theory]
    [InlineData("")]
    [InlineData("Routine checkup")]
    [InlineData("Emergency consultation")]
    public void Appointment_Notes_ShouldAcceptVariousValues(string notes)
    {
        // Arrange
        var appointment = new Appointment();

        // Act
        appointment.Notes = notes;

        // Assert
        Assert.Equal(notes, appointment.Notes);
    }

    [Fact]
    public void Appointment_AllProperties_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var appointment = new Appointment();
        var expectedId = 999;
        var expectedPatientId = 50;
        var expectedPatient = new Patient { Id = 50 };
        var expectedDoctorId = 25;
        var expectedDoctor = new Doctor { Id = 25 };
        var expectedAppointmentDate = new DateTime(2025, 1, 15, 14, 0, 0);
        var expectedStatus = "Scheduled";
        var expectedNotes = "Annual physical examination";
        var expectedCreatedAt = DateTime.Now;
        var expectedUpdatedAt = DateTime.Now.AddHours(2);

        // Act
        appointment.Id = expectedId;
        appointment.PatientId = expectedPatientId;
        appointment.Patient = expectedPatient;
        appointment.DoctorId = expectedDoctorId;
        appointment.Doctor = expectedDoctor;
        appointment.AppointmentDate = expectedAppointmentDate;
        appointment.Status = expectedStatus;
        appointment.Notes = expectedNotes;
        appointment.CreatedAt = expectedCreatedAt;
        appointment.UpdatedAt = expectedUpdatedAt;

        // Assert
        Assert.Equal(expectedId, appointment.Id);
        Assert.Equal(expectedPatientId, appointment.PatientId);
        Assert.Equal(expectedPatient, appointment.Patient);
        Assert.Equal(expectedDoctorId, appointment.DoctorId);
        Assert.Equal(expectedDoctor, appointment.Doctor);
        Assert.Equal(expectedAppointmentDate, appointment.AppointmentDate);
        Assert.Equal(expectedStatus, appointment.Status);
        Assert.Equal(expectedNotes, appointment.Notes);
        Assert.Equal(expectedCreatedAt, appointment.CreatedAt);
        Assert.Equal(expectedUpdatedAt, appointment.UpdatedAt);
    }

    [Fact]
    public void Appointment_AppointmentDate_ShouldHandleFutureDates()
    {
        // Arrange
        var appointment = new Appointment();
        var futureDate = DateTime.Now.AddDays(30);

        // Act
        appointment.AppointmentDate = futureDate;

        // Assert
        Assert.Equal(futureDate, appointment.AppointmentDate);
        Assert.True(appointment.AppointmentDate > DateTime.Now);
    }

    [Fact]
    public void Appointment_AppointmentDate_ShouldHandlePastDates()
    {
        // Arrange
        var appointment = new Appointment();
        var pastDate = DateTime.Now.AddDays(-30);

        // Act
        appointment.AppointmentDate = pastDate;

        // Assert
        Assert.Equal(pastDate, appointment.AppointmentDate);
        Assert.True(appointment.AppointmentDate < DateTime.Now);
    }

    [Fact]
    public void Appointment_CreatedAtAndUpdatedAt_ShouldTrackTimestamps()
    {
        // Arrange
        var appointment = new Appointment();
        var createdTime = DateTime.Now;
        var updatedTime = createdTime.AddMinutes(10);

        // Act
        appointment.CreatedAt = createdTime;
        appointment.UpdatedAt = updatedTime;

        // Assert
        Assert.Equal(createdTime, appointment.CreatedAt);
        Assert.Equal(updatedTime, appointment.UpdatedAt);
        Assert.True(appointment.UpdatedAt > appointment.CreatedAt);
    }
}
