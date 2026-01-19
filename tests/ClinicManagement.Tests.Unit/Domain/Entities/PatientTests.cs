using Xunit;
using ClinicManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ClinicManagement.Tests.Unit.Domain.Entities;

public class PatientTests
{
    [Fact]
    public void Patient_Constructor_ShouldInitializeWithDefaultValues()
    {
        // Arrange & Act
        var patient = new Patient();

        // Assert
        Assert.Equal(0, patient.Id);
        Assert.Equal(0, patient.UserId);
        Assert.Null(patient.User);
        Assert.Equal(string.Empty, patient.MedicalHistory);
        Assert.Equal(default(DateTime), patient.CreatedAt);
        Assert.Null(patient.UpdatedAt);
        Assert.NotNull(patient.Appointments);
        Assert.Empty(patient.Appointments);
        Assert.NotNull(patient.Bills);
        Assert.Empty(patient.Bills);
    }

    [Fact]
    public void Patient_Id_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var patient = new Patient();
        var expectedId = 100;

        // Act
        patient.Id = expectedId;

        // Assert
        Assert.Equal(expectedId, patient.Id);
    }

    [Fact]
    public void Patient_UserId_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var patient = new Patient();
        var expectedUserId = 50;

        // Act
        patient.UserId = expectedUserId;

        // Assert
        Assert.Equal(expectedUserId, patient.UserId);
    }

    [Fact]
    public void Patient_User_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var patient = new Patient();
        var expectedUser = new User { Id = 1, Name = "John Doe" };

        // Act
        patient.User = expectedUser;

        // Assert
        Assert.Equal(expectedUser, patient.User);
        Assert.Equal(1, patient.User.Id);
        Assert.Equal("John Doe", patient.User.Name);
    }

    [Fact]
    public void Patient_MedicalHistory_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var patient = new Patient();
        var expectedHistory = "Diabetes, Hypertension";

        // Act
        patient.MedicalHistory = expectedHistory;

        // Assert
        Assert.Equal(expectedHistory, patient.MedicalHistory);
    }

    [Fact]
    public void Patient_CreatedAt_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var patient = new Patient();
        var expectedCreatedAt = DateTime.Now;

        // Act
        patient.CreatedAt = expectedCreatedAt;

        // Assert
        Assert.Equal(expectedCreatedAt, patient.CreatedAt);
    }

    [Fact]
    public void Patient_UpdatedAt_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var patient = new Patient();
        var expectedUpdatedAt = DateTime.Now;

        // Act
        patient.UpdatedAt = expectedUpdatedAt;

        // Assert
        Assert.Equal(expectedUpdatedAt, patient.UpdatedAt);
    }

    [Fact]
    public void Patient_UpdatedAt_ShouldAcceptNullValue()
    {
        // Arrange
        var patient = new Patient();

        // Act
        patient.UpdatedAt = null;

        // Assert
        Assert.Null(patient.UpdatedAt);
    }

    [Fact]
    public void Patient_Appointments_ShouldInitializeAsEmptyList()
    {
        // Arrange & Act
        var patient = new Patient();

        // Assert
        Assert.NotNull(patient.Appointments);
        Assert.IsAssignableFrom<ICollection<Appointment>>(patient.Appointments);
        Assert.Empty(patient.Appointments);
    }

    [Fact]
    public void Patient_Appointments_ShouldAddAppointmentsCorrectly()
    {
        // Arrange
        var patient = new Patient();
        var appointment1 = new Appointment { Id = 1 };
        var appointment2 = new Appointment { Id = 2 };

        // Act
        patient.Appointments.Add(appointment1);
        patient.Appointments.Add(appointment2);

        // Assert
        Assert.Equal(2, patient.Appointments.Count);
        Assert.Contains(appointment1, patient.Appointments);
        Assert.Contains(appointment2, patient.Appointments);
    }

    [Fact]
    public void Patient_Bills_ShouldInitializeAsEmptyList()
    {
        // Arrange & Act
        var patient = new Patient();

        // Assert
        Assert.NotNull(patient.Bills);
        Assert.IsAssignableFrom<ICollection<Bill>>(patient.Bills);
        Assert.Empty(patient.Bills);
    }

    [Fact]
    public void Patient_Bills_ShouldAddBillsCorrectly()
    {
        // Arrange
        var patient = new Patient();
        var bill1 = new Bill { Id = 1 };
        var bill2 = new Bill { Id = 2 };

        // Act
        patient.Bills.Add(bill1);
        patient.Bills.Add(bill2);

        // Assert
        Assert.Equal(2, patient.Bills.Count);
        Assert.Contains(bill1, patient.Bills);
        Assert.Contains(bill2, patient.Bills);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(999)]
    [InlineData(-1)]
    public void Patient_Id_ShouldAcceptVariousValues(int id)
    {
        // Arrange
        var patient = new Patient();

        // Act
        patient.Id = id;

        // Assert
        Assert.Equal(id, patient.Id);
    }

    [Theory]
    [InlineData("")]
    [InlineData("No medical history")]
    [InlineData("Cancer, Heart Disease, Stroke")]
    public void Patient_MedicalHistory_ShouldAcceptVariousValues(string history)
    {
        // Arrange
        var patient = new Patient();

        // Act
        patient.MedicalHistory = history;

        // Assert
        Assert.Equal(history, patient.MedicalHistory);
    }

    [Fact]
    public void Patient_AllProperties_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var patient = new Patient();
        var expectedId = 10;
        var expectedUserId = 20;
        var expectedUser = new User { Id = 20, Name = "Test User" };
        var expectedHistory = "Asthma";
        var expectedCreatedAt = DateTime.Now;
        var expectedUpdatedAt = DateTime.Now.AddHours(2);
        var appointment = new Appointment { Id = 1 };
        var bill = new Bill { Id = 1 };

        // Act
        patient.Id = expectedId;
        patient.UserId = expectedUserId;
        patient.User = expectedUser;
        patient.MedicalHistory = expectedHistory;
        patient.CreatedAt = expectedCreatedAt;
        patient.UpdatedAt = expectedUpdatedAt;
        patient.Appointments.Add(appointment);
        patient.Bills.Add(bill);

        // Assert
        Assert.Equal(expectedId, patient.Id);
        Assert.Equal(expectedUserId, patient.UserId);
        Assert.Equal(expectedUser, patient.User);
        Assert.Equal(expectedHistory, patient.MedicalHistory);
        Assert.Equal(expectedCreatedAt, patient.CreatedAt);
        Assert.Equal(expectedUpdatedAt, patient.UpdatedAt);
        Assert.Single(patient.Appointments);
        Assert.Single(patient.Bills);
    }

    [Fact]
    public void Patient_Appointments_ShouldAllowReassignment()
    {
        // Arrange
        var patient = new Patient();
        var newAppointments = new List<Appointment>
        {
            new Appointment { Id = 1 },
            new Appointment { Id = 2 }
        };

        // Act
        patient.Appointments = newAppointments;

        // Assert
        Assert.Equal(2, patient.Appointments.Count);
        Assert.Same(newAppointments, patient.Appointments);
    }

    [Fact]
    public void Patient_Bills_ShouldAllowReassignment()
    {
        // Arrange
        var patient = new Patient();
        var newBills = new List<Bill>
        {
            new Bill { Id = 1 },
            new Bill { Id = 2 },
            new Bill { Id = 3 }
        };

        // Act
        patient.Bills = newBills;

        // Assert
        Assert.Equal(3, patient.Bills.Count);
        Assert.Same(newBills, patient.Bills);
    }
}
