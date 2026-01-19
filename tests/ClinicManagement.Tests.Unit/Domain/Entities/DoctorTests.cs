using Xunit;
using ClinicManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ClinicManagement.Tests.Unit.Domain.Entities;

public class DoctorTests
{
    [Fact]
    public void Doctor_Constructor_ShouldInitializeWithDefaultValues()
    {
        // Arrange & Act
        var doctor = new Doctor();

        // Assert
        Assert.Equal(0, doctor.Id);
        Assert.Equal(0, doctor.UserId);
        Assert.Null(doctor.User);
        Assert.Equal(string.Empty, doctor.Specialization);
        Assert.Equal(string.Empty, doctor.Qualifications);
        Assert.Equal(0, doctor.ClinicId);
        Assert.Null(doctor.Clinic);
        Assert.Equal(default(DateTime), doctor.CreatedAt);
        Assert.Null(doctor.UpdatedAt);
        Assert.NotNull(doctor.Appointments);
        Assert.Empty(doctor.Appointments);
    }

    [Fact]
    public void Doctor_Id_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var doctor = new Doctor();
        var expectedId = 200;

        // Act
        doctor.Id = expectedId;

        // Assert
        Assert.Equal(expectedId, doctor.Id);
    }

    [Fact]
    public void Doctor_UserId_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var doctor = new Doctor();
        var expectedUserId = 150;

        // Act
        doctor.UserId = expectedUserId;

        // Assert
        Assert.Equal(expectedUserId, doctor.UserId);
    }

    [Fact]
    public void Doctor_User_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var doctor = new Doctor();
        var expectedUser = new User { Id = 1, Name = "Dr. Smith" };

        // Act
        doctor.User = expectedUser;

        // Assert
        Assert.Equal(expectedUser, doctor.User);
        Assert.Equal(1, doctor.User.Id);
        Assert.Equal("Dr. Smith", doctor.User.Name);
    }

    [Fact]
    public void Doctor_Specialization_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var doctor = new Doctor();
        var expectedSpecialization = "Cardiology";

        // Act
        doctor.Specialization = expectedSpecialization;

        // Assert
        Assert.Equal(expectedSpecialization, doctor.Specialization);
    }

    [Fact]
    public void Doctor_Qualifications_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var doctor = new Doctor();
        var expectedQualifications = "MD, PhD";

        // Act
        doctor.Qualifications = expectedQualifications;

        // Assert
        Assert.Equal(expectedQualifications, doctor.Qualifications);
    }

    [Fact]
    public void Doctor_ClinicId_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var doctor = new Doctor();
        var expectedClinicId = 10;

        // Act
        doctor.ClinicId = expectedClinicId;

        // Assert
        Assert.Equal(expectedClinicId, doctor.ClinicId);
    }

    [Fact]
    public void Doctor_Clinic_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var doctor = new Doctor();
        var expectedClinic = new Clinic { Id = 5, Name = "City Hospital" };

        // Act
        doctor.Clinic = expectedClinic;

        // Assert
        Assert.Equal(expectedClinic, doctor.Clinic);
        Assert.Equal(5, doctor.Clinic.Id);
        Assert.Equal("City Hospital", doctor.Clinic.Name);
    }

    [Fact]
    public void Doctor_CreatedAt_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var doctor = new Doctor();
        var expectedCreatedAt = DateTime.Now;

        // Act
        doctor.CreatedAt = expectedCreatedAt;

        // Assert
        Assert.Equal(expectedCreatedAt, doctor.CreatedAt);
    }

    [Fact]
    public void Doctor_UpdatedAt_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var doctor = new Doctor();
        var expectedUpdatedAt = DateTime.Now;

        // Act
        doctor.UpdatedAt = expectedUpdatedAt;

        // Assert
        Assert.Equal(expectedUpdatedAt, doctor.UpdatedAt);
    }

    [Fact]
    public void Doctor_UpdatedAt_ShouldAcceptNullValue()
    {
        // Arrange
        var doctor = new Doctor();

        // Act
        doctor.UpdatedAt = null;

        // Assert
        Assert.Null(doctor.UpdatedAt);
    }

    [Fact]
    public void Doctor_Appointments_ShouldInitializeAsEmptyList()
    {
        // Arrange & Act
        var doctor = new Doctor();

        // Assert
        Assert.NotNull(doctor.Appointments);
        Assert.IsAssignableFrom<ICollection<Appointment>>(doctor.Appointments);
        Assert.Empty(doctor.Appointments);
    }

    [Fact]
    public void Doctor_Appointments_ShouldAddAppointmentsCorrectly()
    {
        // Arrange
        var doctor = new Doctor();
        var appointment1 = new Appointment { Id = 1 };
        var appointment2 = new Appointment { Id = 2 };
        var appointment3 = new Appointment { Id = 3 };

        // Act
        doctor.Appointments.Add(appointment1);
        doctor.Appointments.Add(appointment2);
        doctor.Appointments.Add(appointment3);

        // Assert
        Assert.Equal(3, doctor.Appointments.Count);
        Assert.Contains(appointment1, doctor.Appointments);
        Assert.Contains(appointment2, doctor.Appointments);
        Assert.Contains(appointment3, doctor.Appointments);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(999)]
    [InlineData(-1)]
    public void Doctor_Id_ShouldAcceptVariousValues(int id)
    {
        // Arrange
        var doctor = new Doctor();

        // Act
        doctor.Id = id;

        // Assert
        Assert.Equal(id, doctor.Id);
    }

    [Theory]
    [InlineData("")]
    [InlineData("Neurology")]
    [InlineData("Pediatrics")]
    [InlineData("Orthopedics")]
    public void Doctor_Specialization_ShouldAcceptVariousValues(string specialization)
    {
        // Arrange
        var doctor = new Doctor();

        // Act
        doctor.Specialization = specialization;

        // Assert
        Assert.Equal(specialization, doctor.Specialization);
    }

    [Theory]
    [InlineData("")]
    [InlineData("MD")]
    [InlineData("MBBS, MS")]
    [InlineData("PhD, FRCS")]
    public void Doctor_Qualifications_ShouldAcceptVariousValues(string qualifications)
    {
        // Arrange
        var doctor = new Doctor();

        // Act
        doctor.Qualifications = qualifications;

        // Assert
        Assert.Equal(qualifications, doctor.Qualifications);
    }

    [Fact]
    public void Doctor_AllProperties_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var doctor = new Doctor();
        var expectedId = 15;
        var expectedUserId = 25;
        var expectedUser = new User { Id = 25, Name = "Dr. Johnson" };
        var expectedSpecialization = "Oncology";
        var expectedQualifications = "MD, FACS";
        var expectedClinicId = 3;
        var expectedClinic = new Clinic { Id = 3, Name = "Memorial Hospital" };
        var expectedCreatedAt = DateTime.Now;
        var expectedUpdatedAt = DateTime.Now.AddHours(3);
        var appointment = new Appointment { Id = 1 };

        // Act
        doctor.Id = expectedId;
        doctor.UserId = expectedUserId;
        doctor.User = expectedUser;
        doctor.Specialization = expectedSpecialization;
        doctor.Qualifications = expectedQualifications;
        doctor.ClinicId = expectedClinicId;
        doctor.Clinic = expectedClinic;
        doctor.CreatedAt = expectedCreatedAt;
        doctor.UpdatedAt = expectedUpdatedAt;
        doctor.Appointments.Add(appointment);

        // Assert
        Assert.Equal(expectedId, doctor.Id);
        Assert.Equal(expectedUserId, doctor.UserId);
        Assert.Equal(expectedUser, doctor.User);
        Assert.Equal(expectedSpecialization, doctor.Specialization);
        Assert.Equal(expectedQualifications, doctor.Qualifications);
        Assert.Equal(expectedClinicId, doctor.ClinicId);
        Assert.Equal(expectedClinic, doctor.Clinic);
        Assert.Equal(expectedCreatedAt, doctor.CreatedAt);
        Assert.Equal(expectedUpdatedAt, doctor.UpdatedAt);
        Assert.Single(doctor.Appointments);
    }

    [Fact]
    public void Doctor_Appointments_ShouldAllowReassignment()
    {
        // Arrange
        var doctor = new Doctor();
        var newAppointments = new List<Appointment>
        {
            new Appointment { Id = 10 },
            new Appointment { Id = 20 },
            new Appointment { Id = 30 }
        };

        // Act
        doctor.Appointments = newAppointments;

        // Assert
        Assert.Equal(3, doctor.Appointments.Count);
        Assert.Same(newAppointments, doctor.Appointments);
    }

    [Fact]
    public void Doctor_Appointments_ShouldRemoveAppointments()
    {
        // Arrange
        var doctor = new Doctor();
        var appointment = new Appointment { Id = 100 };
        doctor.Appointments.Add(appointment);

        // Act
        var removed = doctor.Appointments.Remove(appointment);

        // Assert
        Assert.True(removed);
        Assert.Empty(doctor.Appointments);
    }
}
