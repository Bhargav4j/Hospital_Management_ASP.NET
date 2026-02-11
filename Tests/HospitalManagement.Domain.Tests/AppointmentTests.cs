using Xunit;
using HospitalManagement.Domain.Entities;

namespace HospitalManagement.Domain.Entities.Tests;

public class AppointmentTests
{
    [Fact]
    public void Appointment_Constructor_InitializesWithDefaultValues()
    {
        // Arrange & Act
        var appointment = new Appointment();

        // Assert
        Assert.NotNull(appointment);
        Assert.Equal(0, appointment.Id);
        Assert.Equal(0, appointment.PatientId);
        Assert.Equal(0, appointment.DoctorId);
        Assert.Equal(default(DateTime), appointment.AppointmentDate);
        Assert.Equal(string.Empty, appointment.AppointmentTime);
        Assert.Equal(string.Empty, appointment.Status);
        Assert.Null(appointment.Symptoms);
        Assert.Null(appointment.Notes);
        Assert.Equal(default(DateTime), appointment.CreatedDate);
        Assert.Null(appointment.ModifiedDate);
        Assert.False(appointment.IsActive);
        Assert.Equal(string.Empty, appointment.CreatedBy);
        Assert.Null(appointment.ModifiedBy);
        Assert.Null(appointment.Patient);
        Assert.Null(appointment.Doctor);
        Assert.NotNull(appointment.Treatments);
        Assert.Empty(appointment.Treatments);
    }

    [Fact]
    public void Appointment_SetId_StoresValue()
    {
        // Arrange
        var appointment = new Appointment();
        var expectedId = 456;

        // Act
        appointment.Id = expectedId;

        // Assert
        Assert.Equal(expectedId, appointment.Id);
    }

    [Fact]
    public void Appointment_SetPatientId_StoresValue()
    {
        // Arrange
        var appointment = new Appointment();
        var expectedPatientId = 10;

        // Act
        appointment.PatientId = expectedPatientId;

        // Assert
        Assert.Equal(expectedPatientId, appointment.PatientId);
    }

    [Fact]
    public void Appointment_SetDoctorId_StoresValue()
    {
        // Arrange
        var appointment = new Appointment();
        var expectedDoctorId = 20;

        // Act
        appointment.DoctorId = expectedDoctorId;

        // Assert
        Assert.Equal(expectedDoctorId, appointment.DoctorId);
    }

    [Fact]
    public void Appointment_SetAppointmentDate_StoresValue()
    {
        // Arrange
        var appointment = new Appointment();
        var expectedDate = new DateTime(2025, 6, 15);

        // Act
        appointment.AppointmentDate = expectedDate;

        // Assert
        Assert.Equal(expectedDate, appointment.AppointmentDate);
    }

    [Fact]
    public void Appointment_SetAppointmentTime_StoresValue()
    {
        // Arrange
        var appointment = new Appointment();
        var expectedTime = "10:00 AM";

        // Act
        appointment.AppointmentTime = expectedTime;

        // Assert
        Assert.Equal(expectedTime, appointment.AppointmentTime);
    }

    [Fact]
    public void Appointment_SetStatus_WithPending_StoresValue()
    {
        // Arrange
        var appointment = new Appointment();
        var expectedStatus = "Pending";

        // Act
        appointment.Status = expectedStatus;

        // Assert
        Assert.Equal(expectedStatus, appointment.Status);
    }

    [Fact]
    public void Appointment_SetStatus_WithApproved_StoresValue()
    {
        // Arrange
        var appointment = new Appointment();
        var expectedStatus = "Approved";

        // Act
        appointment.Status = expectedStatus;

        // Assert
        Assert.Equal(expectedStatus, appointment.Status);
    }

    [Fact]
    public void Appointment_SetStatus_WithCompleted_StoresValue()
    {
        // Arrange
        var appointment = new Appointment();
        var expectedStatus = "Completed";

        // Act
        appointment.Status = expectedStatus;

        // Assert
        Assert.Equal(expectedStatus, appointment.Status);
    }

    [Fact]
    public void Appointment_SetStatus_WithCancelled_StoresValue()
    {
        // Arrange
        var appointment = new Appointment();
        var expectedStatus = "Cancelled";

        // Act
        appointment.Status = expectedStatus;

        // Assert
        Assert.Equal(expectedStatus, appointment.Status);
    }

    [Fact]
    public void Appointment_SetSymptoms_WithNull_StoresValue()
    {
        // Arrange
        var appointment = new Appointment();

        // Act
        appointment.Symptoms = null;

        // Assert
        Assert.Null(appointment.Symptoms);
    }

    [Fact]
    public void Appointment_SetSymptoms_WithValue_StoresValue()
    {
        // Arrange
        var appointment = new Appointment();
        var expectedSymptoms = "Fever, Headache";

        // Act
        appointment.Symptoms = expectedSymptoms;

        // Assert
        Assert.Equal(expectedSymptoms, appointment.Symptoms);
    }

    [Fact]
    public void Appointment_SetNotes_WithNull_StoresValue()
    {
        // Arrange
        var appointment = new Appointment();

        // Act
        appointment.Notes = null;

        // Assert
        Assert.Null(appointment.Notes);
    }

    [Fact]
    public void Appointment_SetNotes_WithValue_StoresValue()
    {
        // Arrange
        var appointment = new Appointment();
        var expectedNotes = "Patient requires follow-up in 2 weeks";

        // Act
        appointment.Notes = expectedNotes;

        // Assert
        Assert.Equal(expectedNotes, appointment.Notes);
    }

    [Fact]
    public void Appointment_SetCreatedDate_StoresValue()
    {
        // Arrange
        var appointment = new Appointment();
        var expectedDate = DateTime.UtcNow;

        // Act
        appointment.CreatedDate = expectedDate;

        // Assert
        Assert.Equal(expectedDate, appointment.CreatedDate);
    }

