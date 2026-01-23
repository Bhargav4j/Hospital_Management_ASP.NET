using Xunit;
using ClinicManagement.Domain.Entities;
using System;

namespace ClinicManagement.Domain.Entities.Tests;

/// <summary>
/// Unit tests for TreatmentHistory entity
/// </summary>
public class TreatmentHistoryTests
{
    [Fact]
    public void TreatmentHistory_Constructor_ShouldInitializeWithDefaultValues()
    {
        // Arrange & Act
        var treatmentHistory = new TreatmentHistory();

        // Assert
        Assert.Equal(0, treatmentHistory.Id);
        Assert.Equal(0, treatmentHistory.AppointmentId);
        Assert.Equal(0, treatmentHistory.PatientId);
        Assert.Equal(0, treatmentHistory.DoctorId);
        Assert.Equal(string.Empty, treatmentHistory.Diagnosis);
        Assert.Equal(string.Empty, treatmentHistory.Prescription);
        Assert.Null(treatmentHistory.Tests);
        Assert.Null(treatmentHistory.Remarks);
        Assert.Equal("System", treatmentHistory.CreatedBy);
        Assert.False(treatmentHistory.IsActive);
    }

    [Fact]
    public void TreatmentHistory_SetProperties_ShouldStoreCorrectValues()
    {
        // Arrange
        var treatmentHistory = new TreatmentHistory();
        var treatmentDate = new DateTime(2023, 6, 15);
        var createdDate = DateTime.UtcNow;

        // Act
        treatmentHistory.Id = 1;
        treatmentHistory.AppointmentId = 10;
        treatmentHistory.PatientId = 5;
        treatmentHistory.DoctorId = 3;
        treatmentHistory.Diagnosis = "Common Cold";
        treatmentHistory.Prescription = "Paracetamol 500mg";
        treatmentHistory.Tests = "Blood Test";
        treatmentHistory.Remarks = "Rest advised";
        treatmentHistory.TreatmentDate = treatmentDate;
        treatmentHistory.CreatedDate = createdDate;
        treatmentHistory.IsActive = true;
        treatmentHistory.CreatedBy = "Doctor";

        // Assert
        Assert.Equal(1, treatmentHistory.Id);
        Assert.Equal(10, treatmentHistory.AppointmentId);
        Assert.Equal(5, treatmentHistory.PatientId);
        Assert.Equal(3, treatmentHistory.DoctorId);
        Assert.Equal("Common Cold", treatmentHistory.Diagnosis);
        Assert.Equal("Paracetamol 500mg", treatmentHistory.Prescription);
        Assert.Equal("Blood Test", treatmentHistory.Tests);
        Assert.Equal("Rest advised", treatmentHistory.Remarks);
        Assert.Equal(treatmentDate, treatmentHistory.TreatmentDate);
        Assert.Equal(createdDate, treatmentHistory.CreatedDate);
        Assert.True(treatmentHistory.IsActive);
        Assert.Equal("Doctor", treatmentHistory.CreatedBy);
    }

    [Fact]
    public void TreatmentHistory_Tests_ShouldBeNullable()
    {
        // Arrange
        var treatmentHistory = new TreatmentHistory();

        // Act & Assert
        Assert.Null(treatmentHistory.Tests);

        // Act - Set value
        treatmentHistory.Tests = "X-Ray, ECG";

        // Assert
        Assert.NotNull(treatmentHistory.Tests);
        Assert.Equal("X-Ray, ECG", treatmentHistory.Tests);
    }

    [Fact]
    public void TreatmentHistory_Remarks_ShouldBeNullable()
    {
        // Arrange
        var treatmentHistory = new TreatmentHistory();

        // Act & Assert
        Assert.Null(treatmentHistory.Remarks);

        // Act - Set value
        treatmentHistory.Remarks = "Patient recovering well";

        // Assert
        Assert.NotNull(treatmentHistory.Remarks);
        Assert.Equal("Patient recovering well", treatmentHistory.Remarks);
    }

    [Fact]
    public void TreatmentHistory_ModifiedDate_ShouldBeNullable()
    {
        // Arrange
        var treatmentHistory = new TreatmentHistory();

        // Act & Assert
        Assert.Null(treatmentHistory.ModifiedDate);

        // Act - Set value
        var modifiedDate = DateTime.UtcNow;
        treatmentHistory.ModifiedDate = modifiedDate;

        // Assert
        Assert.NotNull(treatmentHistory.ModifiedDate);
        Assert.Equal(modifiedDate, treatmentHistory.ModifiedDate);
    }

    [Fact]
    public void TreatmentHistory_ModifiedBy_ShouldBeNullable()
    {
        // Arrange
        var treatmentHistory = new TreatmentHistory();

        // Act & Assert
        Assert.Null(treatmentHistory.ModifiedBy);

        // Act - Set value
        treatmentHistory.ModifiedBy = "UpdatedByDoctor";

        // Assert
        Assert.NotNull(treatmentHistory.ModifiedBy);
        Assert.Equal("UpdatedByDoctor", treatmentHistory.ModifiedBy);
    }

    [Theory]
    [InlineData("Flu", "Tamiflu")]
    [InlineData("Diabetes", "Insulin")]
    [InlineData("Hypertension", "Lisinopril")]
    public void TreatmentHistory_DiagnosisAndPrescription_ShouldAcceptVariousValues(string diagnosis, string prescription)
    {
        // Arrange
        var treatmentHistory = new TreatmentHistory();

        // Act
        treatmentHistory.Diagnosis = diagnosis;
        treatmentHistory.Prescription = prescription;

        // Assert
        Assert.Equal(diagnosis, treatmentHistory.Diagnosis);
        Assert.Equal(prescription, treatmentHistory.Prescription);
    }

    [Fact]
    public void TreatmentHistory_NavigationProperties_ShouldBeInitializable()
    {
        // Arrange
        var treatmentHistory = new TreatmentHistory();
        var appointment = new Appointment { Id = 1 };
        var patient = new Patient { Id = 1 };
        var doctor = new Doctor { Id = 1 };

        // Act
        treatmentHistory.Appointment = appointment;
        treatmentHistory.Patient = patient;
        treatmentHistory.Doctor = doctor;

        // Assert
        Assert.NotNull(treatmentHistory.Appointment);
        Assert.NotNull(treatmentHistory.Patient);
        Assert.NotNull(treatmentHistory.Doctor);
        Assert.Equal(appointment, treatmentHistory.Appointment);
        Assert.Equal(patient, treatmentHistory.Patient);
        Assert.Equal(doctor, treatmentHistory.Doctor);
    }
}
