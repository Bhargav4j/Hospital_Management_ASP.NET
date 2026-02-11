using HospitalManagement.Domain.Entities;
using Xunit;

namespace Tests.HospitalManagement.Domain.Entities;

public class FeedbackTests
{
    [Fact]
    public void Feedback_Constructor_ShouldInitializeDefaultValues()
    {
        // Arrange & Act
        var feedback = new Feedback();

        // Assert
        Assert.Equal(0, feedback.Id);
        Assert.Equal(0, feedback.PatientId);
        Assert.Equal(string.Empty, feedback.Message);
        Assert.Equal(0, feedback.Rating);
        Assert.Equal("System", feedback.CreatedBy);
        Assert.False(feedback.IsActive);
    }

    [Fact]
    public void Feedback_SetProperties_ShouldSetValues()
    {
        // Arrange
        var feedback = new Feedback();
        var createdDate = DateTime.UtcNow;

        // Act
        feedback.Id = 1;
        feedback.PatientId = 10;
        feedback.Message = "Excellent service!";
        feedback.Rating = 5;
        feedback.CreatedDate = createdDate;
        feedback.IsActive = true;

        // Assert
        Assert.Equal(1, feedback.Id);
        Assert.Equal(10, feedback.PatientId);
        Assert.Equal("Excellent service!", feedback.Message);
        Assert.Equal(5, feedback.Rating);
        Assert.Equal(createdDate, feedback.CreatedDate);
        Assert.True(feedback.IsActive);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    public void Feedback_Rating_CanBeSetToValidRatings(int rating)
    {
        // Arrange
        var feedback = new Feedback();

        // Act
        feedback.Rating = rating;

        // Assert
        Assert.Equal(rating, feedback.Rating);
    }

    [Fact]
    public void Feedback_Message_CanBeEmpty()
    {
        // Arrange & Act
        var feedback = new Feedback { Message = string.Empty };

        // Assert
        Assert.Equal(string.Empty, feedback.Message);
    }

    [Fact]
    public void Feedback_ModifiedDate_CanBeNull()
    {
        // Arrange & Act
        var feedback = new Feedback();

        // Assert
        Assert.Null(feedback.ModifiedDate);
    }

    [Fact]
    public void Feedback_ModifiedDate_CanBeSet()
    {
        // Arrange
        var feedback = new Feedback();
        var modifiedDate = DateTime.UtcNow;

        // Act
        feedback.ModifiedDate = modifiedDate;

        // Assert
        Assert.Equal(modifiedDate, feedback.ModifiedDate);
    }

    [Fact]
    public void Feedback_ModifiedBy_CanBeNull()
    {
        // Arrange & Act
        var feedback = new Feedback();

        // Assert
        Assert.Null(feedback.ModifiedBy);
    }

    [Fact]
    public void Feedback_ModifiedBy_CanBeSet()
    {
        // Arrange
        var feedback = new Feedback();

        // Act
        feedback.ModifiedBy = "Admin";

        // Assert
        Assert.Equal("Admin", feedback.ModifiedBy);
    }

    [Fact]
    public void Feedback_Patient_CanBeSet()
    {
        // Arrange
        var feedback = new Feedback();
        var patient = new User { Id = 10, Name = "Patient Name" };

        // Act
        feedback.Patient = patient;

        // Assert
        Assert.NotNull(feedback.Patient);
        Assert.Equal(10, feedback.Patient.Id);
    }

    [Fact]
    public void Feedback_CreatedBy_DefaultsToSystem()
    {
        // Arrange & Act
        var feedback = new Feedback();

        // Assert
        Assert.Equal("System", feedback.CreatedBy);
    }
}
