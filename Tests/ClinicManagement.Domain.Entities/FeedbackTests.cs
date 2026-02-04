using Xunit;
using ClinicManagement.Domain.Entities;
using System;

namespace ClinicManagement.Domain.Entities.Tests;

/// <summary>
/// Test class for Feedback entity
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
        Assert.Equal(string.Empty, feedback.Message);
        Assert.Equal("System", feedback.CreatedBy);
        Assert.False(feedback.IsActive);
    }

    [Fact]
    public void Feedback_SetProperties_ShouldStoreValues()
    {
        // Arrange
        var feedback = new Feedback();
        var expectedId = 1;
        var expectedPatientId = 10;
        var expectedDoctorId = 20;
        var expectedMessage = "Great service!";
        var expectedRating = 5;
        var expectedCreatedDate = DateTime.UtcNow;
        var expectedModifiedDate = DateTime.UtcNow;
        var expectedIsActive = true;
        var expectedCreatedBy = "Patient";
        var expectedModifiedBy = "Patient";

        // Act
        feedback.Id = expectedId;
        feedback.PatientId = expectedPatientId;
        feedback.DoctorId = expectedDoctorId;
        feedback.Message = expectedMessage;
        feedback.Rating = expectedRating;
        feedback.CreatedDate = expectedCreatedDate;
        feedback.ModifiedDate = expectedModifiedDate;
        feedback.IsActive = expectedIsActive;
        feedback.CreatedBy = expectedCreatedBy;
        feedback.ModifiedBy = expectedModifiedBy;

        // Assert
        Assert.Equal(expectedId, feedback.Id);
        Assert.Equal(expectedPatientId, feedback.PatientId);
        Assert.Equal(expectedDoctorId, feedback.DoctorId);
        Assert.Equal(expectedMessage, feedback.Message);
        Assert.Equal(expectedRating, feedback.Rating);
        Assert.Equal(expectedCreatedDate, feedback.CreatedDate);
        Assert.Equal(expectedModifiedDate, feedback.ModifiedDate);
        Assert.Equal(expectedIsActive, feedback.IsActive);
        Assert.Equal(expectedCreatedBy, feedback.CreatedBy);
        Assert.Equal(expectedModifiedBy, feedback.ModifiedBy);
    }

    [Fact]
    public void Feedback_DoctorId_ShouldAcceptNull()
    {
        // Arrange
        var feedback = new Feedback();

        // Act
        feedback.DoctorId = null;

        // Assert
        Assert.Null(feedback.DoctorId);
    }

    [Fact]
    public void Feedback_Rating_ShouldAcceptNull()
    {
        // Arrange
        var feedback = new Feedback();

        // Act
        feedback.Rating = null;

        // Assert
        Assert.Null(feedback.Rating);
    }

    [Fact]
    public void Feedback_ModifiedDate_ShouldAcceptNull()
    {
        // Arrange
        var feedback = new Feedback();

        // Act
        feedback.ModifiedDate = null;

        // Assert
        Assert.Null(feedback.ModifiedDate);
    }

    [Fact]
    public void Feedback_ModifiedBy_ShouldAcceptNull()
    {
        // Arrange
        var feedback = new Feedback();

        // Act
        feedback.ModifiedBy = null;

        // Assert
        Assert.Null(feedback.ModifiedBy);
    }

    [Fact]
    public void Feedback_Patient_ShouldStorePatientReference()
    {
        // Arrange
        var feedback = new Feedback();
        var patient = new Patient { Id = 1, Name = "John Doe" };

        // Act
        feedback.Patient = patient;

        // Assert
        Assert.NotNull(feedback.Patient);
        Assert.Equal(patient.Id, feedback.Patient.Id);
        Assert.Equal(patient.Name, feedback.Patient.Name);
    }

    [Fact]
    public void Feedback_IsActive_ShouldToggleBetweenTrueAndFalse()
    {
        // Arrange
        var feedback = new Feedback();

        // Act & Assert
        feedback.IsActive = true;
        Assert.True(feedback.IsActive);

        feedback.IsActive = false;
        Assert.False(feedback.IsActive);
    }

    [Fact]
    public void Feedback_Message_ShouldAcceptEmptyString()
    {
        // Arrange
        var feedback = new Feedback();

        // Act
        feedback.Message = string.Empty;

        // Assert
        Assert.Equal(string.Empty, feedback.Message);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    public void Feedback_Rating_ShouldStoreRatingValue(int rating)
    {
        // Arrange
        var feedback = new Feedback();

        // Act
        feedback.Rating = rating;

        // Assert
        Assert.Equal(rating, feedback.Rating);
    }

    [Fact]
    public void Feedback_Message_ShouldStoreLongString()
    {
        // Arrange
        var feedback = new Feedback();
        var longMessage = new string('A', 500);

        // Act
        feedback.Message = longMessage;

        // Assert
        Assert.Equal(longMessage, feedback.Message);
    }
}
