using Xunit;
using ClinicManagement.Application.DTOs;
using System;

namespace ClinicManagement.Application.Tests.DTOs
{
    public class PatientDtoTests
    {
        [Fact]
        public void PatientDto_Constructor_ShouldInitializeWithDefaults()
        {
            // Act
            var dto = new PatientDto();

            // Assert
            Assert.Equal(0, dto.PatientID);
            Assert.Equal(string.Empty, dto.Name);
            Assert.Equal(string.Empty, dto.Email);
            Assert.Equal(string.Empty, dto.Phone);
            Assert.Equal(string.Empty, dto.Address);
            Assert.Equal(string.Empty, dto.Gender);
        }

        [Fact]
        public void PatientDto_SetProperties_ShouldRetainValues()
        {
            // Arrange
            var dto = new PatientDto();
            var birthDate = new DateTime(1990, 5, 15);

            // Act
            dto.PatientID = 1;
            dto.Name = "John Doe";
            dto.Email = "john@test.com";
            dto.Phone = "1234567890";
            dto.Address = "123 Main St";
            dto.BirthDate = birthDate;
            dto.Gender = "Male";

            // Assert
            Assert.Equal(1, dto.PatientID);
            Assert.Equal("John Doe", dto.Name);
            Assert.Equal("john@test.com", dto.Email);
            Assert.Equal("1234567890", dto.Phone);
            Assert.Equal("123 Main St", dto.Address);
            Assert.Equal(birthDate, dto.BirthDate);
            Assert.Equal("Male", dto.Gender);
        }

        [Fact]
        public void CreatePatientDto_Constructor_ShouldInitializeWithDefaults()
        {
            // Act
            var dto = new CreatePatientDto();

            // Assert
            Assert.Equal(string.Empty, dto.Name);
            Assert.Equal(string.Empty, dto.Email);
            Assert.Equal(string.Empty, dto.Password);
            Assert.Equal(string.Empty, dto.Phone);
            Assert.Equal(string.Empty, dto.Address);
            Assert.Equal(string.Empty, dto.Gender);
        }

        [Fact]
        public void CreatePatientDto_SetProperties_ShouldRetainValues()
        {
            // Arrange
            var dto = new CreatePatientDto();
            var birthDate = new DateTime(1990, 5, 15);

            // Act
            dto.Name = "Jane Doe";
            dto.Email = "jane@test.com";
            dto.Password = "password123";
            dto.Phone = "9876543210";
            dto.Address = "456 Oak St";
            dto.BirthDate = birthDate;
            dto.Gender = "Female";

            // Assert
            Assert.Equal("Jane Doe", dto.Name);
            Assert.Equal("jane@test.com", dto.Email);
            Assert.Equal("password123", dto.Password);
            Assert.Equal("9876543210", dto.Phone);
            Assert.Equal("456 Oak St", dto.Address);
            Assert.Equal(birthDate, dto.BirthDate);
            Assert.Equal("Female", dto.Gender);
        }

        [Fact]
        public void UpdatePatientDto_Constructor_ShouldInitializeWithDefaults()
        {
            // Act
            var dto = new UpdatePatientDto();

            // Assert
            Assert.Equal(0, dto.PatientID);
            Assert.Equal(string.Empty, dto.Name);
            Assert.Equal(string.Empty, dto.Email);
            Assert.Equal(string.Empty, dto.Phone);
            Assert.Equal(string.Empty, dto.Address);
            Assert.Equal(string.Empty, dto.Gender);
        }

        [Fact]
        public void UpdatePatientDto_SetProperties_ShouldRetainValues()
        {
            // Arrange
            var dto = new UpdatePatientDto();
            var birthDate = new DateTime(1990, 5, 15);

            // Act
            dto.PatientID = 1;
            dto.Name = "Updated Name";
            dto.Email = "updated@test.com";
            dto.Phone = "1111111111";
            dto.Address = "789 Elm St";
            dto.BirthDate = birthDate;
            dto.Gender = "Other";

            // Assert
            Assert.Equal(1, dto.PatientID);
            Assert.Equal("Updated Name", dto.Name);
            Assert.Equal("updated@test.com", dto.Email);
            Assert.Equal("1111111111", dto.Phone);
            Assert.Equal("789 Elm St", dto.Address);
            Assert.Equal(birthDate, dto.BirthDate);
            Assert.Equal("Other", dto.Gender);
        }
    }
}
