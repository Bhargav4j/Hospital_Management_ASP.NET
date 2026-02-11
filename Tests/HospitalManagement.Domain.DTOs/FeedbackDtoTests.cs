using HospitalManagement.Domain.DTOs;
using Xunit;

namespace Tests.HospitalManagement.Domain.DTOs;

public class FeedbackDtoTests
{
    [Fact]
    public void FeedbackDto_Constructor_ShouldInitializeDefaultValues()
    {
        // Arrange & Act
        var feedbackDto = new FeedbackDto();

        // Assert
        Assert.Equal(0, feedbackDto.Id);
        Assert.Equal(0, feedbackDto.PatientId);
        Assert.Equal(string.Empty, feedbackDto.PatientName);
        Assert.Equal(string.Empty, feedbackDto.Message);
        Assert.Equal(0, feedbackDto.Rating);
    }

    [Fact]
    public void FeedbackDto_SetProperties_ShouldSetValues()
    {
        // Arrange
        var feedbackDto = new FeedbackDto();
        var createdDate = DateTime.UtcNow;

        // Act
        feedbackDto.Id = 1;
        feedbackDto.PatientId = 10;
        feedbackDto.PatientName = "John Doe";
        feedbackDto.Message = "Great service!";
        feedbackDto.Rating = 5;
        feedbackDto.CreatedDate = createdDate;

        // Assert
        Assert.Equal(1, feedbackDto.Id);
        Assert.Equal(10, feedbackDto.PatientId);
        Assert.Equal("John Doe", feedbackDto.PatientName);
        Assert.Equal("Great service!", feedbackDto.Message);
        Assert.Equal(5, feedbackDto.Rating);
        Assert.Equal(createdDate, feedbackDto.CreatedDate);
    }
}

public class FeedbackCreateDtoTests
{
    [Fact]
    public void FeedbackCreateDto_Constructor_ShouldInitializeDefaultValues()
    {
        // Arrange & Act
        var feedbackCreateDto = new FeedbackCreateDto();

        // Assert
        Assert.Equal(0, feedbackCreateDto.PatientId);
        Assert.Equal(string.Empty, feedbackCreateDto.Message);
        Assert.Equal(0, feedbackCreateDto.Rating);
    }

    [Fact]
    public void FeedbackCreateDto_SetProperties_ShouldSetValues()
    {
        // Arrange
        var feedbackCreateDto = new FeedbackCreateDto();

        // Act
        feedbackCreateDto.PatientId = 10;
        feedbackCreateDto.Message = "Excellent!";
        feedbackCreateDto.Rating = 5;

        // Assert
        Assert.Equal(10, feedbackCreateDto.PatientId);
        Assert.Equal("Excellent!", feedbackCreateDto.Message);
        Assert.Equal(5, feedbackCreateDto.Rating);
    }
}

public class FeedbackUpdateDtoTests
{
    [Fact]
    public void FeedbackUpdateDto_Constructor_ShouldInitializeDefaultValues()
    {
        // Arrange & Act
        var feedbackUpdateDto = new FeedbackUpdateDto();

        // Assert
        Assert.Equal(string.Empty, feedbackUpdateDto.Message);
        Assert.Equal(0, feedbackUpdateDto.Rating);
    }

    [Fact]
    public void FeedbackUpdateDto_SetProperties_ShouldSetValues()
    {
        // Arrange
        var feedbackUpdateDto = new FeedbackUpdateDto();

        // Act
        feedbackUpdateDto.Message = "Updated feedback";
        feedbackUpdateDto.Rating = 4;

        // Assert
        Assert.Equal("Updated feedback", feedbackUpdateDto.Message);
        Assert.Equal(4, feedbackUpdateDto.Rating);
    }
}
