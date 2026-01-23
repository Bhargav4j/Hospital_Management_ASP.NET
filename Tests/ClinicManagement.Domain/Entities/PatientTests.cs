using System;
using System.Collections.Generic;
using Xunit;
using ClinicManagement.Domain.Entities;

namespace ClinicManagement.Domain.Entities.Tests
{
    public class PatientTests
    {
        [Fact]
        public void Patient_Constructor_InitializesProperties()
        {
            // Arrange & Act
            var patient = new Patient();

            // Assert
            Assert.NotNull(patient);
            Assert.Equal(0, patient.Id);
            Assert.Equal(string.Empty, patient.Name);
            Assert.Equal(string.Empty, patient.Email);
            Assert.Equal(string.Empty, patient.Password);
            Assert.Equal(string.Empty, patient.Phone);
            Assert.Equal(string.Empty, patient.Gender);
            Assert.Equal(string.Empty, patient.Address);
            Assert.Equal(string.Empty, patient.CreatedBy);
            Assert.False(patient.IsActive);
        }

        [Fact]
        public void Patient_SetId_StoresValue()
        {
            // Arrange
            var patient = new Patient();
            int expectedId = 123;

            // Act
            patient.Id = expectedId;

            // Assert
            Assert.Equal(expectedId, patient.Id);
        }

        [Fact]
        public void Patient_SetName_StoresValue()
        {
            // Arrange
            var patient = new Patient();
            string expectedName = "John Doe";

            // Act
            patient.Name = expectedName;

            // Assert
            Assert.Equal(expectedName, patient.Name);
        }

        [Fact]
        public void Patient_SetEmail_StoresValue()
        {
            // Arrange
            var patient = new Patient();
            string expectedEmail = "john@example.com";

            // Act
            patient.Email = expectedEmail;

            // Assert
            Assert.Equal(expectedEmail, patient.Email);
        }

        [Fact]
        public void Patient_SetPassword_StoresValue()
        {
            // Arrange
            var patient = new Patient();
            string expectedPassword = "SecurePass123";

            // Act
            patient.Password = expectedPassword;

            // Assert
            Assert.Equal(expectedPassword, patient.Password);
        }

        [Fact]
        public void Patient_SetPhone_StoresValue()
        {
            // Arrange
            var patient = new Patient();
            string expectedPhone = "1234567890";

            // Act
            patient.Phone = expectedPhone;

            // Assert
            Assert.Equal(expectedPhone, patient.Phone);
        }

        [Fact]
        public void Patient_SetBirthDate_StoresValue()
        {
            // Arrange
            var patient = new Patient();
            DateTime expectedBirthDate = new DateTime(1990, 5, 15);

            // Act
            patient.BirthDate = expectedBirthDate;

            // Assert
            Assert.Equal(expectedBirthDate, patient.BirthDate);
        }

        [Fact]
        public void Patient_SetGender_StoresValue()
        {
            // Arrange
            var patient = new Patient();
            string expectedGender = "Male";

            // Act
            patient.Gender = expectedGender;

            // Assert
            Assert.Equal(expectedGender, patient.Gender);
        }

        [Fact]
        public void Patient_SetAddress_StoresValue()
        {
            // Arrange
            var patient = new Patient();
            string expectedAddress = "123 Main Street";

            // Act
            patient.Address = expectedAddress;

            // Assert
            Assert.Equal(expectedAddress, patient.Address);
        }

        [Fact]
        public void Patient_SetCreatedDate_StoresValue()
        {
            // Arrange
            var patient = new Patient();
            DateTime expectedDate = DateTime.UtcNow;

            // Act
            patient.CreatedDate = expectedDate;

            // Assert
            Assert.Equal(expectedDate, patient.CreatedDate);
        }

