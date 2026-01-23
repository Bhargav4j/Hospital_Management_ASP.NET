using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using ClinicManagement.Domain.Entities;
using ClinicManagement.Infrastructure.Data;
using ClinicManagement.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ClinicManagement.Infrastructure.Repositories.Tests;

/// <summary>
/// Unit tests for PatientRepository
/// </summary>
public class PatientRepositoryTests
{
    private readonly Mock<ILogger<PatientRepository>> _mockLogger;

    public PatientRepositoryTests()
    {
        _mockLogger = new Mock<ILogger<PatientRepository>>();
    }

    private ClinicDbContext CreateInMemoryContext()
    {
        var options = new DbContextOptionsBuilder<ClinicDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        return new ClinicDbContext(options);
    }

    [Fact]
    public void PatientRepository_Constructor_WithNullContext_ShouldThrowArgumentNullException()
    {
        // Arrange, Act & Assert
        Assert.Throws<ArgumentNullException>(() => new PatientRepository(null!, _mockLogger.Object));
    }

    [Fact]
    public void PatientRepository_Constructor_WithNullLogger_ShouldThrowArgumentNullException()
    {
        // Arrange
        using var context = CreateInMemoryContext();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new PatientRepository(context, null!));
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllActivePatients()
    {
        // Arrange
        using var context = CreateInMemoryContext();
        var repository = new PatientRepository(context, _mockLogger.Object);

        var patients = new List<Patient>
        {
            new Patient { Id = 1, Name = "Patient 1", Email = "p1@test.com", IsActive = true },
            new Patient { Id = 2, Name = "Patient 2", Email = "p2@test.com", IsActive = true },
            new Patient { Id = 3, Name = "Patient 3", Email = "p3@test.com", IsActive = false }
        };

        await context.Patients.AddRangeAsync(patients);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.GetAllAsync();

        // Assert
        Assert.Equal(2, result.Count());
        Assert.All(result, p => Assert.True(p.IsActive));
    }

    [Fact]
    public async Task GetByIdAsync_WithValidId_ShouldReturnPatient()
    {
        // Arrange
        using var context = CreateInMemoryContext();
        var repository = new PatientRepository(context, _mockLogger.Object);

        var patient = new Patient { Id = 1, Name = "Test Patient", Email = "test@test.com", IsActive = true };
        await context.Patients.AddAsync(patient);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.GetByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Test Patient", result.Name);
    }

    [Fact]
    public async Task GetByIdAsync_WithInvalidId_ShouldReturnNull()
    {
        // Arrange
        using var context = CreateInMemoryContext();
        var repository = new PatientRepository(context, _mockLogger.Object);

        // Act
        var result = await repository.GetByIdAsync(999);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetByEmailAsync_WithValidEmail_ShouldReturnPatient()
    {
        // Arrange
        using var context = CreateInMemoryContext();
        var repository = new PatientRepository(context, _mockLogger.Object);

        var patient = new Patient { Id = 1, Name = "Test Patient", Email = "test@test.com", IsActive = true };
        await context.Patients.AddAsync(patient);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.GetByEmailAsync("test@test.com");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("test@test.com", result.Email);
    }

    [Fact]
    public async Task GetByEmailAsync_WithInvalidEmail_ShouldReturnNull()
    {
        // Arrange
        using var context = CreateInMemoryContext();
        var repository = new PatientRepository(context, _mockLogger.Object);

        // Act
        var result = await repository.GetByEmailAsync("invalid@test.com");

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task AddAsync_ShouldAddPatientAndSetDefaults()
    {
        // Arrange
        using var context = CreateInMemoryContext();
        var repository = new PatientRepository(context, _mockLogger.Object);

        var patient = new Patient { Name = "New Patient", Email = "new@test.com" };

        // Act
        var result = await repository.AddAsync(patient);

        // Assert
        Assert.NotEqual(0, result.Id);
        Assert.True(result.IsActive);
        Assert.NotEqual(default, result.CreatedDate);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdatePatientAndSetModifiedDate()
    {
        // Arrange
        using var context = CreateInMemoryContext();
        var repository = new PatientRepository(context, _mockLogger.Object);

        var patient = new Patient { Id = 1, Name = "Original", Email = "original@test.com", IsActive = true };
        await context.Patients.AddAsync(patient);
        await context.SaveChangesAsync();

        // Act
        patient.Name = "Updated";
        await repository.UpdateAsync(patient);

        // Assert
        var updated = await context.Patients.FindAsync(1);
        Assert.NotNull(updated);
        Assert.Equal("Updated", updated.Name);
        Assert.NotNull(updated.ModifiedDate);
    }

    [Fact]
    public async Task DeleteAsync_ShouldSoftDeletePatient()
    {
        // Arrange
        using var context = CreateInMemoryContext();
        var repository = new PatientRepository(context, _mockLogger.Object);

        var patient = new Patient { Id = 1, Name = "Test", Email = "test@test.com", IsActive = true };
        await context.Patients.AddAsync(patient);
        await context.SaveChangesAsync();

        // Act
        await repository.DeleteAsync(1);

        // Assert
        var deleted = await context.Patients.FindAsync(1);
        Assert.NotNull(deleted);
        Assert.False(deleted.IsActive);
        Assert.NotNull(deleted.ModifiedDate);
    }

    [Fact]
    public async Task ExistsAsync_WithExistingId_ShouldReturnTrue()
    {
        // Arrange
        using var context = CreateInMemoryContext();
        var repository = new PatientRepository(context, _mockLogger.Object);

        var patient = new Patient { Id = 1, Name = "Test", Email = "test@test.com", IsActive = true };
        await context.Patients.AddAsync(patient);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.ExistsAsync(1);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task ExistsAsync_WithNonExistingId_ShouldReturnFalse()
    {
        // Arrange
        using var context = CreateInMemoryContext();
        var repository = new PatientRepository(context, _mockLogger.Object);

        // Act
        var result = await repository.ExistsAsync(999);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task SearchAsync_WithEmptyTerm_ShouldReturnAllActivePatients()
    {
        // Arrange
        using var context = CreateInMemoryContext();
        var repository = new PatientRepository(context, _mockLogger.Object);

        var patients = new List<Patient>
        {
            new Patient { Id = 1, Name = "Patient 1", Email = "p1@test.com", IsActive = true },
            new Patient { Id = 2, Name = "Patient 2", Email = "p2@test.com", IsActive = true }
        };

        await context.Patients.AddRangeAsync(patients);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.SearchAsync("");

        // Assert
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task SearchAsync_WithMatchingTerm_ShouldReturnMatchingPatients()
    {
        // Arrange
        using var context = CreateInMemoryContext();
        var repository = new PatientRepository(context, _mockLogger.Object);

        var patients = new List<Patient>
        {
            new Patient { Id = 1, Name = "John Doe", Email = "john@test.com", PhoneNo = "1234567890", IsActive = true },
            new Patient { Id = 2, Name = "Jane Smith", Email = "jane@test.com", PhoneNo = "0987654321", IsActive = true }
        };

        await context.Patients.AddRangeAsync(patients);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.SearchAsync("john");

        // Assert
        Assert.Single(result);
        Assert.Equal("John Doe", result.First().Name);
    }
}
