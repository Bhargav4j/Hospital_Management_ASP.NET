using Xunit;
using ClinicManagement.Application.DTOs;
using System;

namespace ClinicManagement.Tests.Unit.Application.DTOs;

public class AppointmentDtoTests
{
    [Fact]
    public void AppointmentDto_Constructor_ShouldInitializeWithDefaultValues()
    {
        // Arrange & Act
        var appointmentDto = new AppointmentDto();

        // Assert
        Assert.Equal(0, appointmentDto.Id);
        Assert.Equal(0, appointmentDto.PatientId);
        Assert.Equal(string.Empty, appointmentDto.PatientName);
        Assert.Equal(0, appointmentDto.DoctorId);
        Assert.Equal(string.Empty, appointmentDto.DoctorName);
        Assert.Equal(default(DateTime), appointmentDto.AppointmentDate);
        Assert.Equal(string.Empty, appointmentDto.Status);
        Assert.Equal(string.Empty, appointmentDto.Notes);
    }

    [Fact]
    public void AppointmentDto_Id_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var appointmentDto = new AppointmentDto();
        var expectedId = 500;

        // Act
        appointmentDto.Id = expectedId;

        // Assert
        Assert.Equal(expectedId, appointmentDto.Id);
    }

    [Fact]
    public void AppointmentDto_PatientId_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var appointmentDto = new AppointmentDto();
        var expectedPatientId = 100;

        // Act
        appointmentDto.PatientId = expectedPatientId;

        // Assert
        Assert.Equal(expectedPatientId, appointmentDto.PatientId);
    }

    [Fact]
    public void AppointmentDto_PatientName_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var appointmentDto = new AppointmentDto();
        var expectedPatientName = "John Doe";

        // Act
        appointmentDto.PatientName = expectedPatientName;

        // Assert
        Assert.Equal(expectedPatientName, appointmentDto.PatientName);
    }

    [Fact]
    public void AppointmentDto_DoctorId_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var appointmentDto = new AppointmentDto();
        var expectedDoctorId = 200;

        // Act
        appointmentDto.DoctorId = expectedDoctorId;

        // Assert
        Assert.Equal(expectedDoctorId, appointmentDto.DoctorId);
    }

    [Fact]
    public void AppointmentDto_DoctorName_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var appointmentDto = new AppointmentDto();
        var expectedDoctorName = "Dr. Smith";

        // Act
        appointmentDto.DoctorName = expectedDoctorName;

        // Assert
        Assert.Equal(expectedDoctorName, appointmentDto.DoctorName);
    }

    [Fact]
    public void AppointmentDto_AllProperties_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var appointmentDto = new AppointmentDto();
        var expectedId = 1;
        var expectedPatientId = 10;
        var expectedPatientName = "Jane Doe";
        var expectedDoctorId = 20;
        var expectedDoctorName = "Dr. Johnson";
        var expectedAppointmentDate = new DateTime(2024, 12, 25, 10, 0, 0);
        var expectedStatus = "Confirmed";
        var expectedNotes = "Annual checkup";

        // Act
        appointmentDto.Id = expectedId;
        appointmentDto.PatientId = expectedPatientId;
        appointmentDto.PatientName = expectedPatientName;
        appointmentDto.DoctorId = expectedDoctorId;
        appointmentDto.DoctorName = expectedDoctorName;
        appointmentDto.AppointmentDate = expectedAppointmentDate;
        appointmentDto.Status = expectedStatus;
        appointmentDto.Notes = expectedNotes;

        // Assert
        Assert.Equal(expectedId, appointmentDto.Id);
        Assert.Equal(expectedPatientId, appointmentDto.PatientId);
        Assert.Equal(expectedPatientName, appointmentDto.PatientName);
        Assert.Equal(expectedDoctorId, appointmentDto.DoctorId);
        Assert.Equal(expectedDoctorName, appointmentDto.DoctorName);
        Assert.Equal(expectedAppointmentDate, appointmentDto.AppointmentDate);
        Assert.Equal(expectedStatus, appointmentDto.Status);
        Assert.Equal(expectedNotes, appointmentDto.Notes);
    }

    [Theory]
    [InlineData("")]
    [InlineData("Pending")]
    [InlineData("Confirmed")]
    [InlineData("Cancelled")]
    [InlineData("Completed")]
    public void AppointmentDto_Status_ShouldAcceptVariousValues(string status)
    {
        // Arrange
        var appointmentDto = new AppointmentDto();

        // Act
        appointmentDto.Status = status;

        // Assert
        Assert.Equal(status, appointmentDto.Status);
    }
}

