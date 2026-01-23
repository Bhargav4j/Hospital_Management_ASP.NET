using Xunit;
using ClinicManagement.Domain.Entities;
using System;

namespace ClinicManagement.Domain.Entities.Tests;

/// <summary>
/// Unit tests for Feedback entity
/// </summary>
public class FeedbackTests
{
    [Fact]
    public void Feedback_Constructor_ShouldInitializeWithDefaultValues()
    {
        // Arrange & Act
        var feedback = new Feedback();

        // Assert
        Assert.Equal(0, feedback.Id);
        Assert.Equal(0, feedback.PatientId);
        Assert.Null(feedback.DoctorId);
        Assert.Equal(string.Empty, feedback.Subject);
        Assert.Equal(string.Empty, feedback.Message);
        Assert.Null(feedback.Rating);
        Assert.Equal("Pending", feedback.Status);
        Assert.Equal("System", feedback.CreatedBy);
        Assert.False(feedback.IsActive);
    }

    [Fact]
    public void Feedback_SetProperties_ShouldStoreCorrectValues()
    {
        // Arrange
        var feedback = new Feedback();
        var createdDate = DateTime.UtcNow;

        // Act
        feedback.Id = 1;
        feedback.PatientId = 5;
        feedback.DoctorId = 3;
        feedback.Subject = "Excellent Service";
        feedback.Message = "The doctor was very helpful and professional";
        feedback.Rating = 5;
        feedback.Status = "Reviewed";
        feedback.CreatedDate = createdDate;
        feedback.IsActive = true;
        feedback.CreatedBy = "Patient";

        // Assert
        Assert.Equal(1, feedback.Id);
        Assert.Equal(5, feedback.PatientId);
        Assert.Equal(3, feedback.DoctorId);
        Assert.Equal("Excellent Service", feedback.Subject);
        Assert.Equal("The doctor was very helpful and professional", feedback.Message);
        Assert.Equal(5, feedback.Rating);
        Assert.Equal("Reviewed", feedback.Status);
        Assert.Equal(createdDate, feedback.CreatedDate);
        Assert.True(feedback.IsActive);
        Assert.Equal("Patient", feedback.CreatedBy);
    }

    [Theory]
    [InlineData("Pending")]
    [InlineData("Reviewed")]
    [InlineData("Resolved")]
    public void Feedback_Status_ShouldAcceptValidValues(string status)
    {
        // Arrange
        var feedback = new Feedback();

        // Act
        feedback.Status = status;

        // Assert
        Assert.Equal(status, feedback.Status);
    }

    [Fact]
    public void Feedback_DoctorId_ShouldBeNullable()
    {
        // Arrange
        var feedback = new Feedback();

        // Act & Assert
        Assert.Null(feedback.DoctorId);

        // Act - Set value
        feedback.DoctorId = 10;

        // Assert
        Assert.NotNull(feedback.DoctorId);
        Assert.Equal(10, feedback.DoctorId);
    }

    [Fact]
    public void Feedback_Rating_ShouldBeNullable()
    {
        // Arrange
        var feedback = new Feedback();

        // Act & Assert
        Assert.Null(feedback.Rating);

        // Act - Set value
        feedback.Rating = 4;

        // Assert
        Assert.NotNull(feedback.Rating);
        Assert.Equal(4, feedback.Rating);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    public void Feedback_Rating_ShouldAcceptValidRatings(int rating)
    {
        // Arrange
        var feedback = new Feedback();

        // Act
        feedback.Rating = rating;

        // Assert
        Assert.Equal(rating, feedback.Rating);
    }

    [Fact]
    public void Feedback_ModifiedDate_ShouldBeNullable()
    {
        // Arrange
        var feedback = new Feedback();

        // Act & Assert
        Assert.Null(feedback.ModifiedDate);

        // Act - Set value
        var modifiedDate = DateTime.UtcNow;
        feedback.ModifiedDate = modifiedDate;

        // Assert
        Assert.NotNull(feedback.ModifiedDate);
        Assert.Equal(modifiedDate, feedback.ModifiedDate);
    }

    [Fact]
    public void Feedback_ModifiedBy_ShouldBeNullable()
    {
        // Arrange
        var feedback = new Feedback();

        // Act & Assert
        Assert.Null(feedback.ModifiedBy);

        // Act - Set value
        feedback.ModifiedBy = "UpdatedByAdmin";

        // Assert
        Assert.NotNull(feedback.ModifiedBy);
        Assert.Equal("UpdatedByAdmin", feedback.ModifiedBy);
    }

    [Fact]
    public void Feedback_DefaultStatus_ShouldBePending()
    {
        // Arrange & Act
        var feedback = new Feedback();

        // Assert
        Assert.Equal("Pending", feedback.Status);
    }

    [Fact]
    public void Feedback_NavigationProperty_Patient_ShouldBeInitializable()
    {
        // Arrange
        var feedback = new Feedback();
        var patient = new Patient { Id = 1, Name = "John Doe" };

        // Act
        feedback.Patient = patient;

        // Assert
        Assert.NotNull(feedback.Patient);
        Assert.Equal(patient, feedback.Patient);
    }

    [Fact]
    public void Feedback_SubjectAndMessage_ShouldAcceptLongText()
    {
        // Arrange
        var feedback = new Feedback();
        var longSubject = new string('A', 200);
        var longMessage = new string('B', 1000);

        // Act
        feedback.Subject = longSubject;
        feedback.Message = longMessage;

        // Assert
        Assert.Equal(longSubject, feedback.Subject);
        Assert.Equal(longMessage, feedback.Message);
    }
}
