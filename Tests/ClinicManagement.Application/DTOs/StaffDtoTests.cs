using System;
using Xunit;
using ClinicManagement.Application.DTOs;

namespace ClinicManagement.Application.DTOs.Tests
{
    public class StaffDtoTests
    {
        [Fact]
        public void StaffDto_Constructor_InitializesProperties()
        {
            // Arrange & Act
            var dto = new StaffDto();

            // Assert
            Assert.NotNull(dto);
            Assert.Equal(0, dto.Id);
            Assert.Equal(string.Empty, dto.Name);
            Assert.Equal(string.Empty, dto.Email);
            Assert.Equal(string.Empty, dto.Phone);
            Assert.Equal(string.Empty, dto.Role);
            Assert.False(dto.IsActive);
        }

        [Fact]
        public void StaffDto_AllProperties_CanBeSetAndRetrieved()
        {
            // Arrange
            var dto = new StaffDto
            {
                Id = 700,
                Name = "Sarah Wilson",
                Email = "sarah@clinic.com",
                Phone = "6665554444",
                Role = "Nurse",
                Department = "Emergency",
                IsActive = true
            };

            // Assert
            Assert.Equal(700, dto.Id);
            Assert.Equal("Sarah Wilson", dto.Name);
            Assert.Equal("sarah@clinic.com", dto.Email);
            Assert.Equal("6665554444", dto.Phone);
            Assert.Equal("Nurse", dto.Role);
            Assert.Equal("Emergency", dto.Department);
            Assert.True(dto.IsActive);
        }

        [Fact]
        public void StaffDto_Department_AcceptsNull()
        {
            // Arrange
            var dto = new StaffDto { Department = null };

            // Assert
            Assert.Null(dto.Department);
        }

        [Fact]
        public void StaffCreateDto_Constructor_InitializesProperties()
        {
            // Arrange & Act
            var dto = new StaffCreateDto();

            // Assert
            Assert.NotNull(dto);
            Assert.Equal(string.Empty, dto.Name);
            Assert.Equal(string.Empty, dto.Email);
            Assert.Equal(string.Empty, dto.Password);
            Assert.Equal(string.Empty, dto.Phone);
            Assert.Equal(string.Empty, dto.Role);
        }

        [Fact]
        public void StaffCreateDto_AllProperties_CanBeSetAndRetrieved()
        {
            // Arrange
            var dto = new StaffCreateDto
            {
                Name = "Michael Brown",
                Email = "michael@hospital.com",
                Password = "Staff12345",
                Phone = "5556667777",
                Role = "Receptionist",
                Department = "General"
            };

            // Assert
            Assert.Equal("Michael Brown", dto.Name);
            Assert.Equal("michael@hospital.com", dto.Email);
            Assert.Equal("Staff12345", dto.Password);
            Assert.Equal("5556667777", dto.Phone);
            Assert.Equal("Receptionist", dto.Role);
            Assert.Equal("General", dto.Department);
        }

        [Fact]
        public void StaffCreateDto_Department_AcceptsNull()
        {
            // Arrange
            var dto = new StaffCreateDto { Department = null };

            // Assert
            Assert.Null(dto.Department);
        }

        [Fact]
        public void StaffDto_SetIsActive_StoresTrue()
        {
            // Arrange
            var dto = new StaffDto { IsActive = true };

            // Assert
            Assert.True(dto.IsActive);
        }

        [Fact]
        public void StaffDto_SetIsActive_StoresFalse()
        {
            // Arrange
            var dto = new StaffDto { IsActive = false };

            // Assert
            Assert.False(dto.IsActive);
        }
    }
}
