using Xunit;
using HospitalManagement.Domain.Entities;

namespace HospitalManagement.Domain.Entities.Tests;

public class TreatmentTests
{
    [Fact]
    public void Treatment_Constructor_InitializesWithDefaultValues()
    {
        // Arrange & Act
        var treatment = new Treatment();

        // Assert
        Assert.NotNull(treatment);
        Assert.Equal(0, treatment.Id);
        Assert.Equal(0, treatment.PatientId);
        Assert.Equal(0, treatment.DoctorId);
        Assert.Null(treatment.AppointmentId);
        Assert.Equal(default(DateTime), treatment.TreatmentDate);
        Assert.Equal(string.Empty, treatment.Diagnosis);
        Assert.Equal(string.Empty, treatment.Prescription);
        Assert.Null(treatment.TestResults);
        Assert.Null(treatment.Notes);
        Assert.Equal(default(DateTime), treatment.CreatedDate);
        Assert.Null(treatment.ModifiedDate);
        Assert.False(treatment.IsActive);
        Assert.Equal(string.Empty, treatment.CreatedBy);
        Assert.Null(treatment.ModifiedBy);
        Assert.Null(treatment.Patient);
        Assert.Null(treatment.Doctor);
        Assert.Null(treatment.Appointment);
    }

    [Fact]
    public void Treatment_SetId_StoresValue()
    {
        // Arrange
        var treatment = new Treatment();
        var expectedId = 555;

        // Act
        treatment.Id = expectedId;

        // Assert
        Assert.Equal(expectedId, treatment.Id);
    }

    [Fact]
    public void Treatment_SetPatientId_StoresValue()
    {
        // Arrange
        var treatment = new Treatment();
        var expectedPatientId = 15;

        // Act
        treatment.PatientId = expectedPatientId;

        // Assert
        Assert.Equal(expectedPatientId, treatment.PatientId);
    }

    [Fact]
    public void Treatment_SetDoctorId_StoresValue()
    {
        // Arrange
        var treatment = new Treatment();
        var expectedDoctorId = 25;

        // Act
        treatment.DoctorId = expectedDoctorId;

        // Assert
        Assert.Equal(expectedDoctorId, treatment.DoctorId);
    }

    [Fact]
    public void Treatment_SetAppointmentId_WithNull_StoresValue()
    {
        // Arrange
        var treatment = new Treatment();

        // Act
        treatment.AppointmentId = null;

        // Assert
        Assert.Null(treatment.AppointmentId);
    }

    [Fact]
    public void Treatment_SetAppointmentId_WithValue_StoresValue()
    {
        // Arrange
        var treatment = new Treatment();
        var expectedAppointmentId = 100;

        // Act
        treatment.AppointmentId = expectedAppointmentId;

        // Assert
        Assert.Equal(expectedAppointmentId, treatment.AppointmentId);
    }

    [Fact]
    public void Treatment_SetTreatmentDate_StoresValue()
    {
        // Arrange
        var treatment = new Treatment();
        var expectedDate = new DateTime(2025, 8, 15);

        // Act
        treatment.TreatmentDate = expectedDate;

        // Assert
        Assert.Equal(expectedDate, treatment.TreatmentDate);
    }

    [Fact]
    public void Treatment_SetDiagnosis_StoresValue()
    {
        // Arrange
        var treatment = new Treatment();
        var expectedDiagnosis = "Hypertension";

        // Act
        treatment.Diagnosis = expectedDiagnosis;

        // Assert
        Assert.Equal(expectedDiagnosis, treatment.Diagnosis);
    }

    [Fact]
    public void Treatment_SetPrescription_StoresValue()
    {
        // Arrange
        var treatment = new Treatment();
        var expectedPrescription = "Aspirin 100mg, twice daily";

        // Act
        treatment.Prescription = expectedPrescription;

        // Assert
        Assert.Equal(expectedPrescription, treatment.Prescription);
    }

