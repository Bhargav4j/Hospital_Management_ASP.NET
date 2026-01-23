using System;
using Xunit;
using ClinicManagement.Domain.Entities;

namespace ClinicManagement.Domain.Entities.Tests
{
    public class TreatmentHistoryTests
    {
        [Fact]
        public void TreatmentHistory_Constructor_InitializesProperties()
        {
            // Arrange & Act
            var treatment = new TreatmentHistory();

            // Assert
            Assert.NotNull(treatment);
            Assert.Equal(0, treatment.Id);
            Assert.Equal(0, treatment.PatientId);
            Assert.Equal(0, treatment.DoctorId);
            Assert.Equal(string.Empty, treatment.Diagnosis);
            Assert.Equal(string.Empty, treatment.Treatment);
            Assert.Equal(string.Empty, treatment.CreatedBy);
            Assert.False(treatment.IsActive);
        }

        [Fact]
        public void TreatmentHistory_SetId_StoresValue()
        {
            // Arrange
            var treatment = new TreatmentHistory();
            int expectedId = 401;

            // Act
            treatment.Id = expectedId;

            // Assert
            Assert.Equal(expectedId, treatment.Id);
        }

        [Fact]
        public void TreatmentHistory_SetPatientId_StoresValue()
        {
            // Arrange
            var treatment = new TreatmentHistory();
            int expectedPatientId = 200;

            // Act
            treatment.PatientId = expectedPatientId;

            // Assert
            Assert.Equal(expectedPatientId, treatment.PatientId);
        }

        [Fact]
        public void TreatmentHistory_SetDoctorId_StoresValue()
        {
            // Arrange
            var treatment = new TreatmentHistory();
            int expectedDoctorId = 100;

            // Act
            treatment.DoctorId = expectedDoctorId;

            // Assert
            Assert.Equal(expectedDoctorId, treatment.DoctorId);
        }

        [Fact]
        public void TreatmentHistory_SetAppointmentId_StoresValue()
        {
            // Arrange
            var treatment = new TreatmentHistory();
            int? expectedAppointmentId = 300;

            // Act
            treatment.AppointmentId = expectedAppointmentId;

            // Assert
            Assert.Equal(expectedAppointmentId, treatment.AppointmentId);
        }

        [Fact]
        public void TreatmentHistory_SetAppointmentId_AcceptsNull()
        {
            // Arrange
            var treatment = new TreatmentHistory();

            // Act
            treatment.AppointmentId = null;

            // Assert
            Assert.Null(treatment.AppointmentId);
        }

        [Fact]
        public void TreatmentHistory_SetTreatmentDate_StoresValue()
        {
            // Arrange
            var treatment = new TreatmentHistory();
            DateTime expectedDate = new DateTime(2026, 1, 15);

            // Act
            treatment.TreatmentDate = expectedDate;

            // Assert
            Assert.Equal(expectedDate, treatment.TreatmentDate);
        }

        [Fact]
        public void TreatmentHistory_SetDiagnosis_StoresValue()
        {
            // Arrange
            var treatment = new TreatmentHistory();
            string expectedDiagnosis = "Acute bronchitis";

            // Act
            treatment.Diagnosis = expectedDiagnosis;

            // Assert
            Assert.Equal(expectedDiagnosis, treatment.Diagnosis);
        }

        [Fact]
        public void TreatmentHistory_SetTreatment_StoresValue()
        {
            // Arrange
            var treatment = new TreatmentHistory();
            string expectedTreatment = "Antibiotics and rest";

            // Act
            treatment.Treatment = expectedTreatment;

            // Assert
            Assert.Equal(expectedTreatment, treatment.Treatment);
        }

        [Fact]
        public void TreatmentHistory_SetPrescription_StoresValue()
        {
            // Arrange
            var treatment = new TreatmentHistory();
            string expectedPrescription = "Amoxicillin 500mg 3x daily";

            // Act
            treatment.Prescription = expectedPrescription;

            // Assert
            Assert.Equal(expectedPrescription, treatment.Prescription);
        }

        [Fact]
        public void TreatmentHistory_SetPrescription_AcceptsNull()
        {
            // Arrange
            var treatment = new TreatmentHistory();

            // Act
            treatment.Prescription = null;

            // Assert
            Assert.Null(treatment.Prescription);
        }

