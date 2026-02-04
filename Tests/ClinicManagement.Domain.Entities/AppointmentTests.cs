using Xunit;
using ClinicManagement.Domain.Entities;
using ClinicManagement.Domain.Enums;
using System;

namespace ClinicManagement.Domain.Entities.Tests;

/// <summary>
/// Test class for Appointment entity
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
        Assert.Equal("System", appointment.CreatedBy);
        Assert.False(appointment.IsActive);
    }

    [Fact]
    public void Appointment_SetProperties_ShouldStoreValues()
    {
        // Arrange
        var appointment = new Appointment();
        var expectedId = 1;
        var expectedPatientId = 10;
        var expectedDoctorId = 20;
        var expectedAppointmentDate = new DateTime(2024, 12, 25);
        var expectedAppointmentTime = new TimeSpan(14, 30, 0);
        var expectedStatus = AppointmentStatus.Approved;
        var expectedSymptoms = "Fever and headache";
        var expectedDiagnosis = "Viral infection";
        var expectedPrescription = "Rest and fluids";
        var expectedCreatedDate = DateTime.UtcNow;
        var expectedModifiedDate = DateTime.UtcNow;
        var expectedIsActive = true;
        var expectedCreatedBy = "Doctor";
        var expectedModifiedBy = "Doctor";

        // Act
        appointment.Id = expectedId;
        appointment.PatientId = expectedPatientId;
        appointment.DoctorId = expectedDoctorId;
        appointment.AppointmentDate = expectedAppointmentDate;
        appointment.AppointmentTime = expectedAppointmentTime;
        appointment.Status = expectedStatus;
        appointment.Symptoms = expectedSymptoms;
        appointment.Diagnosis = expectedDiagnosis;
        appointment.Prescription = expectedPrescription;
        appointment.CreatedDate = expectedCreatedDate;
        appointment.ModifiedDate = expectedModifiedDate;
        appointment.IsActive = expectedIsActive;
        appointment.CreatedBy = expectedCreatedBy;
        appointment.ModifiedBy = expectedModifiedBy;

        // Assert
        Assert.Equal(expectedId, appointment.Id);
        Assert.Equal(expectedPatientId, appointment.PatientId);
        Assert.Equal(expectedDoctorId, appointment.DoctorId);
        Assert.Equal(expectedAppointmentDate, appointment.AppointmentDate);
        Assert.Equal(expectedAppointmentTime, appointment.AppointmentTime);
        Assert.Equal(expectedStatus, appointment.Status);
        Assert.Equal(expectedSymptoms, appointment.Symptoms);
        Assert.Equal(expectedDiagnosis, appointment.Diagnosis);
        Assert.Equal(expectedPrescription, appointment.Prescription);
        Assert.Equal(expectedCreatedDate, appointment.CreatedDate);
        Assert.Equal(expectedModifiedDate, appointment.ModifiedDate);
        Assert.Equal(expectedIsActive, appointment.IsActive);
        Assert.Equal(expectedCreatedBy, appointment.CreatedBy);
        Assert.Equal(expectedModifiedBy, appointment.ModifiedBy);
    }

    [Fact]
    public void Appointment_Symptoms_ShouldAcceptNull()
    {
        // Arrange
        var appointment = new Appointment();

        // Act
        appointment.Symptoms = null;

        // Assert
        Assert.Null(appointment.Symptoms);
    }

    [Fact]
    public void Appointment_Diagnosis_ShouldAcceptNull()
    {
        // Arrange
        var appointment = new Appointment();

        // Act
        appointment.Diagnosis = null;

        // Assert
        Assert.Null(appointment.Diagnosis);
    }

    [Fact]
    public void Appointment_Prescription_ShouldAcceptNull()
    {
        // Arrange
        var appointment = new Appointment();

        // Act
        appointment.Prescription = null;

        // Assert
        Assert.Null(appointment.Prescription);
    }

    [Fact]
    public void Appointment_ModifiedDate_ShouldAcceptNull()
    {
        // Arrange
        var appointment = new Appointment();

        // Act
        appointment.ModifiedDate = null;

        // Assert
        Assert.Null(appointment.ModifiedDate);
    }

    [Fact]
    public void Appointment_ModifiedBy_ShouldAcceptNull()
    {
        // Arrange
        var appointment = new Appointment();

        // Act
        appointment.ModifiedBy = null;

        // Assert
        Assert.Null(appointment.ModifiedBy);
    }

    [Fact]
    public void Appointment_Patient_ShouldStorePatientReference()
    {
        // Arrange
        var appointment = new Appointment();
        var patient = new Patient { Id = 1, Name = "John Doe" };

        // Act
        appointment.Patient = patient;

        // Assert
        Assert.NotNull(appointment.Patient);
        Assert.Equal(patient.Id, appointment.Patient.Id);
        Assert.Equal(patient.Name, appointment.Patient.Name);
    }

    [Fact]
    public void Appointment_Doctor_ShouldStoreDoctorReference()
    {
        // Arrange
        var appointment = new Appointment();
        var doctor = new Doctor { Id = 1, Name = "Dr. Smith" };

        // Act
        appointment.Doctor = doctor;

        // Assert
        Assert.NotNull(appointment.Doctor);
        Assert.Equal(doctor.Id, appointment.Doctor.Id);
        Assert.Equal(doctor.Name, appointment.Doctor.Name);
    }

    [Theory]
    [InlineData(AppointmentStatus.Pending)]
    [InlineData(AppointmentStatus.Approved)]
    [InlineData(AppointmentStatus.Rejected)]
    [InlineData(AppointmentStatus.Completed)]
    [InlineData(AppointmentStatus.Cancelled)]
    public void Appointment_Status_ShouldAcceptAllStatusValues(AppointmentStatus status)
    {
        // Arrange
        var appointment = new Appointment();

        // Act
        appointment.Status = status;

        // Assert
        Assert.Equal(status, appointment.Status);
    }

    [Fact]
    public void Appointment_IsActive_ShouldToggleBetweenTrueAndFalse()
    {
        // Arrange
        var appointment = new Appointment();

        // Act & Assert
        appointment.IsActive = true;
        Assert.True(appointment.IsActive);

        appointment.IsActive = false;
        Assert.False(appointment.IsActive);
    }

    [Fact]
    public void Appointment_AppointmentTime_ShouldStoreTimeSpanValue()
    {
        // Arrange
        var appointment = new Appointment();
        var time = new TimeSpan(10, 30, 0);

        // Act
        appointment.AppointmentTime = time;

        // Assert
        Assert.Equal(time, appointment.AppointmentTime);
    }

    [Fact]
    public void Appointment_AppointmentDate_ShouldStoreDateTimeValue()
    {
        // Arrange
        var appointment = new Appointment();
        var date = new DateTime(2024, 11, 15);

        // Act
        appointment.AppointmentDate = date;

        // Assert
        Assert.Equal(date, appointment.AppointmentDate);
    }
}