public class CreateAppointmentDtoTests
{
    [Fact]
    public void CreateAppointmentDto_Constructor_ShouldInitializeWithDefaultValues()
    {
        // Arrange & Act
        var createAppointmentDto = new CreateAppointmentDto();

        // Assert
        Assert.Equal(0, createAppointmentDto.PatientId);
        Assert.Equal(0, createAppointmentDto.DoctorId);
        Assert.Equal(default(DateTime), createAppointmentDto.AppointmentDate);
        Assert.Equal(string.Empty, createAppointmentDto.Notes);
    }

    [Fact]
    public void CreateAppointmentDto_PatientId_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var createAppointmentDto = new CreateAppointmentDto();
        var expectedPatientId = 50;

        // Act
        createAppointmentDto.PatientId = expectedPatientId;

        // Assert
        Assert.Equal(expectedPatientId, createAppointmentDto.PatientId);
    }

    [Fact]
    public void CreateAppointmentDto_DoctorId_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var createAppointmentDto = new CreateAppointmentDto();
        var expectedDoctorId = 75;

        // Act
        createAppointmentDto.DoctorId = expectedDoctorId;

        // Assert
        Assert.Equal(expectedDoctorId, createAppointmentDto.DoctorId);
    }

    [Fact]
    public void CreateAppointmentDto_AppointmentDate_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var createAppointmentDto = new CreateAppointmentDto();
        var expectedAppointmentDate = new DateTime(2025, 1, 15, 14, 30, 0);

        // Act
        createAppointmentDto.AppointmentDate = expectedAppointmentDate;

        // Assert
        Assert.Equal(expectedAppointmentDate, createAppointmentDto.AppointmentDate);
    }

    [Fact]
    public void CreateAppointmentDto_Notes_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var createAppointmentDto = new CreateAppointmentDto();
        var expectedNotes = "Patient needs follow-up";

        // Act
        createAppointmentDto.Notes = expectedNotes;

        // Assert
        Assert.Equal(expectedNotes, createAppointmentDto.Notes);
    }

    [Fact]
    public void CreateAppointmentDto_AllProperties_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var createAppointmentDto = new CreateAppointmentDto();
        var expectedPatientId = 123;
        var expectedDoctorId = 456;
        var expectedAppointmentDate = new DateTime(2025, 3, 20, 9, 0, 0);
        var expectedNotes = "Routine checkup";

        // Act
        createAppointmentDto.PatientId = expectedPatientId;
        createAppointmentDto.DoctorId = expectedDoctorId;
        createAppointmentDto.AppointmentDate = expectedAppointmentDate;
        createAppointmentDto.Notes = expectedNotes;

        // Assert
        Assert.Equal(expectedPatientId, createAppointmentDto.PatientId);
        Assert.Equal(expectedDoctorId, createAppointmentDto.DoctorId);
        Assert.Equal(expectedAppointmentDate, createAppointmentDto.AppointmentDate);
        Assert.Equal(expectedNotes, createAppointmentDto.Notes);
    }

    [Theory]
    [InlineData("")]
    [InlineData("Emergency visit")]
    [InlineData("Follow-up consultation")]
    public void CreateAppointmentDto_Notes_ShouldAcceptVariousValues(string notes)
    {
        // Arrange
        var createAppointmentDto = new CreateAppointmentDto();

        // Act
        createAppointmentDto.Notes = notes;

        // Assert
        Assert.Equal(notes, createAppointmentDto.Notes);
    }

    [Fact]
    public void CreateAppointmentDto_AppointmentDate_ShouldHandleFutureDates()
    {
        // Arrange
        var createAppointmentDto = new CreateAppointmentDto();
        var futureDate = DateTime.Now.AddDays(30);

        // Act
        createAppointmentDto.AppointmentDate = futureDate;

        // Assert
        Assert.Equal(futureDate, createAppointmentDto.AppointmentDate);
        Assert.True(createAppointmentDto.AppointmentDate > DateTime.Now);
    }
}