    [Fact]
    public void Treatment_SetTestResults_WithNull_StoresValue()
    {
        // Arrange
        var treatment = new Treatment();

        // Act
        treatment.TestResults = null;

        // Assert
        Assert.Null(treatment.TestResults);
    }

    [Fact]
    public void Treatment_SetTestResults_WithValue_StoresValue()
    {
        // Arrange
        var treatment = new Treatment();
        var expectedTestResults = "Blood pressure: 140/90";

        // Act
        treatment.TestResults = expectedTestResults;

        // Assert
        Assert.Equal(expectedTestResults, treatment.TestResults);
    }

    [Fact]
    public void Treatment_SetNotes_WithNull_StoresValue()
    {
        // Arrange
        var treatment = new Treatment();

        // Act
        treatment.Notes = null;

        // Assert
        Assert.Null(treatment.Notes);
    }

    [Fact]
    public void Treatment_SetNotes_WithValue_StoresValue()
    {
        // Arrange
        var treatment = new Treatment();
        var expectedNotes = "Patient advised to reduce salt intake";

        // Act
        treatment.Notes = expectedNotes;

        // Assert
        Assert.Equal(expectedNotes, treatment.Notes);
    }

    [Fact]
    public void Treatment_SetCreatedDate_StoresValue()
    {
        // Arrange
        var treatment = new Treatment();
        var expectedDate = DateTime.UtcNow;

        // Act
        treatment.CreatedDate = expectedDate;

        // Assert
        Assert.Equal(expectedDate, treatment.CreatedDate);
    }

    [Fact]
    public void Treatment_SetModifiedDate_WithNull_StoresValue()
    {
        // Arrange
        var treatment = new Treatment();

        // Act
        treatment.ModifiedDate = null;

        // Assert
        Assert.Null(treatment.ModifiedDate);
    }

    [Fact]
    public void Treatment_SetModifiedDate_WithValue_StoresValue()
    {
        // Arrange
        var treatment = new Treatment();
        var expectedDate = DateTime.UtcNow;

        // Act
        treatment.ModifiedDate = expectedDate;

        // Assert
        Assert.Equal(expectedDate, treatment.ModifiedDate);
    }

    [Fact]
    public void Treatment_SetIsActive_WithTrue_StoresValue()
    {
        // Arrange
        var treatment = new Treatment();

        // Act
        treatment.IsActive = true;

        // Assert
        Assert.True(treatment.IsActive);
    }

    [Fact]
    public void Treatment_SetIsActive_WithFalse_StoresValue()
    {
        // Arrange
        var treatment = new Treatment();

        // Act
        treatment.IsActive = false;

        // Assert
        Assert.False(treatment.IsActive);
    }

    [Fact]
    public void Treatment_SetCreatedBy_StoresValue()
    {
        // Arrange
        var treatment = new Treatment();
        var expectedCreatedBy = "doctor";

        // Act
        treatment.CreatedBy = expectedCreatedBy;

        // Assert
        Assert.Equal(expectedCreatedBy, treatment.CreatedBy);
    }

    [Fact]
    public void Treatment_SetModifiedBy_WithNull_StoresValue()
    {
        // Arrange
        var treatment = new Treatment();

        // Act
        treatment.ModifiedBy = null;

        // Assert
        Assert.Null(treatment.ModifiedBy);
    }

    [Fact]
    public void Treatment_SetModifiedBy_WithValue_StoresValue()
    {
        // Arrange
        var treatment = new Treatment();
        var expectedModifiedBy = "nurse";

        // Act
        treatment.ModifiedBy = expectedModifiedBy;

        // Assert
        Assert.Equal(expectedModifiedBy, treatment.ModifiedBy);
    }

    [Fact]
    public void Treatment_SetPatient_StoresValue()
    {
        // Arrange
        var treatment = new Treatment();
        var patient = new Patient { Id = 1, FirstName = "Emma", LastName = "Wilson" };

        // Act
        treatment.Patient = patient;

        // Assert
        Assert.NotNull(treatment.Patient);
        Assert.Equal(1, treatment.Patient.Id);
        Assert.Equal("Emma", treatment.Patient.FirstName);
        Assert.Equal("Wilson", treatment.Patient.LastName);
    }

