using Xunit;
using ClinicManagement.Domain.Entities;
using System;

namespace ClinicManagement.Domain.Entities.Tests;

/// <summary>
/// Unit tests for Notification entity
/// </summary>
public class NotificationTests
{
    [Fact]
    public void Notification_Constructor_ShouldInitializeWithDefaultValues()
    {
        // Arrange & Act
        var notification = new Notification();

        // Assert
        Assert.Equal(0, notification.Id);
        Assert.Null(notification.PatientId);
        Assert.Null(notification.DoctorId);
        Assert.Equal(string.Empty, notification.Title);
        Assert.Equal(string.Empty, notification.Message);
        Assert.Equal("Info", notification.Type);
        Assert.False(notification.IsRead);
        Assert.Equal("System", notification.CreatedBy);
        Assert.False(notification.IsActive);
    }

    [Fact]
    public void Notification_SetProperties_ShouldStoreCorrectValues()
    {
        // Arrange
        var notification = new Notification();
        var createdDate = DateTime.UtcNow;

        // Act
        notification.Id = 1;
        notification.PatientId = 5;
        notification.DoctorId = 3;
        notification.Title = "Appointment Reminder";
        notification.Message = "Your appointment is scheduled for tomorrow";
        notification.Type = "Warning";
        notification.IsRead = true;
        notification.CreatedDate = createdDate;
        notification.IsActive = true;
        notification.CreatedBy = "System";

        // Assert
        Assert.Equal(1, notification.Id);
        Assert.Equal(5, notification.PatientId);
        Assert.Equal(3, notification.DoctorId);
        Assert.Equal("Appointment Reminder", notification.Title);
        Assert.Equal("Your appointment is scheduled for tomorrow", notification.Message);
        Assert.Equal("Warning", notification.Type);
        Assert.True(notification.IsRead);
        Assert.Equal(createdDate, notification.CreatedDate);
        Assert.True(notification.IsActive);
        Assert.Equal("System", notification.CreatedBy);
    }

    [Theory]
    [InlineData("Info")]
    [InlineData("Warning")]
    [InlineData("Success")]
    [InlineData("Error")]
    public void Notification_Type_ShouldAcceptValidValues(string type)
    {
        // Arrange
        var notification = new Notification();

        // Act
        notification.Type = type;

        // Assert
        Assert.Equal(type, notification.Type);
    }

    [Fact]
    public void Notification_PatientId_ShouldBeNullable()
    {
        // Arrange
        var notification = new Notification();

        // Act & Assert
        Assert.Null(notification.PatientId);

        // Act - Set value
        notification.PatientId = 10;

        // Assert
        Assert.NotNull(notification.PatientId);
        Assert.Equal(10, notification.PatientId);
    }

    [Fact]
    public void Notification_DoctorId_ShouldBeNullable()
    {
        // Arrange
        var notification = new Notification();

        // Act & Assert
        Assert.Null(notification.DoctorId);

        // Act - Set value
        notification.DoctorId = 20;

        // Assert
        Assert.NotNull(notification.DoctorId);
        Assert.Equal(20, notification.DoctorId);
    }

    [Fact]
    public void Notification_IsRead_DefaultValue_ShouldBeFalse()
    {
        // Arrange & Act
        var notification = new Notification();

        // Assert
        Assert.False(notification.IsRead);
    }

    [Fact]
    public void Notification_ModifiedDate_ShouldBeNullable()
    {
        // Arrange
        var notification = new Notification();

        // Act & Assert
        Assert.Null(notification.ModifiedDate);

        // Act - Set value
        var modifiedDate = DateTime.UtcNow;
        notification.ModifiedDate = modifiedDate;

        // Assert
        Assert.NotNull(notification.ModifiedDate);
        Assert.Equal(modifiedDate, notification.ModifiedDate);
    }

    [Fact]
    public void Notification_ModifiedBy_ShouldBeNullable()
    {
        // Arrange
        var notification = new Notification();

        // Act & Assert
        Assert.Null(notification.ModifiedBy);

        // Act - Set value
        notification.ModifiedBy = "UpdatedByAdmin";

        // Assert
        Assert.NotNull(notification.ModifiedBy);
        Assert.Equal("UpdatedByAdmin", notification.ModifiedBy);
    }

    [Fact]
    public void Notification_DefaultType_ShouldBeInfo()
    {
        // Arrange & Act
        var notification = new Notification();

        // Assert
        Assert.Equal("Info", notification.Type);
    }

    [Fact]
    public void Notification_NavigationProperty_Patient_ShouldBeNullable()
    {
        // Arrange
        var notification = new Notification();

        // Act & Assert
        Assert.Null(notification.Patient);

        // Act - Set value
        var patient = new Patient { Id = 1, Name = "John Doe" };
        notification.Patient = patient;

        // Assert
        Assert.NotNull(notification.Patient);
        Assert.Equal(patient, notification.Patient);
    }

    [Fact]
    public void Notification_MarkAsRead_ShouldUpdateIsRead()
    {
        // Arrange
        var notification = new Notification { IsRead = false };

        // Act
        notification.IsRead = true;

        // Assert
        Assert.True(notification.IsRead);
    }
}
