using System;
using Xunit;
using ClinicManagement.Application.DTOs;

namespace ClinicManagement.Application.DTOs.Tests
{
    public class FeedbackDtoTests
    {
        [Fact]
        public void FeedbackDto_Constructor_InitializesProperties()
        {
            // Arrange & Act
            var dto = new FeedbackDto();

            // Assert
            Assert.NotNull(dto);
            Assert.Equal(0, dto.Id);
            Assert.Equal(0, dto.PatientId);
            Assert.Equal(string.Empty, dto.PatientName);
            Assert.Equal(string.Empty, dto.Subject);
            Assert.Equal(string.Empty, dto.Message);
            Assert.Equal(0, dto.Rating);
        }

        [Fact]
        public void FeedbackDto_AllProperties_CanBeSetAndRetrieved()
        {
            // Arrange
            var dto = new FeedbackDto
            {
                Id = 500,
                PatientId = 250,
                PatientName = "Robert Smith",
                DoctorId = 125,
                DoctorName = "Dr. Lee",
                Subject = "Excellent care",
                Message = "Very professional and caring",
                Rating = 5,
                CreatedDate = new DateTime(2026, 1, 25)
            };

            // Assert
            Assert.Equal(500, dto.Id);
            Assert.Equal(250, dto.PatientId);
            Assert.Equal("Robert Smith", dto.PatientName);
            Assert.Equal(125, dto.DoctorId);
            Assert.Equal("Dr. Lee", dto.DoctorName);
            Assert.Equal("Excellent care", dto.Subject);
            Assert.Equal("Very professional and caring", dto.Message);
            Assert.Equal(5, dto.Rating);
            Assert.Equal(new DateTime(2026, 1, 25), dto.CreatedDate);
        }

        [Fact]
        public void FeedbackDto_DoctorId_AcceptsNull()
        {
            // Arrange
            var dto = new FeedbackDto { DoctorId = null };

            // Assert
            Assert.Null(dto.DoctorId);
        }

        [Fact]
        public void FeedbackDto_DoctorName_AcceptsNull()
        {
            // Arrange
            var dto = new FeedbackDto { DoctorName = null };

            // Assert
            Assert.Null(dto.DoctorName);
        }

        [Fact]
        public void FeedbackCreateDto_Constructor_InitializesProperties()
        {
            // Arrange & Act
            var dto = new FeedbackCreateDto();

            // Assert
            Assert.NotNull(dto);
            Assert.Equal(0, dto.PatientId);
            Assert.Equal(string.Empty, dto.Subject);
            Assert.Equal(string.Empty, dto.Message);
            Assert.Equal(0, dto.Rating);
        }

        [Fact]
        public void FeedbackCreateDto_AllProperties_CanBeSetAndRetrieved()
        {
            // Arrange
            var dto = new FeedbackCreateDto
            {
                PatientId = 350,
                DoctorId = 175,
                Subject = "Great service",
                Message = "Very satisfied with treatment",
                Rating = 4
            };

            // Assert
            Assert.Equal(350, dto.PatientId);
            Assert.Equal(175, dto.DoctorId);
            Assert.Equal("Great service", dto.Subject);
            Assert.Equal("Very satisfied with treatment", dto.Message);
            Assert.Equal(4, dto.Rating);
        }

        [Fact]
        public void FeedbackCreateDto_DoctorId_AcceptsNull()
        {
            // Arrange
            var dto = new FeedbackCreateDto { DoctorId = null };

            // Assert
            Assert.Null(dto.DoctorId);
        }

        [Fact]
        public void FeedbackCreateDto_Rating_StoresZero()
        {
            // Arrange
            var dto = new FeedbackCreateDto { Rating = 0 };

            // Assert
            Assert.Equal(0, dto.Rating);
        }

        [Fact]
        public void FeedbackCreateDto_Rating_StoresNegativeValue()
        {
            // Arrange
            var dto = new FeedbackCreateDto { Rating = -1 };

            // Assert
            Assert.Equal(-1, dto.Rating);
        }

        [Fact]
        public void FeedbackCreateDto_Rating_StoresMaxValue()
        {
            // Arrange
            var dto = new FeedbackCreateDto { Rating = 5 };

            // Assert
            Assert.Equal(5, dto.Rating);
        }
    }
}
