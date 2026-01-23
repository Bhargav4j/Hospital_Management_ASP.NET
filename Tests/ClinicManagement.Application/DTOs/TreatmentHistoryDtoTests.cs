using System;
using Xunit;
using ClinicManagement.Application.DTOs;

namespace ClinicManagement.Application.DTOs.Tests
{
    public class TreatmentHistoryDtoTests
    {
        [Fact]
        public void TreatmentHistoryDto_Constructor_InitializesProperties()
        {
            // Arrange & Act
            var dto = new TreatmentHistoryDto();

            // Assert
            Assert.NotNull(dto);
            Assert.Equal(0, dto.Id);
            Assert.Equal(0, dto.PatientId);
            Assert.Equal(0, dto.DoctorId);
            Assert.Equal(string.Empty, dto.PatientName);
            Assert.Equal(string.Empty, dto.DoctorName);
            Assert.Equal(string.Empty, dto.Diagnosis);
            Assert.Equal(string.Empty, dto.Treatment);
        }

        [Fact]
        public void TreatmentHistoryDto_AllProperties_CanBeSetAndRetrieved()
        {
            // Arrange
            var dto = new TreatmentHistoryDto
            {
                Id = 300,
                PatientId = 200,
                DoctorId = 100,
                PatientName = "Alice Brown",
                DoctorName = "Dr. Williams",
                TreatmentDate = new DateTime(2026, 1, 15),
                Diagnosis = "Hypertension",
                Treatment = "Medication and lifestyle changes",
                Prescription = "Amlodipine 5mg daily",
                Notes = "Monitor blood pressure weekly"
            };

            // Assert
            Assert.Equal(300, dto.Id);
            Assert.Equal(200, dto.PatientId);
            Assert.Equal(100, dto.DoctorId);
            Assert.Equal("Alice Brown", dto.PatientName);
            Assert.Equal("Dr. Williams", dto.DoctorName);
            Assert.Equal(new DateTime(2026, 1, 15), dto.TreatmentDate);
            Assert.Equal("Hypertension", dto.Diagnosis);
            Assert.Equal("Medication and lifestyle changes", dto.Treatment);
            Assert.Equal("Amlodipine 5mg daily", dto.Prescription);
            Assert.Equal("Monitor blood pressure weekly", dto.Notes);
        }

        [Fact]
        public void TreatmentHistoryCreateDto_Constructor_InitializesProperties()
        {
            // Arrange & Act
            var dto = new TreatmentHistoryCreateDto();

            // Assert
            Assert.NotNull(dto);
            Assert.Equal(0, dto.PatientId);
            Assert.Equal(0, dto.DoctorId);
            Assert.Equal(string.Empty, dto.Diagnosis);
            Assert.Equal(string.Empty, dto.Treatment);
        }

        [Fact]
        public void TreatmentHistoryCreateDto_AllProperties_CanBeSetAndRetrieved()
        {
            // Arrange
            var dto = new TreatmentHistoryCreateDto
            {
                PatientId = 400,
                DoctorId = 200,
                AppointmentId = 500,
                Diagnosis = "Acute bronchitis",
                Treatment = "Antibiotics and rest",
                Prescription = "Amoxicillin 500mg 3x daily",
                Notes = "Follow-up in 7 days"
            };

            // Assert
            Assert.Equal(400, dto.PatientId);
            Assert.Equal(200, dto.DoctorId);
            Assert.Equal(500, dto.AppointmentId);
            Assert.Equal("Acute bronchitis", dto.Diagnosis);
            Assert.Equal("Antibiotics and rest", dto.Treatment);
            Assert.Equal("Amoxicillin 500mg 3x daily", dto.Prescription);
            Assert.Equal("Follow-up in 7 days", dto.Notes);
        }

        [Fact]
        public void TreatmentHistoryCreateDto_AppointmentId_AcceptsNull()
        {
            // Arrange
            var dto = new TreatmentHistoryCreateDto { AppointmentId = null };

            // Assert
            Assert.Null(dto.AppointmentId);
        }

        [Fact]
        public void TreatmentHistoryUpdateDto_Constructor_InitializesProperties()
        {
            // Arrange & Act
            var dto = new TreatmentHistoryUpdateDto();

            // Assert
            Assert.NotNull(dto);
            Assert.Equal(string.Empty, dto.Diagnosis);
            Assert.Equal(string.Empty, dto.Treatment);
        }

        [Fact]
        public void TreatmentHistoryUpdateDto_AllProperties_CanBeSetAndRetrieved()
        {
            // Arrange
            var dto = new TreatmentHistoryUpdateDto
            {
                Diagnosis = "Type 2 Diabetes",
                Treatment = "Insulin therapy",
                Prescription = "Metformin 1000mg twice daily",
                Notes = "Patient responding well to treatment"
            };

            // Assert
            Assert.Equal("Type 2 Diabetes", dto.Diagnosis);
            Assert.Equal("Insulin therapy", dto.Treatment);
            Assert.Equal("Metformin 1000mg twice daily", dto.Prescription);
            Assert.Equal("Patient responding well to treatment", dto.Notes);
        }
    }
}
