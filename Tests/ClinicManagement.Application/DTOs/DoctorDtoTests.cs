using System;
using Xunit;
using ClinicManagement.Application.DTOs;

namespace ClinicManagement.Application.DTOs.Tests
{
    public class DoctorDtoTests
    {
        [Fact]
        public void DoctorDto_Constructor_InitializesProperties()
        {
            // Arrange & Act
            var dto = new DoctorDto();

            // Assert
            Assert.NotNull(dto);
            Assert.Equal(0, dto.Id);
            Assert.Equal(string.Empty, dto.Name);
            Assert.Equal(string.Empty, dto.Email);
            Assert.Equal(string.Empty, dto.Phone);
            Assert.Equal(string.Empty, dto.Specialization);
            Assert.Equal(string.Empty, dto.Qualification);
            Assert.Equal(0, dto.Experience);
            Assert.Equal(0m, dto.ConsultationFee);
            Assert.False(dto.IsActive);
        }

        [Fact]
        public void DoctorDto_AllProperties_CanBeSetAndRetrieved()
        {
            // Arrange
            var dto = new DoctorDto
            {
                Id = 10,
                Name = "Dr. Smith",
                Email = "smith@hospital.com",
                Phone = "9988776655",
                Specialization = "Cardiology",
                Qualification = "MBBS, MD",
                Experience = 15,
                ConsultationFee = 800.50m,
                ProfileImage = "profile.jpg",
                IsActive = true
            };

            // Assert
            Assert.Equal(10, dto.Id);
            Assert.Equal("Dr. Smith", dto.Name);
            Assert.Equal("smith@hospital.com", dto.Email);
            Assert.Equal("9988776655", dto.Phone);
            Assert.Equal("Cardiology", dto.Specialization);
            Assert.Equal("MBBS, MD", dto.Qualification);
            Assert.Equal(15, dto.Experience);
            Assert.Equal(800.50m, dto.ConsultationFee);
            Assert.Equal("profile.jpg", dto.ProfileImage);
            Assert.True(dto.IsActive);
        }

        [Fact]
        public void DoctorCreateDto_Constructor_InitializesProperties()
        {
            // Arrange & Act
            var dto = new DoctorCreateDto();

            // Assert
            Assert.NotNull(dto);
            Assert.Equal(string.Empty, dto.Name);
            Assert.Equal(string.Empty, dto.Email);
            Assert.Equal(string.Empty, dto.Password);
            Assert.Equal(string.Empty, dto.Phone);
            Assert.Equal(string.Empty, dto.Specialization);
            Assert.Equal(string.Empty, dto.Qualification);
            Assert.Equal(0, dto.Experience);
            Assert.Equal(0m, dto.ConsultationFee);
        }

        [Fact]
        public void DoctorCreateDto_AllProperties_CanBeSetAndRetrieved()
        {
            // Arrange
            var dto = new DoctorCreateDto
            {
                Name = "Dr. Johnson",
                Email = "johnson@clinic.com",
                Password = "DocPass123",
                Phone = "8887776655",
                Specialization = "Neurology",
                Qualification = "MBBS, DM",
                Experience = 20,
                ConsultationFee = 1000.00m
            };

            // Assert
            Assert.Equal("Dr. Johnson", dto.Name);
            Assert.Equal("johnson@clinic.com", dto.Email);
            Assert.Equal("DocPass123", dto.Password);
            Assert.Equal("8887776655", dto.Phone);
            Assert.Equal("Neurology", dto.Specialization);
            Assert.Equal("MBBS, DM", dto.Qualification);
            Assert.Equal(20, dto.Experience);
            Assert.Equal(1000.00m, dto.ConsultationFee);
        }

        [Fact]
        public void DoctorUpdateDto_Constructor_InitializesProperties()
        {
            // Arrange & Act
            var dto = new DoctorUpdateDto();

            // Assert
            Assert.NotNull(dto);
            Assert.Equal(string.Empty, dto.Name);
            Assert.Equal(string.Empty, dto.Email);
            Assert.Equal(string.Empty, dto.Phone);
            Assert.Equal(string.Empty, dto.Specialization);
            Assert.Equal(string.Empty, dto.Qualification);
            Assert.Equal(0, dto.Experience);
            Assert.Equal(0m, dto.ConsultationFee);
        }

        [Fact]
        public void DoctorUpdateDto_AllProperties_CanBeSetAndRetrieved()
        {
            // Arrange
            var dto = new DoctorUpdateDto
            {
                Name = "Dr. Updated",
                Email = "updated@hospital.com",
                Phone = "7778889999",
                Specialization = "Orthopedics",
                Qualification = "MBBS, MS",
                Experience = 10,
                ConsultationFee = 600.00m
            };

            // Assert
            Assert.Equal("Dr. Updated", dto.Name);
            Assert.Equal("updated@hospital.com", dto.Email);
            Assert.Equal("7778889999", dto.Phone);
            Assert.Equal("Orthopedics", dto.Specialization);
            Assert.Equal("MBBS, MS", dto.Qualification);
            Assert.Equal(10, dto.Experience);
            Assert.Equal(600.00m, dto.ConsultationFee);
        }
    }
}
