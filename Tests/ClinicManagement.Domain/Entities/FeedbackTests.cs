using System;
using Xunit;
using ClinicManagement.Domain.Entities;

namespace ClinicManagement.Domain.Entities.Tests
{
    public class FeedbackTests
    {
        [Fact]
        public void Feedback_Constructor_InitializesProperties()
        {
            // Arrange & Act
            var feedback = new Feedback();

            // Assert
            Assert.NotNull(feedback);
            Assert.Equal(0, feedback.Id);
            Assert.Equal(0, feedback.PatientId);
            Assert.Equal(string.Empty, feedback.Subject);
            Assert.Equal(string.Empty, feedback.Message);
            Assert.Equal(0, feedback.Rating);
            Assert.Equal(string.Empty, feedback.CreatedBy);
            Assert.False(feedback.IsActive);
        }

        [Fact]
        public void Feedback_SetId_StoresValue()
        {
            // Arrange
            var feedback = new Feedback();
            int expectedId = 501;

            // Act
            feedback.Id = expectedId;

            // Assert
            Assert.Equal(expectedId, feedback.Id);
        }

        [Fact]
        public void Feedback_SetPatientId_StoresValue()
        {
            // Arrange
            var feedback = new Feedback();
            int expectedPatientId = 250;

            // Act
            feedback.PatientId = expectedPatientId;

            // Assert
            Assert.Equal(expectedPatientId, feedback.PatientId);
        }

        [Fact]
        public void Feedback_SetDoctorId_StoresValue()
        {
            // Arrange
            var feedback = new Feedback();
            int? expectedDoctorId = 125;

            // Act
            feedback.DoctorId = expectedDoctorId;

            // Assert
            Assert.Equal(expectedDoctorId, feedback.DoctorId);
        }

        [Fact]
        public void Feedback_SetDoctorId_AcceptsNull()
        {
            // Arrange
            var feedback = new Feedback();

            // Act
            feedback.DoctorId = null;

            // Assert
            Assert.Null(feedback.DoctorId);
        }

        [Fact]
        public void Feedback_SetSubject_StoresValue()
        {
            // Arrange
            var feedback = new Feedback();
            string expectedSubject = "Excellent service";

            // Act
            feedback.Subject = expectedSubject;

            // Assert
            Assert.Equal(expectedSubject, feedback.Subject);
        }

        [Fact]
        public void Feedback_SetMessage_StoresValue()
        {
            // Arrange
            var feedback = new Feedback();
            string expectedMessage = "The doctor was very professional and caring";

            // Act
            feedback.Message = expectedMessage;

            // Assert
            Assert.Equal(expectedMessage, feedback.Message);
        }

        [Fact]
        public void Feedback_SetRating_StoresValue()
        {
            // Arrange
            var feedback = new Feedback();
            int expectedRating = 5;

            // Act
            feedback.Rating = expectedRating;

            // Assert
            Assert.Equal(expectedRating, feedback.Rating);
        }

        [Fact]
        public void Feedback_SetRating_StoresZero()
        {
            // Arrange
            var feedback = new Feedback();

            // Act
            feedback.Rating = 0;

            // Assert
            Assert.Equal(0, feedback.Rating);
        }

        [Fact]
        public void Feedback_SetRating_StoresNegativeValue()
        {
            // Arrange
            var feedback = new Feedback();

            // Act
            feedback.Rating = -1;

            // Assert
            Assert.Equal(-1, feedback.Rating);
        }

        [Fact]
        public void Feedback_SetCreatedDate_StoresValue()
        {
            // Arrange
            var feedback = new Feedback();
            DateTime expectedDate = DateTime.UtcNow;

            // Act
            feedback.CreatedDate = expectedDate;

            // Assert
            Assert.Equal(expectedDate, feedback.CreatedDate);
        }

        [Fact]
        public void Feedback_SetModifiedDate_StoresValue()
        {
            // Arrange
            var feedback = new Feedback();
            DateTime? expectedDate = DateTime.UtcNow;

            // Act
            feedback.ModifiedDate = expectedDate;

            // Assert
            Assert.Equal(expectedDate, feedback.ModifiedDate);
        }

        [Fact]
        public void Feedback_SetModifiedDate_AcceptsNull()
        {
            // Arrange
            var feedback = new Feedback();

            // Act
            feedback.ModifiedDate = null;

            // Assert
            Assert.Null(feedback.ModifiedDate);
        }

        [Fact]
        public void Feedback_SetIsActive_StoresTrue()
        {
            // Arrange
            var feedback = new Feedback();

            // Act
            feedback.IsActive = true;

            // Assert
            Assert.True(feedback.IsActive);
        }

        [Fact]
        public void Feedback_SetIsActive_StoresFalse()
        {
            // Arrange
            var feedback = new Feedback();

            // Act
            feedback.IsActive = false;

            // Assert
            Assert.False(feedback.IsActive);
        }

        [Fact]
        public void Feedback_SetCreatedBy_StoresValue()
        {
            // Arrange
            var feedback = new Feedback();
            string expectedCreatedBy = "PatientPortal";

            // Act
            feedback.CreatedBy = expectedCreatedBy;

            // Assert
            Assert.Equal(expectedCreatedBy, feedback.CreatedBy);
        }

        [Fact]
        public void Feedback_SetModifiedBy_StoresValue()
        {
            // Arrange
            var feedback = new Feedback();
            string expectedModifiedBy = "Support";

            // Act
            feedback.ModifiedBy = expectedModifiedBy;

            // Assert
            Assert.Equal(expectedModifiedBy, feedback.ModifiedBy);
        }

        [Fact]
        public void Feedback_SetModifiedBy_AcceptsNull()
        {
            // Arrange
            var feedback = new Feedback();

            // Act
            feedback.ModifiedBy = null;

            // Assert
            Assert.Null(feedback.ModifiedBy);
        }

        [Fact]
        public void Feedback_AllProperties_CanBeSetAndRetrieved()
        {
            // Arrange
            var feedback = new Feedback
            {
                Id = 666,
                PatientId = 444,
                DoctorId = 222,
                Subject = "Great experience",
                Message = "Very satisfied with the treatment and care",
                Rating = 5,
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = DateTime.UtcNow,
                IsActive = true,
                CreatedBy = "Patient",
                ModifiedBy = "Admin"
            };

            // Assert
            Assert.Equal(666, feedback.Id);
            Assert.Equal(444, feedback.PatientId);
            Assert.Equal(222, feedback.DoctorId);
            Assert.Equal("Great experience", feedback.Subject);
            Assert.Equal("Very satisfied with the treatment and care", feedback.Message);
            Assert.Equal(5, feedback.Rating);
            Assert.True(feedback.IsActive);
            Assert.Equal("Patient", feedback.CreatedBy);
            Assert.Equal("Admin", feedback.ModifiedBy);
        }
    }
}
