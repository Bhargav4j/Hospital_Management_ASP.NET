using System;
using Xunit;
using ClinicManagement.Domain.Entities;

namespace ClinicManagement.Domain.Entities.Tests
{
    public class AppointmentTests
    {
        [Fact]
        public void Appointment_Constructor_InitializesProperties()
        {
            // Arrange & Act
            var appointment = new Appointment();

            // Assert
            Assert.NotNull(appointment);
            Assert.Equal(0, appointment.Id);
            Assert.Equal(0, appointment.PatientId);
            Assert.Equal(0, appointment.DoctorId);
            Assert.Equal(string.Empty, appointment.TimeSlot);
            Assert.Equal(string.Empty, appointment.Status);
            Assert.Equal(string.Empty, appointment.CreatedBy);
            Assert.False(appointment.IsActive);
        }

        [Fact]
        public void Appointment_SetId_StoresValue()
        {
            // Arrange
            var appointment = new Appointment();
            int expectedId = 101;

            // Act
            appointment.Id = expectedId;

            // Assert
            Assert.Equal(expectedId, appointment.Id);
        }

        [Fact]
        public void Appointment_SetPatientId_StoresValue()
        {
            // Arrange
            var appointment = new Appointment();
            int expectedPatientId = 50;

            // Act
            appointment.PatientId = expectedPatientId;

            // Assert
            Assert.Equal(expectedPatientId, appointment.PatientId);
        }

        [Fact]
        public void Appointment_SetDoctorId_StoresValue()
        {
            // Arrange
            var appointment = new Appointment();
            int expectedDoctorId = 25;

            // Act
            appointment.DoctorId = expectedDoctorId;

            // Assert
            Assert.Equal(expectedDoctorId, appointment.DoctorId);
        }

        [Fact]
        public void Appointment_SetAppointmentDate_StoresValue()
        {
            // Arrange
            var appointment = new Appointment();
            DateTime expectedDate = new DateTime(2026, 2, 15, 10, 30, 0);

            // Act
            appointment.AppointmentDate = expectedDate;

            // Assert
            Assert.Equal(expectedDate, appointment.AppointmentDate);
        }

        [Fact]
        public void Appointment_SetTimeSlot_StoresValue()
        {
            // Arrange
            var appointment = new Appointment();
            string expectedTimeSlot = "10:00 AM - 10:30 AM";

            // Act
            appointment.TimeSlot = expectedTimeSlot;

            // Assert
            Assert.Equal(expectedTimeSlot, appointment.TimeSlot);
        }

        [Fact]
        public void Appointment_SetStatus_StoresValue()
        {
            // Arrange
            var appointment = new Appointment();
            string expectedStatus = "Confirmed";

            // Act
            appointment.Status = expectedStatus;

            // Assert
            Assert.Equal(expectedStatus, appointment.Status);
        }

        [Fact]
        public void Appointment_SetReason_StoresValue()
        {
            // Arrange
            var appointment = new Appointment();
            string expectedReason = "Annual checkup";

            // Act
            appointment.Reason = expectedReason;

            // Assert
            Assert.Equal(expectedReason, appointment.Reason);
        }

        [Fact]
        public void Appointment_SetReason_AcceptsNull()
        {
            // Arrange
            var appointment = new Appointment();

            // Act
            appointment.Reason = null;

            // Assert
            Assert.Null(appointment.Reason);
        }

        [Fact]
        public void Appointment_SetNotes_StoresValue()
        {
            // Arrange
            var appointment = new Appointment();
            string expectedNotes = "Patient requested morning appointment";

            // Act
            appointment.Notes = expectedNotes;

            // Assert
            Assert.Equal(expectedNotes, appointment.Notes);
        }

        [Fact]
        public void Appointment_SetNotes_AcceptsNull()
        {
            // Arrange
            var appointment = new Appointment();

            // Act
            appointment.Notes = null;

            // Assert
            Assert.Null(appointment.Notes);
        }

        [Fact]
        public void Appointment_SetCreatedDate_StoresValue()
        {
            // Arrange
            var appointment = new Appointment();
            DateTime expectedDate = DateTime.UtcNow;

            // Act
            appointment.CreatedDate = expectedDate;

            // Assert
            Assert.Equal(expectedDate, appointment.CreatedDate);
        }

        [Fact]
        public void Appointment_SetModifiedDate_StoresValue()
        {
            // Arrange
            var appointment = new Appointment();
            DateTime? expectedDate = DateTime.UtcNow;

            // Act
            appointment.ModifiedDate = expectedDate;

            // Assert
            Assert.Equal(expectedDate, appointment.ModifiedDate);
        }

        [Fact]
        public void Appointment_SetModifiedDate_AcceptsNull()
        {
            // Arrange
            var appointment = new Appointment();

            // Act
            appointment.ModifiedDate = null;

            // Assert
            Assert.Null(appointment.ModifiedDate);
        }

        [Fact]
        public void Appointment_SetIsActive_StoresTrue()
        {
            // Arrange
            var appointment = new Appointment();

            // Act
            appointment.IsActive = true;

            // Assert
            Assert.True(appointment.IsActive);
        }

        [Fact]
        public void Appointment_SetCreatedBy_StoresValue()
        {
            // Arrange
            var appointment = new Appointment();
            string expectedCreatedBy = "ReceptionDesk";

            // Act
            appointment.CreatedBy = expectedCreatedBy;

            // Assert
            Assert.Equal(expectedCreatedBy, appointment.CreatedBy);
        }

        [Fact]
        public void Appointment_SetModifiedBy_StoresValue()
        {
            // Arrange
            var appointment = new Appointment();
            string expectedModifiedBy = "Admin";

            // Act
            appointment.ModifiedBy = expectedModifiedBy;

            // Assert
            Assert.Equal(expectedModifiedBy, appointment.ModifiedBy);
        }

        [Fact]
        public void Appointment_SetModifiedBy_AcceptsNull()
        {
            // Arrange
            var appointment = new Appointment();

            // Act
            appointment.ModifiedBy = null;

            // Assert
            Assert.Null(appointment.ModifiedBy);
        }

        [Fact]
        public void Appointment_AllProperties_CanBeSetAndRetrieved()
        {
            // Arrange
            var appointment = new Appointment
            {
                Id = 200,
                PatientId = 100,
                DoctorId = 50,
                AppointmentDate = new DateTime(2026, 3, 10, 14, 0, 0),
                TimeSlot = "2:00 PM - 2:30 PM",
                Status = "Scheduled",
                Reason = "Follow-up consultation",
                Notes = "Patient has history of hypertension",
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = DateTime.UtcNow,
                IsActive = true,
                CreatedBy = "Receptionist",
                ModifiedBy = "System"
            };

            // Assert
            Assert.Equal(200, appointment.Id);
            Assert.Equal(100, appointment.PatientId);
            Assert.Equal(50, appointment.DoctorId);
            Assert.Equal(new DateTime(2026, 3, 10, 14, 0, 0), appointment.AppointmentDate);
            Assert.Equal("2:00 PM - 2:30 PM", appointment.TimeSlot);
            Assert.Equal("Scheduled", appointment.Status);
            Assert.Equal("Follow-up consultation", appointment.Reason);
            Assert.Equal("Patient has history of hypertension", appointment.Notes);
            Assert.True(appointment.IsActive);
            Assert.Equal("Receptionist", appointment.CreatedBy);
            Assert.Equal("System", appointment.ModifiedBy);
        }
    }
}
