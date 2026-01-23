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
    public class DoctorRepositoryTests
    {
        private readonly Mock<ILogger<DoctorRepository>> _mockLogger;

        public DoctorRepositoryTests()
        {
            _mockLogger = new Mock<ILogger<DoctorRepository>>();
        }

        private ClinicDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<ClinicDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            return new ClinicDbContext(options);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsActiveDoctors()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new DoctorRepository(context, _mockLogger.Object);
            context.Doctors.Add(new Doctor { Id = 1, Name = "Dr. Smith", IsActive = true });
            context.Doctors.Add(new Doctor { Id = 2, Name = "Dr. Inactive", IsActive = false });
            await context.SaveChangesAsync();

            // Act
            var result = await repository.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
        }

        [Fact]
        public async Task GetByIdAsync_WithValidId_ReturnsDoctor()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new DoctorRepository(context, _mockLogger.Object);
            context.Doctors.Add(new Doctor { Id = 1, Name = "Dr. Johnson", IsActive = true });
            await context.SaveChangesAsync();

            // Act
            var result = await repository.GetByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Dr. Johnson", result.Name);
        }

        [Fact]
        public async Task GetByIdAsync_WithInvalidId_ReturnsNull()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new DoctorRepository(context, _mockLogger.Object);

            // Act
            var result = await repository.GetByIdAsync(999);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetByEmailAsync_WithValidEmail_ReturnsDoctor()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new DoctorRepository(context, _mockLogger.Object);
            context.Doctors.Add(new Doctor { Id = 1, Email = "doctor@hospital.com", IsActive = true });
            await context.SaveChangesAsync();

            // Act
            var result = await repository.GetByEmailAsync("doctor@hospital.com");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("doctor@hospital.com", result.Email);
        }

        [Fact]
        public async Task AddAsync_AddsDoctorToDatabase()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new DoctorRepository(context, _mockLogger.Object);
            var doctor = new Doctor { Name = "Dr. New", Email = "new@hospital.com" };

            // Act
            var result = await repository.AddAsync(doctor);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Id > 0);
        }

        [Fact]
        public async Task UpdateAsync_UpdatesDoctor()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new DoctorRepository(context, _mockLogger.Object);
            var doctor = new Doctor { Name = "Original Doctor", IsActive = true };
            context.Doctors.Add(doctor);
            await context.SaveChangesAsync();

            // Act
            doctor.Name = "Updated Doctor";
            await repository.UpdateAsync(doctor);

            // Assert
            var updated = await context.Doctors.FindAsync(doctor.Id);
            Assert.Equal("Updated Doctor", updated.Name);
        }

        [Fact]
        public async Task DeleteAsync_SetsIsActiveToFalse()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new DoctorRepository(context, _mockLogger.Object);
            var doctor = new Doctor { Id = 1, Name = "To Delete", IsActive = true };
            context.Doctors.Add(doctor);
            await context.SaveChangesAsync();

            // Act
            await repository.DeleteAsync(1);

            // Assert
            var deleted = await context.Doctors.FindAsync(1);
            Assert.False(deleted.IsActive);
        }

        [Fact]
        public async Task ExistsAsync_WithExistingId_ReturnsTrue()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new DoctorRepository(context, _mockLogger.Object);
            context.Doctors.Add(new Doctor { Id = 1, IsActive = true });
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
            var repository = new DoctorRepository(context, _mockLogger.Object);

            // Act
            var result = await repository.ExistsAsync(999);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task SearchAsync_ReturnsMatchingDoctors()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new DoctorRepository(context, _mockLogger.Object);
            context.Doctors.Add(new Doctor { Id = 1, Name = "Dr. Smith", IsActive = true });
            context.Doctors.Add(new Doctor { Id = 2, Name = "Dr. Jones", IsActive = true });
            await context.SaveChangesAsync();

            // Act
            var result = await repository.SearchAsync("Smith");

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
        }

        [Fact]
        public async Task GetBySpecializationAsync_ReturnsMatchingDoctors()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new DoctorRepository(context, _mockLogger.Object);
            context.Doctors.Add(new Doctor
            {
                Id = 1,
                Name = "Dr. Heart",
                Specialization = "Cardiology",
                IsActive = true
            });
            context.Doctors.Add(new Doctor
            {
                Id = 2,
                Name = "Dr. Brain",
                Specialization = "Neurology",
                IsActive = true
            });
            await context.SaveChangesAsync();

            // Act
            var result = await repository.GetBySpecializationAsync("Cardiology");

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
        }
    }
}
