using System;
using System.Collections.Generic;
using Xunit;
using ClinicManagement.Domain.Entities;

namespace ClinicManagement.Domain.Entities.Tests
{
    public class DoctorTests
    {
        [Fact]
        public void Doctor_Constructor_InitializesProperties()
        {
            // Arrange & Act
            var doctor = new Doctor();

            // Assert
            Assert.NotNull(doctor);
            Assert.Equal(0, doctor.Id);
            Assert.Equal(string.Empty, doctor.Name);
            Assert.Equal(string.Empty, doctor.Email);
            Assert.Equal(string.Empty, doctor.Password);
            Assert.Equal(string.Empty, doctor.Phone);
            Assert.Equal(string.Empty, doctor.Specialization);
            Assert.Equal(string.Empty, doctor.Qualification);
            Assert.Equal(0, doctor.Experience);
            Assert.Equal(0m, doctor.ConsultationFee);
            Assert.Equal(string.Empty, doctor.CreatedBy);
            Assert.False(doctor.IsActive);
        }

        [Fact]
        public void Doctor_SetId_StoresValue()
        {
            // Arrange
            var doctor = new Doctor();
            int expectedId = 456;

            // Act
            doctor.Id = expectedId;

            // Assert
            Assert.Equal(expectedId, doctor.Id);
        }

        [Fact]
        public void Doctor_SetName_StoresValue()
        {
            // Arrange
            var doctor = new Doctor();
            string expectedName = "Dr. Smith";

            // Act
            doctor.Name = expectedName;

            // Assert
            Assert.Equal(expectedName, doctor.Name);
        }

        [Fact]
        public void Doctor_SetEmail_StoresValue()
        {
            // Arrange
            var doctor = new Doctor();
            string expectedEmail = "smith@hospital.com";

            // Act
            doctor.Email = expectedEmail;

            // Assert
            Assert.Equal(expectedEmail, doctor.Email);
        }

        [Fact]
        public void Doctor_SetPassword_StoresValue()
        {
            // Arrange
            var doctor = new Doctor();
            string expectedPassword = "DoctorPass123";

            // Act
            doctor.Password = expectedPassword;

            // Assert
            Assert.Equal(expectedPassword, doctor.Password);
        }

        [Fact]
        public void Doctor_SetPhone_StoresValue()
        {
            // Arrange
            var doctor = new Doctor();
            string expectedPhone = "9988776655";

            // Act
            doctor.Phone = expectedPhone;

            // Assert
            Assert.Equal(expectedPhone, doctor.Phone);
        }

        [Fact]
        public void Doctor_SetSpecialization_StoresValue()
        {
            // Arrange
            var doctor = new Doctor();
            string expectedSpecialization = "Cardiology";

            // Act
            doctor.Specialization = expectedSpecialization;

            // Assert
            Assert.Equal(expectedSpecialization, doctor.Specialization);
        }

        [Fact]
        public void Doctor_SetQualification_StoresValue()
        {
            // Arrange
            var doctor = new Doctor();
            string expectedQualification = "MBBS, MD";

            // Act
            doctor.Qualification = expectedQualification;

            // Assert
            Assert.Equal(expectedQualification, doctor.Qualification);
        }

        [Fact]
        public void Doctor_SetExperience_StoresValue()
        {
            // Arrange
            var doctor = new Doctor();
            int expectedExperience = 15;

            // Act
            doctor.Experience = expectedExperience;

            // Assert
            Assert.Equal(expectedExperience, doctor.Experience);
        }

        [Fact]
        public void Doctor_SetExperience_StoresZero()
        {
            // Arrange
            var doctor = new Doctor();

            // Act
            doctor.Experience = 0;

            // Assert
            Assert.Equal(0, doctor.Experience);
        }

        [Fact]
        public void Doctor_SetConsultationFee_StoresValue()
        {
            // Arrange
            var doctor = new Doctor();
            decimal expectedFee = 500.75m;

            // Act
            doctor.ConsultationFee = expectedFee;

            // Assert
            Assert.Equal(expectedFee, doctor.ConsultationFee);
        }

        [Fact]
        public void Doctor_SetConsultationFee_StoresZero()
        {
            // Arrange
            var doctor = new Doctor();

            // Act
            doctor.ConsultationFee = 0m;

            // Assert
            Assert.Equal(0m, doctor.ConsultationFee);
        }

        [Fact]
        public void Doctor_SetProfileImage_StoresValue()
        {
            // Arrange
            var doctor = new Doctor();
            string expectedImage = "profile.jpg";

            // Act
            doctor.ProfileImage = expectedImage;

            // Assert
            Assert.Equal(expectedImage, doctor.ProfileImage);
        }

