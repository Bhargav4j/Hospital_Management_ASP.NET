using System;
using System.Threading.Tasks;
using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using ClinicManagement.Domain.Entities;
using ClinicManagement.Infrastructure.Data;
using ClinicManagement.Infrastructure.Repositories;

namespace ClinicManagement.Infrastructure.Repositories.Tests
{
    public class AppointmentRepositoryTests
    {
        private readonly Mock<ILogger<AppointmentRepository>> _mockLogger;

        public AppointmentRepositoryTests()
        {
            _mockLogger = new Mock<ILogger<AppointmentRepository>>();
        }

        private ClinicDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<ClinicDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            return new ClinicDbContext(options);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsActiveAppointments()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new AppointmentRepository(context, _mockLogger.Object);

            var patient = new Patient { Id = 1, Name = "Patient 1", IsActive = true };
            var doctor = new Doctor { Id = 1, Name = "Dr. Smith", IsActive = true };
            context.Patients.Add(patient);
            context.Doctors.Add(doctor);
            await context.SaveChangesAsync();

            context.Appointments.Add(new Appointment
            {
                Id = 1,
                PatientId = 1,
                DoctorId = 1,
                IsActive = true
            });
            context.Appointments.Add(new Appointment
            {
                Id = 2,
                PatientId = 1,
                DoctorId = 1,
                IsActive = false
            });
            await context.SaveChangesAsync();

            // Act
            var result = await repository.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
        }

        [Fact]
        public async Task GetByIdAsync_WithValidId_ReturnsAppointment()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new AppointmentRepository(context, _mockLogger.Object);

            var patient = new Patient { Id = 1, Name = "Test Patient", IsActive = true };
            var doctor = new Doctor { Id = 1, Name = "Test Doctor", IsActive = true };
            context.Patients.Add(patient);
            context.Doctors.Add(doctor);
            await context.SaveChangesAsync();

            context.Appointments.Add(new Appointment
            {
                Id = 1,
                PatientId = 1,
                DoctorId = 1,
                TimeSlot = "10:00 AM",
                IsActive = true
            });
            await context.SaveChangesAsync();

            // Act
            var result = await repository.GetByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("10:00 AM", result.TimeSlot);
        }

        [Fact]
        public async Task GetByIdAsync_WithInvalidId_ReturnsNull()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new AppointmentRepository(context, _mockLogger.Object);

            // Act
            var result = await repository.GetByIdAsync(999);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task AddAsync_AddsAppointmentToDatabase()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new AppointmentRepository(context, _mockLogger.Object);

            var patient = new Patient { Id = 1, IsActive = true };
            var doctor = new Doctor { Id = 1, IsActive = true };
            context.Patients.Add(patient);
            context.Doctors.Add(doctor);
            await context.SaveChangesAsync();

            var appointment = new Appointment
            {
                PatientId = 1,
                DoctorId = 1,
                TimeSlot = "2:00 PM"
            };

            // Act
            var result = await repository.AddAsync(appointment);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Id > 0);
        }

        [Fact]
        public async Task UpdateAsync_UpdatesAppointment()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new AppointmentRepository(context, _mockLogger.Object);

            var patient = new Patient { Id = 1, IsActive = true };
            var doctor = new Doctor { Id = 1, IsActive = true };
            context.Patients.Add(patient);
            context.Doctors.Add(doctor);
            await context.SaveChangesAsync();

            var appointment = new Appointment
            {
                PatientId = 1,
                DoctorId = 1,
                TimeSlot = "Original",
                IsActive = true
            };
            context.Appointments.Add(appointment);
            await context.SaveChangesAsync();

            // Act
            appointment.TimeSlot = "Updated";
            await repository.UpdateAsync(appointment);

            // Assert
            var updated = await context.Appointments.FindAsync(appointment.Id);
            Assert.Equal("Updated", updated.TimeSlot);
        }

        [Fact]
        public async Task DeleteAsync_SetsIsActiveToFalse()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new AppointmentRepository(context, _mockLogger.Object);

            var appointment = new Appointment { Id = 1, IsActive = true };
            context.Appointments.Add(appointment);
            await context.SaveChangesAsync();

            // Act
            await repository.DeleteAsync(1);

            // Assert
            var deleted = await context.Appointments.FindAsync(1);
            Assert.False(deleted.IsActive);
        }

        [Fact]
        public async Task ExistsAsync_WithExistingId_ReturnsTrue()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new AppointmentRepository(context, _mockLogger.Object);
            context.Appointments.Add(new Appointment { Id = 1, IsActive = true });
            await context.SaveChangesAsync();

            // Act
            var result = await repository.ExistsAsync(1);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task ExistsAsync_WithNonExistingId_ReturnsFalse()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new AppointmentRepository(context, _mockLogger.Object);

            // Act
            var result = await repository.ExistsAsync(999);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task GetByPatientIdAsync_ReturnsPatientAppointments()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new AppointmentRepository(context, _mockLogger.Object);

            var patient = new Patient { Id = 1, IsActive = true };
            var doctor = new Doctor { Id = 1, IsActive = true };
            context.Patients.Add(patient);
            context.Doctors.Add(doctor);
            await context.SaveChangesAsync();

            context.Appointments.Add(new Appointment
            {
                PatientId = 1,
                DoctorId = 1,
                IsActive = true
            });
            await context.SaveChangesAsync();

            // Act
            var result = await repository.GetByPatientIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
        }

        [Fact]
        public async Task GetByDoctorIdAsync_ReturnsDoctorAppointments()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new AppointmentRepository(context, _mockLogger.Object);

            var patient = new Patient { Id = 1, IsActive = true };
            var doctor = new Doctor { Id = 1, IsActive = true };
            context.Patients.Add(patient);
            context.Doctors.Add(doctor);
            await context.SaveChangesAsync();

            context.Appointments.Add(new Appointment
            {
                PatientId = 1,
                DoctorId = 1,
                IsActive = true
            });
            await context.SaveChangesAsync();

            // Act
            var result = await repository.GetByDoctorIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
        }

        [Fact]
        public async Task GetPendingAppointmentsAsync_ReturnsPendingAppointments()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new AppointmentRepository(context, _mockLogger.Object);

            var patient = new Patient { Id = 1, IsActive = true };
            var doctor = new Doctor { Id = 1, IsActive = true };
            context.Patients.Add(patient);
            context.Doctors.Add(doctor);
            await context.SaveChangesAsync();

            context.Appointments.Add(new Appointment
            {
                PatientId = 1,
                DoctorId = 1,
                Status = "Pending",
                IsActive = true
            });
            context.Appointments.Add(new Appointment
            {
                PatientId = 1,
                DoctorId = 1,
                Status = "Confirmed",
                IsActive = true
            });
            await context.SaveChangesAsync();

            // Act
            var result = await repository.GetPendingAppointmentsAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
        }
    }
}
