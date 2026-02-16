using Xunit;
using Microsoft.EntityFrameworkCore;
using ClinicManagement.Infrastructure.Data;
using ClinicManagement.Infrastructure.Repositories;
using ClinicManagement.Domain.Entities;
using System.Threading.Tasks;

namespace ClinicManagement.Infrastructure.Tests.Repositories
{
    public class RepositoryTests
    {
        private ClinicManagementDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<ClinicManagementDbContext>()
                .UseInMemoryDatabase(databaseName: System.Guid.NewGuid().ToString())
                .Options;

            return new ClinicManagementDbContext(options);
        }

        [Fact]
        public async Task GetByIdAsync_WithValidId_ReturnsEntity()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var repository = new Repository<Patient>(context);
            var patient = new Patient { Name = "Test Patient", Email = "test@test.com" };
            await context.Patients.AddAsync(patient);
            await context.SaveChangesAsync();

            // Act
            var result = await repository.GetByIdAsync(patient.PatientID);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(patient.PatientID, result.PatientID);
        }

        [Fact]
        public async Task GetByIdAsync_WithInvalidId_ReturnsNull()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var repository = new Repository<Patient>(context);

            // Act
            var result = await repository.GetByIdAsync(999);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllEntities()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var repository = new Repository<Patient>(context);
            await context.Patients.AddAsync(new Patient { Name = "Patient1", Email = "p1@test.com" });
            await context.Patients.AddAsync(new Patient { Name = "Patient2", Email = "p2@test.com" });
            await context.SaveChangesAsync();

            // Act
            var result = await repository.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, ((System.Collections.Generic.List<Patient>)result).Count);
        }

        [Fact]
        public async Task AddAsync_AddsEntitySuccessfully()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var repository = new Repository<Patient>(context);
            var patient = new Patient { Name = "New Patient", Email = "new@test.com" };

            // Act
            var result = await repository.AddAsync(patient);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.PatientID > 0);
        }

        [Fact]
        public async Task UpdateAsync_UpdatesEntitySuccessfully()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var repository = new Repository<Patient>(context);
            var patient = new Patient { Name = "Original Name", Email = "test@test.com" };
            await context.Patients.AddAsync(patient);
            await context.SaveChangesAsync();

            // Act
            patient.Name = "Updated Name";
            await repository.UpdateAsync(patient);

            // Assert
            var updated = await context.Patients.FindAsync(patient.PatientID);
            Assert.Equal("Updated Name", updated?.Name);
        }

        [Fact]
        public async Task DeleteAsync_DeletesEntitySuccessfully()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var repository = new Repository<Patient>(context);
            var patient = new Patient { Name = "To Delete", Email = "delete@test.com" };
            await context.Patients.AddAsync(patient);
            await context.SaveChangesAsync();
            var patientId = patient.PatientID;

            // Act
            await repository.DeleteAsync(patientId);

            // Assert
            var deleted = await context.Patients.FindAsync(patientId);
            Assert.Null(deleted);
        }

        [Fact]
        public async Task ExistsAsync_WithExistingEntity_ReturnsTrue()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var repository = new Repository<Patient>(context);
            var patient = new Patient { Name = "Exists", Email = "exists@test.com" };
            await context.Patients.AddAsync(patient);
            await context.SaveChangesAsync();

            // Act
            var result = await repository.ExistsAsync(patient.PatientID);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task ExistsAsync_WithNonExistingEntity_ReturnsFalse()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var repository = new Repository<Patient>(context);

            // Act
            var result = await repository.ExistsAsync(999);

            // Assert
            Assert.False(result);
        }
    }
}