        [Fact]
        public void Doctor_SetProfileImage_AcceptsNull()
        {
            // Arrange
            var doctor = new Doctor();

            // Act
            doctor.ProfileImage = null;

            // Assert
            Assert.Null(doctor.ProfileImage);
        }

        [Fact]
        public void Doctor_SetCreatedDate_StoresValue()
        {
            // Arrange
            var doctor = new Doctor();
            DateTime expectedDate = DateTime.UtcNow;

            // Act
            doctor.CreatedDate = expectedDate;

            // Assert
            Assert.Equal(expectedDate, doctor.CreatedDate);
        }

        [Fact]
        public void Doctor_SetModifiedDate_StoresValue()
        {
            // Arrange
            var doctor = new Doctor();
            DateTime? expectedDate = DateTime.UtcNow;

            // Act
            doctor.ModifiedDate = expectedDate;

            // Assert
            Assert.Equal(expectedDate, doctor.ModifiedDate);
        }

        [Fact]
        public void Doctor_SetModifiedDate_AcceptsNull()
        {
            // Arrange
            var doctor = new Doctor();

            // Act
            doctor.ModifiedDate = null;

            // Assert
            Assert.Null(doctor.ModifiedDate);
        }

        [Fact]
        public void Doctor_SetIsActive_StoresTrue()
        {
            // Arrange
            var doctor = new Doctor();

            // Act
            doctor.IsActive = true;

            // Assert
            Assert.True(doctor.IsActive);
        }

        [Fact]
        public void Doctor_SetIsActive_StoresFalse()
        {
            // Arrange
            var doctor = new Doctor();

            // Act
            doctor.IsActive = false;

            // Assert
            Assert.False(doctor.IsActive);
        }

        [Fact]
        public void Doctor_SetCreatedBy_StoresValue()
        {
            // Arrange
            var doctor = new Doctor();
            string expectedCreatedBy = "Admin";

            // Act
            doctor.CreatedBy = expectedCreatedBy;

            // Assert
            Assert.Equal(expectedCreatedBy, doctor.CreatedBy);
        }

        [Fact]
        public void Doctor_SetModifiedBy_StoresValue()
        {
            // Arrange
            var doctor = new Doctor();
            string expectedModifiedBy = "SuperAdmin";

            // Act
            doctor.ModifiedBy = expectedModifiedBy;

            // Assert
            Assert.Equal(expectedModifiedBy, doctor.ModifiedBy);
        }

        [Fact]
        public void Doctor_SetModifiedBy_AcceptsNull()
        {
            // Arrange
            var doctor = new Doctor();

            // Act
            doctor.ModifiedBy = null;

            // Assert
            Assert.Null(doctor.ModifiedBy);
        }

        [Fact]
        public void Doctor_Appointments_InitializesAsEmptyList()
        {
            // Arrange & Act
            var doctor = new Doctor();

            // Assert
            Assert.NotNull(doctor.Appointments);
            Assert.Empty(doctor.Appointments);
        }

        [Fact]
        public void Doctor_TreatmentHistories_InitializesAsEmptyList()
        {
            // Arrange & Act
            var doctor = new Doctor();

            // Assert
            Assert.NotNull(doctor.TreatmentHistories);
            Assert.Empty(doctor.TreatmentHistories);
        }

        [Fact]
        public void Doctor_AllProperties_CanBeSetAndRetrieved()
        {
            // Arrange
            var doctor = new Doctor
            {
                Id = 10,
                Name = "Dr. Johnson",
                Email = "johnson@clinic.com",
                Password = "Secure999",
                Phone = "5551234567",
                Specialization = "Neurology",
                Qualification = "MBBS, DM Neurology",
                Experience = 20,
                ConsultationFee = 800.50m,
                ProfileImage = "doctor_johnson.png",
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = DateTime.UtcNow,
                IsActive = true,
                CreatedBy = "System",
                ModifiedBy = "Admin"
            };

            // Assert
            Assert.Equal(10, doctor.Id);
            Assert.Equal("Dr. Johnson", doctor.Name);
            Assert.Equal("johnson@clinic.com", doctor.Email);
            Assert.Equal("Secure999", doctor.Password);
            Assert.Equal("5551234567", doctor.Phone);
            Assert.Equal("Neurology", doctor.Specialization);
            Assert.Equal("MBBS, DM Neurology", doctor.Qualification);
            Assert.Equal(20, doctor.Experience);
            Assert.Equal(800.50m, doctor.ConsultationFee);
            Assert.Equal("doctor_johnson.png", doctor.ProfileImage);
            Assert.True(doctor.IsActive);
            Assert.Equal("System", doctor.CreatedBy);
            Assert.Equal("Admin", doctor.ModifiedBy);
        }
    }
}