        [Fact]
        public void TreatmentHistory_SetNotes_StoresValue()
        {
            // Arrange
            var treatment = new TreatmentHistory();
            string expectedNotes = "Patient responded well to treatment";

            // Act
            treatment.Notes = expectedNotes;

            // Assert
            Assert.Equal(expectedNotes, treatment.Notes);
        }

        [Fact]
        public void TreatmentHistory_SetNotes_AcceptsNull()
        {
            // Arrange
            var treatment = new TreatmentHistory();

            // Act
            treatment.Notes = null;

            // Assert
            Assert.Null(treatment.Notes);
        }

        [Fact]
        public void TreatmentHistory_SetCreatedDate_StoresValue()
        {
            // Arrange
            var treatment = new TreatmentHistory();
            DateTime expectedDate = DateTime.UtcNow;

            // Act
            treatment.CreatedDate = expectedDate;

            // Assert
            Assert.Equal(expectedDate, treatment.CreatedDate);
        }

        [Fact]
        public void TreatmentHistory_SetModifiedDate_StoresValue()
        {
            // Arrange
            var treatment = new TreatmentHistory();
            DateTime? expectedDate = DateTime.UtcNow;

            // Act
            treatment.ModifiedDate = expectedDate;

            // Assert
            Assert.Equal(expectedDate, treatment.ModifiedDate);
        }

        [Fact]
        public void TreatmentHistory_SetModifiedDate_AcceptsNull()
        {
            // Arrange
            var treatment = new TreatmentHistory();

            // Act
            treatment.ModifiedDate = null;

            // Assert
            Assert.Null(treatment.ModifiedDate);
        }

        [Fact]
        public void TreatmentHistory_SetIsActive_StoresTrue()
        {
            // Arrange
            var treatment = new TreatmentHistory();

            // Act
            treatment.IsActive = true;

            // Assert
            Assert.True(treatment.IsActive);
        }

        [Fact]
        public void TreatmentHistory_SetCreatedBy_StoresValue()
        {
            // Arrange
            var treatment = new TreatmentHistory();
            string expectedCreatedBy = "Dr. Smith";

            // Act
            treatment.CreatedBy = expectedCreatedBy;

            // Assert
            Assert.Equal(expectedCreatedBy, treatment.CreatedBy);
        }

        [Fact]
        public void TreatmentHistory_SetModifiedBy_StoresValue()
        {
            // Arrange
            var treatment = new TreatmentHistory();
            string expectedModifiedBy = "Admin";

            // Act
            treatment.ModifiedBy = expectedModifiedBy;

            // Assert
            Assert.Equal(expectedModifiedBy, treatment.ModifiedBy);
        }

        [Fact]
        public void TreatmentHistory_SetModifiedBy_AcceptsNull()
        {
            // Arrange
            var treatment = new TreatmentHistory();

            // Act
            treatment.ModifiedBy = null;

            // Assert
            Assert.Null(treatment.ModifiedBy);
        }

        [Fact]
        public void TreatmentHistory_AllProperties_CanBeSetAndRetrieved()
        {
            // Arrange
            var treatment = new TreatmentHistory
            {
                Id = 555,
                PatientId = 777,
                DoctorId = 333,
                AppointmentId = 888,
                TreatmentDate = new DateTime(2026, 1, 10),
                Diagnosis = "Type 2 Diabetes",
                Treatment = "Insulin therapy",
                Prescription = "Metformin 1000mg twice daily",
                Notes = "Monitor blood sugar levels",
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = DateTime.UtcNow,
                IsActive = true,
                CreatedBy = "Dr. Johnson",
                ModifiedBy = "Nurse"
            };

            // Assert
            Assert.Equal(555, treatment.Id);
            Assert.Equal(777, treatment.PatientId);
            Assert.Equal(333, treatment.DoctorId);
            Assert.Equal(888, treatment.AppointmentId);
            Assert.Equal(new DateTime(2026, 1, 10), treatment.TreatmentDate);
            Assert.Equal("Type 2 Diabetes", treatment.Diagnosis);
            Assert.Equal("Insulin therapy", treatment.Treatment);
            Assert.Equal("Metformin 1000mg twice daily", treatment.Prescription);
            Assert.Equal("Monitor blood sugar levels", treatment.Notes);
            Assert.True(treatment.IsActive);
            Assert.Equal("Dr. Johnson", treatment.CreatedBy);
            Assert.Equal("Nurse", treatment.ModifiedBy);
        }
    }
}
