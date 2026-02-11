using HospitalManagement.Domain.DTOs;
using Xunit;

namespace Tests.HospitalManagement.Domain.DTOs;

public class MedicalHistoryDtoTests
{
    [Fact]
    public void MedicalHistoryDto_Constructor_ShouldInitializeDefaultValues()
    {
        // Arrange & Act
        var medicalHistoryDto = new MedicalHistoryDto();

        // Assert
        Assert.Equal(0, medicalHistoryDto.Id);
        Assert.Equal(0, medicalHistoryDto.PatientId);
        Assert.Equal(string.Empty, medicalHistoryDto.PatientName);
        Assert.Equal(string.Empty, medicalHistoryDto.Diagnosis);
        Assert.Equal(string.Empty, medicalHistoryDto.Treatment);
    }

    [Fact]
    public void MedicalHistoryDto_SetProperties_ShouldSetValues()
    {
        // Arrange
        var medicalHistoryDto = new MedicalHistoryDto();
        var visitDate = DateTime.UtcNow.AddDays(-7);

        // Act
        medicalHistoryDto.Id = 1;
        medicalHistoryDto.PatientId = 10;
        medicalHistoryDto.PatientName = "John Doe";
        medicalHistoryDto.Diagnosis = "Flu";
        medicalHistoryDto.Treatment = "Rest";
        medicalHistoryDto.VisitDate = visitDate;
        medicalHistoryDto.Notes = "Follow-up needed";

        // Assert
        Assert.Equal(1, medicalHistoryDto.Id);
        Assert.Equal(10, medicalHistoryDto.PatientId);
        Assert.Equal("John Doe", medicalHistoryDto.PatientName);
        Assert.Equal("Flu", medicalHistoryDto.Diagnosis);
        Assert.Equal("Rest", medicalHistoryDto.Treatment);
        Assert.Equal(visitDate, medicalHistoryDto.VisitDate);
        Assert.Equal("Follow-up needed", medicalHistoryDto.Notes);
    }

    [Fact]
    public void MedicalHistoryDto_Notes_CanBeNull()
    {
        // Arrange & Act
        var medicalHistoryDto = new MedicalHistoryDto();

        // Assert
        Assert.Null(medicalHistoryDto.Notes);
    }
}

public class MedicalHistoryCreateDtoTests
{
    [Fact]
    public void MedicalHistoryCreateDto_Constructor_ShouldInitializeDefaultValues()
    {
        // Arrange & Act
        var medicalHistoryCreateDto = new MedicalHistoryCreateDto();

        // Assert
        Assert.Equal(0, medicalHistoryCreateDto.PatientId);
        Assert.Equal(string.Empty, medicalHistoryCreateDto.Diagnosis);
        Assert.Equal(string.Empty, medicalHistoryCreateDto.Treatment);
    }

    [Fact]
    public void MedicalHistoryCreateDto_SetProperties_ShouldSetValues()
    {
        // Arrange
        var medicalHistoryCreateDto = new MedicalHistoryCreateDto();
        var visitDate = DateTime.UtcNow;

        // Act
        medicalHistoryCreateDto.PatientId = 10;
        medicalHistoryCreateDto.Diagnosis = "Cold";
        medicalHistoryCreateDto.Treatment = "Medicine";
        medicalHistoryCreateDto.VisitDate = visitDate;
        medicalHistoryCreateDto.Notes = "Additional notes";

        // Assert
        Assert.Equal(10, medicalHistoryCreateDto.PatientId);
        Assert.Equal("Cold", medicalHistoryCreateDto.Diagnosis);
        Assert.Equal("Medicine", medicalHistoryCreateDto.Treatment);
        Assert.Equal(visitDate, medicalHistoryCreateDto.VisitDate);
        Assert.Equal("Additional notes", medicalHistoryCreateDto.Notes);
    }
}

public class MedicalHistoryUpdateDtoTests
{
    [Fact]
    public void MedicalHistoryUpdateDto_Constructor_ShouldInitializeDefaultValues()
    {
        // Arrange & Act
        var medicalHistoryUpdateDto = new MedicalHistoryUpdateDto();

        // Assert
        Assert.Equal(string.Empty, medicalHistoryUpdateDto.Diagnosis);
        Assert.Equal(string.Empty, medicalHistoryUpdateDto.Treatment);
    }

    [Fact]
    public void MedicalHistoryUpdateDto_SetProperties_ShouldSetValues()
    {
        // Arrange
        var medicalHistoryUpdateDto = new MedicalHistoryUpdateDto();
        var visitDate = DateTime.UtcNow;

        // Act
        medicalHistoryUpdateDto.Diagnosis = "Updated diagnosis";
        medicalHistoryUpdateDto.Treatment = "Updated treatment";
        medicalHistoryUpdateDto.VisitDate = visitDate;
        medicalHistoryUpdateDto.Notes = "Updated notes";

        // Assert
        Assert.Equal("Updated diagnosis", medicalHistoryUpdateDto.Diagnosis);
        Assert.Equal("Updated treatment", medicalHistoryUpdateDto.Treatment);
        Assert.Equal(visitDate, medicalHistoryUpdateDto.VisitDate);
        Assert.Equal("Updated notes", medicalHistoryUpdateDto.Notes);
    }
}
