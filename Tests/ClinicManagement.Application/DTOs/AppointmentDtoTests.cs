using System;
using Xunit;
using ClinicManagement.Application.DTOs;

namespace ClinicManagement.Application.DTOs.Tests
{
    public class AppointmentDtoTests
    {
        [Fact]
        public void AppointmentDto_Constructor_InitializesProperties()
        {
            // Arrange & Act
            var dto = new AppointmentDto();

            // Assert
            Assert.NotNull(dto);
            Assert.Equal(0, dto.Id);
            Assert.Equal(0, dto.PatientId);
            Assert.Equal(0, dto.DoctorId);
            Assert.Equal(string.Empty, dto.PatientName);
            Assert.Equal(string.Empty, dto.DoctorName);
            Assert.Equal(string.Empty, dto.TimeSlot);
            Assert.Equal(string.Empty, dto.Status);
        }

        [Fact]
        public void AppointmentDto_AllProperties_CanBeSetAndRetrieved()
        {
            // Arrange
            var dto = new AppointmentDto
            {
                Id = 100,
                PatientId = 50,
                DoctorId = 25,
                PatientName = "John Doe",
                DoctorName = "Dr. Smith",
                AppointmentDate = new DateTime(2026, 2, 15, 10, 0, 0),
                TimeSlot = "10:00 AM - 10:30 AM",
                Status = "Confirmed",
                Reason = "Checkup",
                Notes = "Patient requested morning slot"
            };

            // Assert
            Assert.Equal(100, dto.Id);
            Assert.Equal(50, dto.PatientId);
            Assert.Equal(25, dto.DoctorId);
            Assert.Equal("John Doe", dto.PatientName);
            Assert.Equal("Dr. Smith", dto.DoctorName);
            Assert.Equal(new DateTime(2026, 2, 15, 10, 0, 0), dto.AppointmentDate);
            Assert.Equal("10:00 AM - 10:30 AM", dto.TimeSlot);
            Assert.Equal("Confirmed", dto.Status);
            Assert.Equal("Checkup", dto.Reason);
            Assert.Equal("Patient requested morning slot", dto.Notes);
        }

        [Fact]
        public void AppointmentCreateDto_Constructor_InitializesProperties()
        {
            // Arrange & Act
            var dto = new AppointmentCreateDto();

            // Assert
            Assert.NotNull(dto);
            Assert.Equal(0, dto.PatientId);
            Assert.Equal(0, dto.DoctorId);
            Assert.Equal(string.Empty, dto.TimeSlot);
        }

        [Fact]
        public void AppointmentCreateDto_AllProperties_CanBeSetAndRetrieved()
        {
            // Arrange
            var dto = new AppointmentCreateDto
            {
                PatientId = 75,
                DoctorId = 40,
                AppointmentDate = new DateTime(2026, 3, 20, 14, 0, 0),
                TimeSlot = "2:00 PM - 2:30 PM",
                Reason = "Follow-up"
            };

            // Assert
            Assert.Equal(75, dto.PatientId);
            Assert.Equal(40, dto.DoctorId);
            Assert.Equal(new DateTime(2026, 3, 20, 14, 0, 0), dto.AppointmentDate);
            Assert.Equal("2:00 PM - 2:30 PM", dto.TimeSlot);
            Assert.Equal("Follow-up", dto.Reason);
        }

        [Fact]
        public void AppointmentUpdateDto_Constructor_InitializesProperties()
        {
            // Arrange & Act
            var dto = new AppointmentUpdateDto();

            // Assert
            Assert.NotNull(dto);
            Assert.Equal(string.Empty, dto.TimeSlot);
            Assert.Equal(string.Empty, dto.Status);
        }

        [Fact]
        public void AppointmentUpdateDto_AllProperties_CanBeSetAndRetrieved()
        {
            // Arrange
            var dto = new AppointmentUpdateDto
            {
                AppointmentDate = new DateTime(2026, 4, 10, 9, 0, 0),
                TimeSlot = "9:00 AM - 9:30 AM",
                Status = "Rescheduled",
                Reason = "Emergency",
                Notes = "Patient called to reschedule"
            };

            // Assert
            Assert.Equal(new DateTime(2026, 4, 10, 9, 0, 0), dto.AppointmentDate);
            Assert.Equal("9:00 AM - 9:30 AM", dto.TimeSlot);
            Assert.Equal("Rescheduled", dto.Status);
            Assert.Equal("Emergency", dto.Reason);
            Assert.Equal("Patient called to reschedule", dto.Notes);
        }
    }
}
