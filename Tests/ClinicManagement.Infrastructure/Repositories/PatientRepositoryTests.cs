using System;
using System.Threading;
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
    public class PatientRepositoryTests
    {
        private readonly Mock<ILogger<PatientRepository>> _mockLogger;

        public PatientRepositoryTests()
        {
            _mockLogger = new Mock<ILogger<PatientRepository>>();
        }

        private ClinicDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<ClinicDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            return new ClinicDbContext(options);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsActivePatients()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new PatientRepository(context, _mockLogger.Object);
            context.Patients.Add(new Patient { Id = 1, Name = "Patient 1", IsActive = true });
            context.Patients.Add(new Patient { Id = 2, Name = "Patient 2", IsActive = false });
            await context.SaveChangesAsync();

            // Act
            var result = await repository.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
        }

        [Fact]
        public async Task GetByIdAsync_WithValidId_ReturnsPatient()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new PatientRepository(context, _mockLogger.Object);
            context.Patients.Add(new Patient { Id = 1, Name = "Test Patient", IsActive = true });
            await context.SaveChangesAsync();

            // Act
            var result = await repository.GetByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Test Patient", result.Name);
        }

        [Fact]
        public async Task GetByIdAsync_WithInvalidId_ReturnsNull()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new PatientRepository(context, _mockLogger.Object);

            // Act
            var result = await repository.GetByIdAsync(999);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetByEmailAsync_WithValidEmail_ReturnsPatient()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new PatientRepository(context, _mockLogger.Object);
            context.Patients.Add(new Patient { Id = 1, Email = "test@example.com", IsActive = true });
            await context.SaveChangesAsync();

            // Act
            var result = await repository.GetByEmailAsync("test@example.com");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("test@example.com", result.Email);
        }

        [Fact]
        public async Task AddAsync_AddsPatientToDatabase()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new PatientRepository(context, _mockLogger.Object);
            var patient = new Patient { Name = "New Patient", Email = "new@test.com" };

            // Act
            var result = await repository.AddAsync(patient);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Id > 0);
        }

        [Fact]
        public async Task UpdateAsync_UpdatesPatient()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new PatientRepository(context, _mockLogger.Object);
            var patient = new Patient { Name = "Original Name", IsActive = true };
            context.Patients.Add(patient);
            await context.SaveChangesAsync();

            // Act
            patient.Name = "Updated Name";
            await repository.UpdateAsync(patient);

            // Assert
            var updated = await context.Patients.FindAsync(patient.Id);
            Assert.Equal("Updated Name", updated.Name);
        }

        [Fact]
        public async Task DeleteAsync_SetsIsActiveToFalse()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new PatientRepository(context, _mockLogger.Object);
            var patient = new Patient { Id = 1, Name = "To Delete", IsActive = true };
            context.Patients.Add(patient);
            await context.SaveChangesAsync();

            // Act
            await repository.DeleteAsync(1);

            // Assert
            var deleted = await context.Patients.FindAsync(1);
            Assert.False(deleted.IsActive);
        }

        [Fact]
        public async Task ExistsAsync_WithExistingId_ReturnsTrue()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new PatientRepository(context, _mockLogger.Object);
            context.Patients.Add(new Patient { Id = 1, IsActive = true });
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
            var repository = new PatientRepository(context, _mockLogger.Object);

            // Act
            var result = await repository.ExistsAsync(999);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task SearchAsync_ReturnsMatchingPatients()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new PatientRepository(context, _mockLogger.Object);
            context.Patients.Add(new Patient { Id = 1, Name = "John Doe", IsActive = true });
            context.Patients.Add(new Patient { Id = 2, Name = "Jane Smith", IsActive = true });
            await context.SaveChangesAsync();

            // Act
            var result = await repository.SearchAsync("John");

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
        }

        [Fact]
        public async Task ValidateLoginAsync_WithCorrectCredentials_ReturnsPatient()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new PatientRepository(context, _mockLogger.Object);
            context.Patients.Add(new Patient
            {
                Id = 1,
                Email = "login@test.com",
                Password = "password123",
                IsActive = true
            });
            await context.SaveChangesAsync();

            // Act
            var result = await repository.ValidateLoginAsync("login@test.com", "password123");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("login@test.com", result.Email);
        }

        [Fact]
        public async Task ValidateLoginAsync_WithIncorrectCredentials_ReturnsNull()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new PatientRepository(context, _mockLogger.Object);

            // Act
            var result = await repository.ValidateLoginAsync("wrong@test.com", "wrongpass");

            // Assert
            Assert.Null(result);
        }
    }
}
