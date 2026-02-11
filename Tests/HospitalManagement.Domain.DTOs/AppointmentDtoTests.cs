using HospitalManagement.Domain.DTOs;
using Xunit;

namespace Tests.HospitalManagement.Domain.DTOs;

public class AppointmentDtoTests
{
    [Fact]
    public void AppointmentDto_Constructor_ShouldInitializeDefaultValues()
    {
        // Arrange & Act
        var appointmentDto = new AppointmentDto();

        // Assert
        Assert.Equal(0, appointmentDto.Id);
        Assert.Equal(0, appointmentDto.PatientId);
        Assert.Equal(string.Empty, appointmentDto.PatientName);
        Assert.Equal(0, appointmentDto.DoctorId);
        Assert.Equal(string.Empty, appointmentDto.DoctorName);
        Assert.Equal("Pending", appointmentDto.Status);
    }

    [Fact]
    public void AppointmentDto_SetProperties_ShouldSetValues()
    {
        // Arrange
        var appointmentDto = new AppointmentDto();
        var appointmentDate = DateTime.UtcNow.AddDays(1);

        // Act
        appointmentDto.Id = 1;
        appointmentDto.PatientId = 10;
        appointmentDto.PatientName = "John Doe";
        appointmentDto.DoctorId = 20;
        appointmentDto.DoctorName = "Dr. Smith";
        appointmentDto.AppointmentDate = appointmentDate;
        appointmentDto.Symptoms = "Fever";
        appointmentDto.Status = "Confirmed";

        // Assert
        Assert.Equal(1, appointmentDto.Id);
        Assert.Equal(10, appointmentDto.PatientId);
        Assert.Equal("John Doe", appointmentDto.PatientName);
        Assert.Equal(20, appointmentDto.DoctorId);
        Assert.Equal("Dr. Smith", appointmentDto.DoctorName);
        Assert.Equal(appointmentDate, appointmentDto.AppointmentDate);
        Assert.Equal("Fever", appointmentDto.Symptoms);
        Assert.Equal("Confirmed", appointmentDto.Status);
    }

    [Fact]
    public void AppointmentDto_Symptoms_CanBeNull()
    {
        // Arrange & Act
        var appointmentDto = new AppointmentDto();

        // Assert
        Assert.Null(appointmentDto.Symptoms);
    }
}

public class AppointmentCreateDtoTests
{
    [Fact]
    public void AppointmentCreateDto_Constructor_ShouldInitializeDefaultValues()
    {
        // Arrange & Act
        var appointmentCreateDto = new AppointmentCreateDto();

        // Assert
        Assert.Equal(0, appointmentCreateDto.PatientId);
        Assert.Equal(0, appointmentCreateDto.DoctorId);
    }

    [Fact]
    public void AppointmentCreateDto_SetProperties_ShouldSetValues()
    {
        // Arrange
        var appointmentCreateDto = new AppointmentCreateDto();
        var appointmentDate = DateTime.UtcNow.AddDays(1);

        // Act
        appointmentCreateDto.PatientId = 10;
        appointmentCreateDto.DoctorId = 20;
        appointmentCreateDto.AppointmentDate = appointmentDate;
        appointmentCreateDto.Symptoms = "Headache";

        // Assert
        Assert.Equal(10, appointmentCreateDto.PatientId);
        Assert.Equal(20, appointmentCreateDto.DoctorId);
        Assert.Equal(appointmentDate, appointmentCreateDto.AppointmentDate);
        Assert.Equal("Headache", appointmentCreateDto.Symptoms);
    }
}

public class AppointmentUpdateDtoTests
{
    [Fact]
    public void AppointmentUpdateDto_Constructor_ShouldInitializeDefaultValues()
    {
        // Arrange & Act
        var appointmentUpdateDto = new AppointmentUpdateDto();

        // Assert
        Assert.Equal("Pending", appointmentUpdateDto.Status);
    }

    [Fact]
    public void AppointmentUpdateDto_SetProperties_ShouldSetValues()
    {
        // Arrange
        var appointmentUpdateDto = new AppointmentUpdateDto();
        var appointmentDate = DateTime.UtcNow.AddDays(1);

        // Act
        appointmentUpdateDto.AppointmentDate = appointmentDate;
        appointmentUpdateDto.Symptoms = "Cough";
        appointmentUpdateDto.Status = "Cancelled";

        // Assert
        Assert.Equal(appointmentDate, appointmentUpdateDto.AppointmentDate);
        Assert.Equal("Cough", appointmentUpdateDto.Symptoms);
        Assert.Equal("Cancelled", appointmentUpdateDto.Status);
    }
}
