using System;
using Xunit;
using ClinicManagement.Application.DTOs;

namespace ClinicManagement.Application.DTOs.Tests
{
    public class PatientDtoTests
    {
        [Fact]
        public void PatientDto_Constructor_InitializesProperties()
        {
            // Arrange & Act
            var dto = new PatientDto();

            // Assert
            Assert.NotNull(dto);
            Assert.Equal(0, dto.Id);
            Assert.Equal(string.Empty, dto.Name);
            Assert.Equal(string.Empty, dto.Email);
            Assert.Equal(string.Empty, dto.Phone);
            Assert.Equal(string.Empty, dto.Gender);
            Assert.Equal(string.Empty, dto.Address);
            Assert.False(dto.IsActive);
        }

        [Fact]
        public void PatientDto_AllProperties_CanBeSetAndRetrieved()
        {
            // Arrange
            var dto = new PatientDto
            {
                Id = 1,
                Name = "Test Patient",
                Email = "test@example.com",
                Phone = "1234567890",
                BirthDate = new DateTime(1990, 5, 15),
                Gender = "Male",
                Address = "123 Main St",
                IsActive = true
            };

            // Assert
            Assert.Equal(1, dto.Id);
            Assert.Equal("Test Patient", dto.Name);
            Assert.Equal("test@example.com", dto.Email);
            Assert.Equal("1234567890", dto.Phone);
            Assert.Equal(new DateTime(1990, 5, 15), dto.BirthDate);
            Assert.Equal("Male", dto.Gender);
            Assert.Equal("123 Main St", dto.Address);
            Assert.True(dto.IsActive);
        }

        [Fact]
        public void PatientCreateDto_Constructor_InitializesProperties()
        {
            // Arrange & Act
            var dto = new PatientCreateDto();

            // Assert
            Assert.NotNull(dto);
            Assert.Equal(string.Empty, dto.Name);
            Assert.Equal(string.Empty, dto.Email);
            Assert.Equal(string.Empty, dto.Password);
            Assert.Equal(string.Empty, dto.Phone);
            Assert.Equal(string.Empty, dto.Gender);
            Assert.Equal(string.Empty, dto.Address);
        }

        [Fact]
        public void PatientCreateDto_AllProperties_CanBeSetAndRetrieved()
        {
            // Arrange
            var dto = new PatientCreateDto
            {
                Name = "New Patient",
                Email = "new@example.com",
                Password = "SecurePass123",
                Phone = "9876543210",
                BirthDate = new DateTime(1985, 10, 20),
                Gender = "Female",
                Address = "456 Oak Ave"
            };

            // Assert
            Assert.Equal("New Patient", dto.Name);
            Assert.Equal("new@example.com", dto.Email);
            Assert.Equal("SecurePass123", dto.Password);
            Assert.Equal("9876543210", dto.Phone);
            Assert.Equal(new DateTime(1985, 10, 20), dto.BirthDate);
            Assert.Equal("Female", dto.Gender);
            Assert.Equal("456 Oak Ave", dto.Address);
        }

        [Fact]
        public void PatientUpdateDto_Constructor_InitializesProperties()
        {
            // Arrange & Act
            var dto = new PatientUpdateDto();

            // Assert
            Assert.NotNull(dto);
            Assert.Equal(string.Empty, dto.Name);
            Assert.Equal(string.Empty, dto.Email);
            Assert.Equal(string.Empty, dto.Phone);
            Assert.Equal(string.Empty, dto.Gender);
            Assert.Equal(string.Empty, dto.Address);
        }

        [Fact]
        public void PatientUpdateDto_AllProperties_CanBeSetAndRetrieved()
        {
            // Arrange
            var dto = new PatientUpdateDto
            {
                Name = "Updated Patient",
                Email = "updated@example.com",
                Phone = "5555555555",
                BirthDate = new DateTime(1992, 3, 12),
                Gender = "Male",
                Address = "789 Elm St"
            };

            // Assert
            Assert.Equal("Updated Patient", dto.Name);
            Assert.Equal("updated@example.com", dto.Email);
            Assert.Equal("5555555555", dto.Phone);
            Assert.Equal(new DateTime(1992, 3, 12), dto.BirthDate);
            Assert.Equal("Male", dto.Gender);
            Assert.Equal("789 Elm St", dto.Address);
        }
    }
}
