using System;
using Xunit;
using Microsoft.EntityFrameworkCore;
using ClinicManagement.Domain.Entities;
using ClinicManagement.Infrastructure.Data;

namespace ClinicManagement.Infrastructure.Data.Tests
{
    public class ClinicDbContextTests
    {
        private ClinicDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<ClinicDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            return new ClinicDbContext(options);
        }

        [Fact]
        public void ClinicDbContext_Constructor_InitializesDbSets()
        {
            // Arrange & Act
            var context = GetInMemoryDbContext();

            // Assert
            Assert.NotNull(context.Patients);
            Assert.NotNull(context.Doctors);
            Assert.NotNull(context.Appointments);
            Assert.NotNull(context.Bills);
            Assert.NotNull(context.TreatmentHistories);
            Assert.NotNull(context.Feedbacks);
            Assert.NotNull(context.Staff);
        }

        [Fact]
        public void ClinicDbContext_CanAddPatient()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var patient = new Patient
            {
                Name = "Test Patient",
                Email = "test@example.com",
                IsActive = true
            };

            // Act
            context.Patients.Add(patient);
            context.SaveChanges();

            // Assert
            Assert.True(patient.Id > 0);
        }

        [Fact]
        public void ClinicDbContext_CanAddDoctor()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var doctor = new Doctor
            {
                Name = "Dr. Test",
                Email = "doctor@test.com",
                IsActive = true
            };

            // Act
            context.Doctors.Add(doctor);
            context.SaveChanges();

            // Assert
            Assert.True(doctor.Id > 0);
        }

        [Fact]
        public void ClinicDbContext_CanAddAppointment()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var patient = new Patient { Name = "Patient", IsActive = true };
            var doctor = new Doctor { Name = "Doctor", IsActive = true };
            context.Patients.Add(patient);
            context.Doctors.Add(doctor);
            context.SaveChanges();

            var appointment = new Appointment
            {
                PatientId = patient.Id,
                DoctorId = doctor.Id,
                TimeSlot = "10:00 AM",
                IsActive = true
            };

            // Act
            context.Appointments.Add(appointment);
            context.SaveChanges();

            // Assert
            Assert.True(appointment.Id > 0);
        }

        [Fact]
        public void ClinicDbContext_CanAddBill()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var patient = new Patient { Name = "Patient", IsActive = true };
            context.Patients.Add(patient);
            context.SaveChanges();

            var bill = new Bill
            {
                PatientId = patient.Id,
                Amount = 500.00m,
                IsActive = true
            };

            // Act
            context.Bills.Add(bill);
            context.SaveChanges();

            // Assert
            Assert.True(bill.Id > 0);
        }

        [Fact]
        public void ClinicDbContext_CanAddTreatmentHistory()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var patient = new Patient { Name = "Patient", IsActive = true };
            var doctor = new Doctor { Name = "Doctor", IsActive = true };
            context.Patients.Add(patient);
            context.Doctors.Add(doctor);
            context.SaveChanges();

            var treatment = new TreatmentHistory
            {
                PatientId = patient.Id,
                DoctorId = doctor.Id,
                Diagnosis = "Flu",
                Treatment = "Rest",
                IsActive = true
            };

            // Act
            context.TreatmentHistories.Add(treatment);
            context.SaveChanges();

            // Assert
            Assert.True(treatment.Id > 0);
        }

        [Fact]
        public void ClinicDbContext_CanAddFeedback()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var patient = new Patient { Name = "Patient", IsActive = true };
            context.Patients.Add(patient);
            context.SaveChanges();

            var feedback = new Feedback
            {
                PatientId = patient.Id,
                Subject = "Great service",
                Message = "Very satisfied",
                Rating = 5,
                IsActive = true
            };

            // Act
            context.Feedbacks.Add(feedback);
            context.SaveChanges();

            // Assert
            Assert.True(feedback.Id > 0);
        }

        [Fact]
        public void ClinicDbContext_CanAddStaff()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var staff = new Staff
            {
                Name = "Staff Member",
                Email = "staff@clinic.com",
                Role = "Nurse",
                IsActive = true
            };

            // Act
            context.Staff.Add(staff);
            context.SaveChanges();

            // Assert
            Assert.True(staff.Id > 0);
        }

        [Fact]
        public void ClinicDbContext_CanQueryPatients()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            context.Patients.Add(new Patient { Name = "Patient 1", IsActive = true });
            context.Patients.Add(new Patient { Name = "Patient 2", IsActive = true });
            context.SaveChanges();

            // Act
            var patients = context.Patients.ToList();

            // Assert
            Assert.Equal(2, patients.Count);
        }

        [Fact]
        public void ClinicDbContext_CanUpdatePatient()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var patient = new Patient { Name = "Original", IsActive = true };
            context.Patients.Add(patient);
            context.SaveChanges();

            // Act
            patient.Name = "Updated";
            context.SaveChanges();

            // Assert
            var updated = context.Patients.Find(patient.Id);
            Assert.Equal("Updated", updated.Name);
        }

        [Fact]
        public void ClinicDbContext_CanDeletePatient()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var patient = new Patient { Name = "To Delete", IsActive = true };
            context.Patients.Add(patient);
            context.SaveChanges();

            // Act
            context.Patients.Remove(patient);
            context.SaveChanges();

            // Assert
            var deleted = context.Patients.Find(patient.Id);
            Assert.Null(deleted);
        }
    }
}
