using HospitalManagement.Domain.Entities;
using Xunit;

namespace Tests.HospitalManagement.Domain.Entities;

public class MedicalHistoryTests
{
    [Fact]
    public void MedicalHistory_Constructor_ShouldInitializeDefaultValues()
    {
        // Arrange & Act
        var medicalHistory = new MedicalHistory();

        // Assert
        Assert.Equal(0, medicalHistory.Id);
        Assert.Equal(0, medicalHistory.PatientId);
        Assert.Equal(string.Empty, medicalHistory.Diagnosis);
        Assert.Equal(string.Empty, medicalHistory.Treatment);
        Assert.Equal("System", medicalHistory.CreatedBy);
        Assert.False(medicalHistory.IsActive);
    }

    [Fact]
    public void MedicalHistory_SetProperties_ShouldSetValues()
    {
        // Arrange
        var medicalHistory = new MedicalHistory();
        var visitDate = DateTime.UtcNow.AddDays(-7);
        var createdDate = DateTime.UtcNow;

        // Act
        medicalHistory.Id = 1;
        medicalHistory.PatientId = 10;
        medicalHistory.Diagnosis = "Common cold";
        medicalHistory.Treatment = "Rest and fluids";
        medicalHistory.VisitDate = visitDate;
        medicalHistory.Notes = "Patient recovering well";
        medicalHistory.CreatedDate = createdDate;
        medicalHistory.IsActive = true;

        // Assert
        Assert.Equal(1, medicalHistory.Id);
        Assert.Equal(10, medicalHistory.PatientId);
        Assert.Equal("Common cold", medicalHistory.Diagnosis);
        Assert.Equal("Rest and fluids", medicalHistory.Treatment);
        Assert.Equal(visitDate, medicalHistory.VisitDate);
        Assert.Equal("Patient recovering well", medicalHistory.Notes);
        Assert.Equal(createdDate, medicalHistory.CreatedDate);
        Assert.True(medicalHistory.IsActive);
    }

    [Fact]
    public void MedicalHistory_Notes_CanBeNull()
    {
        // Arrange & Act
        var medicalHistory = new MedicalHistory();

        // Assert
        Assert.Null(medicalHistory.Notes);
    }

    [Fact]
    public void MedicalHistory_Notes_CanBeSet()
    {
        // Arrange
        var medicalHistory = new MedicalHistory();

        // Act
        medicalHistory.Notes = "Follow-up required";

        // Assert
        Assert.Equal("Follow-up required", medicalHistory.Notes);
    }

    [Fact]
    public void MedicalHistory_Diagnosis_CanBeEmpty()
    {
        // Arrange & Act
        var medicalHistory = new MedicalHistory { Diagnosis = string.Empty };

        // Assert
        Assert.Equal(string.Empty, medicalHistory.Diagnosis);
    }

    [Fact]
    public void MedicalHistory_Treatment_CanBeEmpty()
    {
        // Arrange & Act
        var medicalHistory = new MedicalHistory { Treatment = string.Empty };

        // Assert
        Assert.Equal(string.Empty, medicalHistory.Treatment);
    }

    [Fact]
    public void MedicalHistory_ModifiedDate_CanBeNull()
    {
        // Arrange & Act
        var medicalHistory = new MedicalHistory();

        // Assert
        Assert.Null(medicalHistory.ModifiedDate);
    }

    [Fact]
    public void MedicalHistory_ModifiedDate_CanBeSet()
    {
        // Arrange
        var medicalHistory = new MedicalHistory();
        var modifiedDate = DateTime.UtcNow;

        // Act
        medicalHistory.ModifiedDate = modifiedDate;

        // Assert
        Assert.Equal(modifiedDate, medicalHistory.ModifiedDate);
    }

    [Fact]
    public void MedicalHistory_ModifiedBy_CanBeNull()
    {
        // Arrange & Act
        var medicalHistory = new MedicalHistory();

        // Assert
        Assert.Null(medicalHistory.ModifiedBy);
    }

    [Fact]
    public void MedicalHistory_ModifiedBy_CanBeSet()
    {
        // Arrange
        var medicalHistory = new MedicalHistory();

        // Act
        medicalHistory.ModifiedBy = "Admin";

        // Assert
        Assert.Equal("Admin", medicalHistory.ModifiedBy);
    }

    [Fact]
    public void MedicalHistory_Patient_CanBeSet()
    {
        // Arrange
        var medicalHistory = new MedicalHistory();
        var patient = new User { Id = 10, Name = "Patient Name" };

        // Act
        medicalHistory.Patient = patient;

        // Assert
        Assert.NotNull(medicalHistory.Patient);
        Assert.Equal(10, medicalHistory.Patient.Id);
    }

    [Fact]
    public void MedicalHistory_CreatedBy_DefaultsToSystem()
    {
        // Arrange & Act
        var medicalHistory = new MedicalHistory();

        // Assert
        Assert.Equal("System", medicalHistory.CreatedBy);
    }
}
