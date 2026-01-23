using System;
using Xunit;
using ClinicManagement.Application.DTOs;

namespace ClinicManagement.Application.DTOs.Tests
{
    public class BillDtoTests
    {
        [Fact]
        public void BillDto_Constructor_InitializesProperties()
        {
            // Arrange & Act
            var dto = new BillDto();

            // Assert
            Assert.NotNull(dto);
            Assert.Equal(0, dto.Id);
            Assert.Equal(0, dto.PatientId);
            Assert.Equal(string.Empty, dto.PatientName);
            Assert.Equal(0m, dto.Amount);
            Assert.Equal(string.Empty, dto.Status);
        }

        [Fact]
        public void BillDto_AllProperties_CanBeSetAndRetrieved()
        {
            // Arrange
            var dto = new BillDto
            {
                Id = 200,
                PatientId = 150,
                DoctorId = 75,
                PatientName = "Jane Doe",
                DoctorName = "Dr. Johnson",
                Amount = 1500.50m,
                BillDate = new DateTime(2026, 1, 20),
                Status = "Paid",
                Description = "Consultation and tests"
            };

            // Assert
            Assert.Equal(200, dto.Id);
            Assert.Equal(150, dto.PatientId);
            Assert.Equal(75, dto.DoctorId);
            Assert.Equal("Jane Doe", dto.PatientName);
            Assert.Equal("Dr. Johnson", dto.DoctorName);
            Assert.Equal(1500.50m, dto.Amount);
            Assert.Equal(new DateTime(2026, 1, 20), dto.BillDate);
            Assert.Equal("Paid", dto.Status);
            Assert.Equal("Consultation and tests", dto.Description);
        }

        [Fact]
        public void BillDto_DoctorId_AcceptsNull()
        {
            // Arrange
            var dto = new BillDto { DoctorId = null };

            // Assert
            Assert.Null(dto.DoctorId);
        }

        [Fact]
        public void BillCreateDto_Constructor_InitializesProperties()
        {
            // Arrange & Act
            var dto = new BillCreateDto();

            // Assert
            Assert.NotNull(dto);
            Assert.Equal(0, dto.PatientId);
            Assert.Equal(0m, dto.Amount);
        }

        [Fact]
        public void BillCreateDto_AllProperties_CanBeSetAndRetrieved()
        {
            // Arrange
            var dto = new BillCreateDto
            {
                PatientId = 300,
                DoctorId = 125,
                Amount = 2500.75m,
                Description = "Emergency surgery"
            };

            // Assert
            Assert.Equal(300, dto.PatientId);
            Assert.Equal(125, dto.DoctorId);
            Assert.Equal(2500.75m, dto.Amount);
            Assert.Equal("Emergency surgery", dto.Description);
        }

        [Fact]
        public void BillCreateDto_DoctorId_AcceptsNull()
        {
            // Arrange
            var dto = new BillCreateDto { DoctorId = null };

            // Assert
            Assert.Null(dto.DoctorId);
        }

        [Fact]
        public void BillUpdateDto_Constructor_InitializesProperties()
        {
            // Arrange & Act
            var dto = new BillUpdateDto();

            // Assert
            Assert.NotNull(dto);
            Assert.Equal(0m, dto.Amount);
            Assert.Equal(string.Empty, dto.Status);
        }

        [Fact]
        public void BillUpdateDto_AllProperties_CanBeSetAndRetrieved()
        {
            // Arrange
            var dto = new BillUpdateDto
            {
                Amount = 1800.00m,
                Status = "Pending",
                Description = "Updated amount after review"
            };

            // Assert
            Assert.Equal(1800.00m, dto.Amount);
            Assert.Equal("Pending", dto.Status);
            Assert.Equal("Updated amount after review", dto.Description);
        }
    }
}