        [Fact]
        public void Patient_SetModifiedDate_StoresValue()
        {
            // Arrange
            var patient = new Patient();
            DateTime? expectedDate = DateTime.UtcNow;

            // Act
            patient.ModifiedDate = expectedDate;

            // Assert
            Assert.Equal(expectedDate, patient.ModifiedDate);
        }

        [Fact]
        public void Patient_SetModifiedDate_AcceptsNull()
        {
            // Arrange
            var patient = new Patient();

            // Act
            patient.ModifiedDate = null;

            // Assert
            Assert.Null(patient.ModifiedDate);
        }

        [Fact]
        public void Patient_SetIsActive_StoresValue()
        {
            // Arrange
            var patient = new Patient();

            // Act
            patient.IsActive = true;

            // Assert
            Assert.True(patient.IsActive);
        }

        [Fact]
        public void Patient_SetCreatedBy_StoresValue()
        {
            // Arrange
            var patient = new Patient();
            string expectedCreatedBy = "Admin";

            // Act
            patient.CreatedBy = expectedCreatedBy;

            // Assert
            Assert.Equal(expectedCreatedBy, patient.CreatedBy);
        }

        [Fact]
        public void Patient_SetModifiedBy_StoresValue()
        {
            // Arrange
            var patient = new Patient();
            string expectedModifiedBy = "SuperAdmin";

            // Act
            patient.ModifiedBy = expectedModifiedBy;

            // Assert
            Assert.Equal(expectedModifiedBy, patient.ModifiedBy);
        }

        [Fact]
        public void Patient_SetModifiedBy_AcceptsNull()
        {
            // Arrange
            var patient = new Patient();

            // Act
            patient.ModifiedBy = null;

            // Assert
            Assert.Null(patient.ModifiedBy);
        }

        [Fact]
        public void Patient_Appointments_InitializesAsEmptyList()
        {
            // Arrange & Act
            var patient = new Patient();

            // Assert
            Assert.NotNull(patient.Appointments);
            Assert.Empty(patient.Appointments);
        }

        [Fact]
        public void Patient_Bills_InitializesAsEmptyList()
        {
            // Arrange & Act
            var patient = new Patient();

            // Assert
            Assert.NotNull(patient.Bills);
            Assert.Empty(patient.Bills);
        }

        [Fact]
        public void Patient_TreatmentHistories_InitializesAsEmptyList()
        {
            // Arrange & Act
            var patient = new Patient();

            // Assert
            Assert.NotNull(patient.TreatmentHistories);
            Assert.Empty(patient.TreatmentHistories);
        }

        [Fact]
        public void Patient_Feedbacks_InitializesAsEmptyList()
        {
            // Arrange & Act
            var patient = new Patient();

            // Assert
            Assert.NotNull(patient.Feedbacks);
            Assert.Empty(patient.Feedbacks);
        }

        [Fact]
        public void Patient_AllProperties_CanBeSetAndRetrieved()
        {
            // Arrange
            var patient = new Patient
            {
                Id = 1,
                Name = "Jane Smith",
                Email = "jane@test.com",
                Password = "Pass123",
                Phone = "9876543210",
                BirthDate = new DateTime(1985, 10, 20),
                Gender = "Female",
                Address = "456 Oak Avenue",
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = DateTime.UtcNow,
                IsActive = true,
                CreatedBy = "System",
                ModifiedBy = "Admin"
            };

            // Assert
            Assert.Equal(1, patient.Id);
            Assert.Equal("Jane Smith", patient.Name);
            Assert.Equal("jane@test.com", patient.Email);
            Assert.Equal("Pass123", patient.Password);
            Assert.Equal("9876543210", patient.Phone);
            Assert.Equal(new DateTime(1985, 10, 20), patient.BirthDate);
            Assert.Equal("Female", patient.Gender);
            Assert.Equal("456 Oak Avenue", patient.Address);
            Assert.True(patient.IsActive);
            Assert.Equal("System", patient.CreatedBy);
            Assert.Equal("Admin", patient.ModifiedBy);
        }
    }
}