    [Fact]
    public void Treatment_SetDoctor_StoresValue()
    {
        // Arrange
        var treatment = new Treatment();
        var doctor = new Doctor { Id = 2, FirstName = "Robert", LastName = "Davis" };

        // Act
        treatment.Doctor = doctor;

        // Assert
        Assert.NotNull(treatment.Doctor);
        Assert.Equal(2, treatment.Doctor.Id);
        Assert.Equal("Robert", treatment.Doctor.FirstName);
        Assert.Equal("Davis", treatment.Doctor.LastName);
    }

    [Fact]
    public void Treatment_SetAppointment_StoresValue()
    {
        // Arrange
        var treatment = new Treatment();
        var appointment = new Appointment { Id = 10, Status = "Completed" };

        // Act
        treatment.Appointment = appointment;

        // Assert
        Assert.NotNull(treatment.Appointment);
        Assert.Equal(10, treatment.Appointment.Id);
        Assert.Equal("Completed", treatment.Appointment.Status);
    }

    [Fact]
    public void Treatment_AllProperties_CanBeSetTogether()
    {
        // Arrange
        var treatment = new Treatment();
        var patient = new Patient { Id = 5, FirstName = "Lisa", LastName = "Anderson" };
        var doctor = new Doctor { Id = 10, FirstName = "Chris", LastName = "Martinez" };
        var appointment = new Appointment { Id = 50, Status = "Completed" };
        var treatmentDate = new DateTime(2025, 9, 10);
        var createdDate = DateTime.UtcNow;
        var modifiedDate = DateTime.UtcNow;

        // Act
        treatment.Id = 300;
        treatment.PatientId = 5;
        treatment.DoctorId = 10;
        treatment.AppointmentId = 50;
        treatment.TreatmentDate = treatmentDate;
        treatment.Diagnosis = "Type 2 Diabetes";
        treatment.Prescription = "Metformin 500mg, once daily";
        treatment.TestResults = "HbA1c: 7.2%";
        treatment.Notes = "Monitor blood sugar levels daily";
        treatment.CreatedDate = createdDate;
        treatment.ModifiedDate = modifiedDate;
        treatment.IsActive = true;
        treatment.CreatedBy = "doctor";
        treatment.ModifiedBy = "admin";
        treatment.Patient = patient;
        treatment.Doctor = doctor;
        treatment.Appointment = appointment;

        // Assert
        Assert.Equal(300, treatment.Id);
        Assert.Equal(5, treatment.PatientId);
        Assert.Equal(10, treatment.DoctorId);
        Assert.Equal(50, treatment.AppointmentId);
        Assert.Equal(treatmentDate, treatment.TreatmentDate);
        Assert.Equal("Type 2 Diabetes", treatment.Diagnosis);
        Assert.Equal("Metformin 500mg, once daily", treatment.Prescription);
        Assert.Equal("HbA1c: 7.2%", treatment.TestResults);
        Assert.Equal("Monitor blood sugar levels daily", treatment.Notes);
        Assert.Equal(createdDate, treatment.CreatedDate);
        Assert.Equal(modifiedDate, treatment.ModifiedDate);
        Assert.True(treatment.IsActive);
        Assert.Equal("doctor", treatment.CreatedBy);
        Assert.Equal("admin", treatment.ModifiedBy);
        Assert.NotNull(treatment.Patient);
        Assert.NotNull(treatment.Doctor);
        Assert.NotNull(treatment.Appointment);
    }

    [Theory]
    [InlineData("Common Cold")]
    [InlineData("Migraine")]
    [InlineData("Allergic Rhinitis")]
    [InlineData("Sprained Ankle")]
    public void Treatment_SetDiagnosis_WithDifferentValues_StoresCorrectly(string diagnosis)
    {
        // Arrange
        var treatment = new Treatment();

        // Act
        treatment.Diagnosis = diagnosis;

        // Assert
        Assert.Equal(diagnosis, treatment.Diagnosis);
    }
}
