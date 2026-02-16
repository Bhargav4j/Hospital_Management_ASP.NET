using Xunit;
using ClinicManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ClinicManagement.Domain.Tests.Entities
{
    public class PatientTests
    {
        [Fact]
        public void Patient_Constructor_ShouldInitializeWithDefaultValues()
        {
            // Act
            var patient = new Patient();

            // Assert
            Assert.Equal(0, patient.PatientID);
            Assert.Equal(string.Empty, patient.Name);
            Assert.Equal(string.Empty, patient.Email);
            Assert.Equal(string.Empty, patient.Password);
            Assert.Equal(string.Empty, patient.Phone);
            Assert.Equal(string.Empty, patient.Address);
            Assert.Equal(string.Empty, patient.Gender);
            Assert.NotNull(patient.Appointments);
            Assert.Empty(patient.Appointments);
        }

        [Fact]
        public void Patient_SetProperties_ShouldRetainValues()
        {
            // Arrange
            var patient = new Patient();
            var testDate = new DateTime(1990, 5, 15);

            // Act
            patient.PatientID = 1;
            patient.Name = "John Doe";
            patient.Email = "john@example.com";
            patient.Password = "password123";
            patient.Phone = "1234567890";
            patient.Address = "123 Main St";
            patient.BirthDate = testDate;
            patient.Gender = "Male";

            // Assert
            Assert.Equal(1, patient.PatientID);
            Assert.Equal("John Doe", patient.Name);
            Assert.Equal("john@example.com", patient.Email);
            Assert.Equal("password123", patient.Password);
            Assert.Equal("1234567890", patient.Phone);
            Assert.Equal("123 Main St", patient.Address);
            Assert.Equal(testDate, patient.BirthDate);
            Assert.Equal("Male", patient.Gender);
        }

        [Fact]
        public void Patient_CreatedDate_ShouldBeSetAutomatically()
        {
            // Arrange & Act
            var before = DateTime.UtcNow.AddSeconds(-1);
            var patient = new Patient();
            var after = DateTime.UtcNow.AddSeconds(1);

            // Assert
            Assert.True(patient.CreatedDate >= before);
            Assert.True(patient.CreatedDate <= after);
        }

        [Fact]
        public void Patient_ModifiedDate_ShouldBeNullByDefault()
        {
            // Act
            var patient = new Patient();

            // Assert
            Assert.Null(patient.ModifiedDate);
        }

        [Fact]
        public void Patient_ModifiedDate_CanBeSet()
        {
            // Arrange
            var patient = new Patient();
            var modifiedDate = DateTime.UtcNow;

            // Act
            patient.ModifiedDate = modifiedDate;

            // Assert
            Assert.Equal(modifiedDate, patient.ModifiedDate);
        }

        [Fact]
        public void Patient_Appointments_CanAddAppointment()
        {
            // Arrange
            var patient = new Patient();
            var appointment = new Appointment { AppointmentID = 1 };

            // Act
            patient.Appointments.Add(appointment);

            // Assert
            Assert.Single(patient.Appointments);
            Assert.Contains(appointment, patient.Appointments);
        }

        [Fact]
        public void Patient_Appointments_CanAddMultipleAppointments()
        {
            // Arrange
            var patient = new Patient();
            var appointment1 = new Appointment { AppointmentID = 1 };
            var appointment2 = new Appointment { AppointmentID = 2 };

            // Act
            patient.Appointments.Add(appointment1);
            patient.Appointments.Add(appointment2);

            // Assert
            Assert.Equal(2, patient.Appointments.Count);
        }

        [Theory]
        [InlineData("")]
        [InlineData("test@test.com")]
        [InlineData("user@domain.co.uk")]
        public void Patient_Email_AcceptsDifferentFormats(string email)
        {
            // Arrange
            var patient = new Patient();

            // Act
            patient.Email = email;

            // Assert
            Assert.Equal(email, patient.Email);
        }

        [Theory]
        [InlineData("Male")]
        [InlineData("Female")]
        [InlineData("Other")]
        public void Patient_Gender_AcceptsDifferentValues(string gender)
        {
            // Arrange
            var patient = new Patient();

            // Act
            patient.Gender = gender;

            // Assert
            Assert.Equal(gender, patient.Gender);
        }
    }
}