public class UpdateAppointmentDtoTests
{
    [Fact]
    public void UpdateAppointmentDto_Constructor_ShouldInitializeWithDefaultValues()
    {
        // Arrange & Act
        var updateAppointmentDto = new UpdateAppointmentDto();

        // Assert
        Assert.Equal(0, updateAppointmentDto.Id);
        Assert.Equal(string.Empty, updateAppointmentDto.Status);
        Assert.Equal(string.Empty, updateAppointmentDto.Notes);
    }

    [Fact]
    public void UpdateAppointmentDto_Id_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var updateAppointmentDto = new UpdateAppointmentDto();
        var expectedId = 999;

        // Act
        updateAppointmentDto.Id = expectedId;

        // Assert
        Assert.Equal(expectedId, updateAppointmentDto.Id);
    }

    [Fact]
    public void UpdateAppointmentDto_Status_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var updateAppointmentDto = new UpdateAppointmentDto();
        var expectedStatus = "Completed";

        // Act
        updateAppointmentDto.Status = expectedStatus;

        // Assert
        Assert.Equal(expectedStatus, updateAppointmentDto.Status);
    }

    [Fact]
    public void UpdateAppointmentDto_Notes_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var updateAppointmentDto = new UpdateAppointmentDto();
        var expectedNotes = "Updated notes";

        // Act
        updateAppointmentDto.Notes = expectedNotes;

        // Assert
        Assert.Equal(expectedNotes, updateAppointmentDto.Notes);
    }

    [Fact]
    public void UpdateAppointmentDto_AllProperties_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var updateAppointmentDto = new UpdateAppointmentDto();
        var expectedId = 777;
        var expectedStatus = "Rescheduled";
        var expectedNotes = "Patient requested new time";

        // Act
        updateAppointmentDto.Id = expectedId;
        updateAppointmentDto.Status = expectedStatus;
        updateAppointmentDto.Notes = expectedNotes;

        // Assert
        Assert.Equal(expectedId, updateAppointmentDto.Id);
        Assert.Equal(expectedStatus, updateAppointmentDto.Status);
        Assert.Equal(expectedNotes, updateAppointmentDto.Notes);
    }

    [Theory]
    [InlineData("")]
    [InlineData("Pending")]
    [InlineData("Confirmed")]
    [InlineData("Cancelled")]
    [InlineData("Completed")]
    [InlineData("No-Show")]
    public void UpdateAppointmentDto_Status_ShouldAcceptVariousValues(string status)
    {
        // Arrange
        var updateAppointmentDto = new UpdateAppointmentDto();

        // Act
        updateAppointmentDto.Status = status;

        // Assert
        Assert.Equal(status, updateAppointmentDto.Status);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(999)]
    [InlineData(-1)]
    public void UpdateAppointmentDto_Id_ShouldAcceptVariousValues(int id)
    {
        // Arrange
        var updateAppointmentDto = new UpdateAppointmentDto();

        // Act
        updateAppointmentDto.Id = id;

        // Assert
        Assert.Equal(id, updateAppointmentDto.Id);
    }

    [Fact]
    public void UpdateAppointmentDto_Notes_ShouldAllowEmptyString()
    {
        // Arrange
        var updateAppointmentDto = new UpdateAppointmentDto();

        // Act
        updateAppointmentDto.Notes = string.Empty;

        // Assert
        Assert.Equal(string.Empty, updateAppointmentDto.Notes);
        Assert.Empty(updateAppointmentDto.Notes);
    }
}