    [Fact]
    public void Appointment_SetModifiedDate_WithNull_StoresValue()
    {
        // Arrange
        var appointment = new Appointment();

        // Act
        appointment.ModifiedDate = null;

        // Assert
        Assert.Null(appointment.ModifiedDate);
    }

    [Fact]
    public void Appointment_SetModifiedDate_WithValue_StoresValue()
    {
        // Arrange
        var appointment = new Appointment();
        var expectedDate = DateTime.UtcNow;

        // Act
        appointment.ModifiedDate = expectedDate;

        // Assert
        Assert.Equal(expectedDate, appointment.ModifiedDate);
    }

    [Fact]
    public void Appointment_SetIsActive_WithTrue_StoresValue()
    {
        // Arrange
        var appointment = new Appointment();

        // Act
        appointment.IsActive = true;

        // Assert
        Assert.True(appointment.IsActive);
    }

    [Fact]
    public void Appointment_SetIsActive_WithFalse_StoresValue()
    {
        // Arrange
        var appointment = new Appointment();

        // Act
        appointment.IsActive = false;

        // Assert
        Assert.False(appointment.IsActive);
    }

    [Fact]
    public void Appointment_SetCreatedBy_StoresValue()
    {
        // Arrange
        var appointment = new Appointment();
        var expectedCreatedBy = "receptionist";

        // Act
        appointment.CreatedBy = expectedCreatedBy;

        // Assert
        Assert.Equal(expectedCreatedBy, appointment.CreatedBy);
    }

    [Fact]
    public void Appointment_SetModifiedBy_WithNull_StoresValue()
    {
        // Arrange
        var appointment = new Appointment();

        // Act
        appointment.ModifiedBy = null;

        // Assert
        Assert.Null(appointment.ModifiedBy);
    }

    [Fact]
    public void Appointment_SetModifiedBy_WithValue_StoresValue()
    {
        // Arrange
        var appointment = new Appointment();
        var expectedModifiedBy = "doctor";

        // Act
        appointment.ModifiedBy = expectedModifiedBy;

        // Assert
        Assert.Equal(expectedModifiedBy, appointment.ModifiedBy);
    }

    [Fact]
    public void Appointment_SetPatient_StoresValue()
    {
        // Arrange
        var appointment = new Appointment();
        var patient = new Patient { Id = 1, FirstName = "John", LastName = "Doe" };

        // Act
        appointment.Patient = patient;

        // Assert
        Assert.NotNull(appointment.Patient);
        Assert.Equal(1, appointment.Patient.Id);
        Assert.Equal("John", appointment.Patient.FirstName);
        Assert.Equal("Doe", appointment.Patient.LastName);
    }

    [Fact]
    public void Appointment_SetDoctor_StoresValue()
    {
        // Arrange
        var appointment = new Appointment();
        var doctor = new Doctor { Id = 2, FirstName = "Jane", LastName = "Smith" };

        // Act
        appointment.Doctor = doctor;

        // Assert
        Assert.NotNull(appointment.Doctor);
        Assert.Equal(2, appointment.Doctor.Id);
        Assert.Equal("Jane", appointment.Doctor.FirstName);
        Assert.Equal("Smith", appointment.Doctor.LastName);
    }

    [Fact]
    public void Appointment_Treatments_CanAddItems()
    {
        // Arrange
        var appointment = new Appointment();
        var treatment = new Treatment { Id = 1 };

        // Act
        appointment.Treatments.Add(treatment);

        // Assert
        Assert.Single(appointment.Treatments);
        Assert.Contains(treatment, appointment.Treatments);
    }

    [Fact]
    public void Appointment_AllProperties_CanBeSetTogether()
    {
        // Arrange
        var appointment = new Appointment();
        var patient = new Patient { Id = 10, FirstName = "Alice", LastName = "Brown" };
        var doctor = new Doctor { Id = 20, FirstName = "Bob", LastName = "Green" };
        var appointmentDate = new DateTime(2025, 7, 20);
        var createdDate = DateTime.UtcNow;
        var modifiedDate = DateTime.UtcNow;

        // Act
        appointment.Id = 100;
        appointment.PatientId = 10;
        appointment.DoctorId = 20;
        appointment.AppointmentDate = appointmentDate;
        appointment.AppointmentTime = "2:00 PM";
        appointment.Status = "Approved";
        appointment.Symptoms = "Chest pain, Shortness of breath";
        appointment.Notes = "Emergency case";
        appointment.CreatedDate = createdDate;
        appointment.ModifiedDate = modifiedDate;
        appointment.IsActive = true;
        appointment.CreatedBy = "nurse";
        appointment.ModifiedBy = "doctor";
        appointment.Patient = patient;
        appointment.Doctor = doctor;

        // Assert
        Assert.Equal(100, appointment.Id);
        Assert.Equal(10, appointment.PatientId);
        Assert.Equal(20, appointment.DoctorId);
        Assert.Equal(appointmentDate, appointment.AppointmentDate);
        Assert.Equal("2:00 PM", appointment.AppointmentTime);
        Assert.Equal("Approved", appointment.Status);
        Assert.Equal("Chest pain, Shortness of breath", appointment.Symptoms);
        Assert.Equal("Emergency case", appointment.Notes);
        Assert.Equal(createdDate, appointment.CreatedDate);
        Assert.Equal(modifiedDate, appointment.ModifiedDate);
        Assert.True(appointment.IsActive);
        Assert.Equal("nurse", appointment.CreatedBy);
        Assert.Equal("doctor", appointment.ModifiedBy);
        Assert.NotNull(appointment.Patient);
        Assert.NotNull(appointment.Doctor);
    }
}
