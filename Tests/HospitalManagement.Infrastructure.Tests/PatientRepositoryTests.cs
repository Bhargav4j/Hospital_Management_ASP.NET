using Xunit;
using Moq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using HospitalManagement.Domain.Entities;
using HospitalManagement.Infrastructure.Repositories;
using HospitalManagement.Infrastructure.Data;

namespace HospitalManagement.Infrastructure.Repositories.Tests;

public class PatientRepositoryTests
{
    private readonly Mock<ILogger<PatientRepository>> _mockLogger;
    private readonly DbContextOptions<HospitalDbContext> _options;

    public PatientRepositoryTests()
    {
        _mockLogger = new Mock<ILogger<PatientRepository>>();
        _options = new DbContextOptionsBuilder<HospitalDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
    }

    [Fact]
    public void PatientRepository_Constructor_WithNullContext_ThrowsArgumentNullException()
    {
        // Arrange & Act & Assert
        Assert.Throws<ArgumentNullException>(() => new PatientRepository(null!, _mockLogger.Object));
    }

    [Fact]
    public void PatientRepository_Constructor_WithNullLogger_ThrowsArgumentNullException()
    {
        // Arrange
        using var context = new HospitalDbContext(_options);

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new PatientRepository(context, null!));
    }

    [Fact]
    public void PatientRepository_Constructor_WithValidParameters_CreatesInstance()
    {
        // Arrange
        using var context = new HospitalDbContext(_options);

        // Act
        var repository = new PatientRepository(context, _mockLogger.Object);

        // Assert
        Assert.NotNull(repository);
    }

    [Fact]
    public async Task GetAllAsync_WithNoPatients_ReturnsEmptyList()
    {
        // Arrange
        using var context = new HospitalDbContext(_options);
        var repository = new PatientRepository(context, _mockLogger.Object);

        // Act
        var result = await repository.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetAllAsync_WithActivePatients_ReturnsActivePatientsOnly()
    {
        // Arrange
        using var context = new HospitalDbContext(_options);
        var repository = new PatientRepository(context, _mockLogger.Object);

        var activePatient = new Patient { Id = 1, FirstName = "Alice", LastName = "Johnson", Email = "alice@test.com", IsActive = true };
        var inactivePatient = new Patient { Id = 2, FirstName = "Bob", LastName = "Smith", Email = "bob@test.com", IsActive = false };

        context.Patients.AddRange(activePatient, inactivePatient);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.GetAllAsync();

        // Assert
        Assert.Single(result);
        Assert.Contains(result, p => p.Id == 1);
    }

    [Fact]
    public async Task GetByIdAsync_WithNonExistentId_ReturnsNull()
    {
        // Arrange
        using var context = new HospitalDbContext(_options);
        var repository = new PatientRepository(context, _mockLogger.Object);

        // Act
        var result = await repository.GetByIdAsync(999);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetByIdAsync_WithExistingId_ReturnsPatient()
    {
        // Arrange
        using var context = new HospitalDbContext(_options);
        var repository = new PatientRepository(context, _mockLogger.Object);

        var patient = new Patient { Id = 1, FirstName = "Charlie", LastName = "Brown", Email = "charlie@test.com", IsActive = true };
        context.Patients.Add(patient);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.GetByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("Charlie", result.FirstName);
    }

    [Fact]
    public async Task GetByIdAsync_WithInactivePatient_ReturnsNull()
    {
        // Arrange
        using var context = new HospitalDbContext(_options);
        var repository = new PatientRepository(context, _mockLogger.Object);

        var patient = new Patient { Id = 1, FirstName = "Diana", LastName = "Prince", Email = "diana@test.com", IsActive = false };
        context.Patients.Add(patient);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.GetByIdAsync(1);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task AddAsync_WithValidPatient_AddsToDatabase()
    {
        // Arrange
        using var context = new HospitalDbContext(_options);
        var repository = new PatientRepository(context, _mockLogger.Object);

        var patient = new Patient { FirstName = "Emma", LastName = "Watson", Email = "emma@test.com" };

        // Act
        var result = await repository.AddAsync(patient);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Id > 0);
        Assert.True(result.IsActive);
        Assert.NotEqual(default(DateTime), result.CreatedDate);
    }

    [Fact]
    public async Task AddAsync_SetsCreatedDateAndIsActive()
    {
        // Arrange
        using var context = new HospitalDbContext(_options);
        var repository = new PatientRepository(context, _mockLogger.Object);

        var patient = new Patient { FirstName = "Frank", LastName = "Miller", Email = "frank@test.com" };

        // Act
        var result = await repository.AddAsync(patient);

        // Assert
        Assert.True(result.IsActive);
        Assert.True((DateTime.UtcNow - result.CreatedDate).TotalSeconds < 5);
    }

    [Fact]
    public async Task UpdateAsync_WithValidPatient_UpdatesDatabase()
    {
        // Arrange
        using var context = new HospitalDbContext(_options);
        var repository = new PatientRepository(context, _mockLogger.Object);

        var patient = new Patient { FirstName = "Grace", LastName = "Hopper", Email = "grace@test.com", IsActive = true };
        context.Patients.Add(patient);
        await context.SaveChangesAsync();

        // Act
        patient.FirstName = "Gracie";
        await repository.UpdateAsync(patient);

        var updated = await context.Patients.FindAsync(patient.Id);

        // Assert
        Assert.NotNull(updated);
        Assert.Equal("Gracie", updated.FirstName);
        Assert.NotNull(updated.ModifiedDate);
    }

    [Fact]
    public async Task UpdateAsync_SetsModifiedDate()
    {
        // Arrange
        using var context = new HospitalDbContext(_options);
        var repository = new PatientRepository(context, _mockLogger.Object);

        var patient = new Patient { FirstName = "Henry", LastName = "Ford", Email = "henry@test.com", IsActive = true };
        context.Patients.Add(patient);
        await context.SaveChangesAsync();

        // Act
        patient.Email = "henry.new@test.com";
        await repository.UpdateAsync(patient);

        var updated = await context.Patients.FindAsync(patient.Id);

        // Assert
        Assert.NotNull(updated);
        Assert.NotNull(updated.ModifiedDate);
        Assert.True((DateTime.UtcNow - updated.ModifiedDate.Value).TotalSeconds < 5);
    }

    [Fact]
    public async Task DeleteAsync_WithExistingId_SoftDeletesPatient()
    {
        // Arrange
        using var context = new HospitalDbContext(_options);
        var repository = new PatientRepository(context, _mockLogger.Object);

        var patient = new Patient { FirstName = "Ivy", LastName = "Green", Email = "ivy@test.com", IsActive = true };
        context.Patients.Add(patient);
        await context.SaveChangesAsync();

        // Act
        await repository.DeleteAsync(patient.Id);

        var deleted = await context.Patients.FindAsync(patient.Id);

        // Assert
        Assert.NotNull(deleted);
        Assert.False(deleted.IsActive);
        Assert.NotNull(deleted.ModifiedDate);
    }

    [Fact]
    public async Task DeleteAsync_WithNonExistentId_DoesNotThrow()
    {
        // Arrange
        using var context = new HospitalDbContext(_options);
        var repository = new PatientRepository(context, _mockLogger.Object);

        // Act & Assert
        await repository.DeleteAsync(999);
    }

    [Fact]
    public async Task ExistsAsync_WithExistingId_ReturnsTrue()
    {
        // Arrange
        using var context = new HospitalDbContext(_options);
        var repository = new PatientRepository(context, _mockLogger.Object);

        var patient = new Patient { FirstName = "Jack", LastName = "Black", Email = "jack@test.com", IsActive = true };
        context.Patients.Add(patient);
        await context.SaveChangesAsync();

        // Act
        var exists = await repository.ExistsAsync(patient.Id);

        // Assert
        Assert.True(exists);
    }

    [Fact]
    public async Task ExistsAsync_WithNonExistentId_ReturnsFalse()
    {
        // Arrange
        using var context = new HospitalDbContext(_options);
        var repository = new PatientRepository(context, _mockLogger.Object);

        // Act
        var exists = await repository.ExistsAsync(999);

        // Assert
        Assert.False(exists);
    }

    [Fact]
    public async Task ExistsAsync_WithInactivePatient_ReturnsFalse()
    {
        // Arrange
        using var context = new HospitalDbContext(_options);
        var repository = new PatientRepository(context, _mockLogger.Object);

        var patient = new Patient { FirstName = "Karen", LastName = "White", Email = "karen@test.com", IsActive = false };
        context.Patients.Add(patient);
        await context.SaveChangesAsync();

        // Act
        var exists = await repository.ExistsAsync(patient.Id);

        // Assert
        Assert.False(exists);
    }

    [Fact]
    public async Task EmailExistsAsync_WithExistingEmail_ReturnsTrue()
    {
        // Arrange
        using var context = new HospitalDbContext(_options);
        var repository = new PatientRepository(context, _mockLogger.Object);

        var patient = new Patient { FirstName = "Larry", LastName = "Page", Email = "larry@test.com", IsActive = true };
        context.Patients.Add(patient);
        await context.SaveChangesAsync();

        // Act
        var exists = await repository.EmailExistsAsync("larry@test.com");

        // Assert
        Assert.True(exists);
    }

    [Fact]
    public async Task EmailExistsAsync_WithNonExistentEmail_ReturnsFalse()
    {
        // Arrange
        using var context = new HospitalDbContext(_options);
        var repository = new PatientRepository(context, _mockLogger.Object);

        // Act
        var exists = await repository.EmailExistsAsync("nonexistent@test.com");

        // Assert
        Assert.False(exists);
    }

    [Fact]
    public async Task SearchAsync_WithMatchingTerm_ReturnsPatients()
    {
        // Arrange
        using var context = new HospitalDbContext(_options);
        var repository = new PatientRepository(context, _mockLogger.Object);

        var patient1 = new Patient { FirstName = "Mary", LastName = "Johnson", Email = "mary.johnson@test.com", IsActive = true };
        var patient2 = new Patient { FirstName = "Mark", LastName = "Smith", Email = "mark.smith@test.com", IsActive = true };
        context.Patients.AddRange(patient1, patient2);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.SearchAsync("Mary");

        // Assert
        Assert.Single(result);
        Assert.Contains(result, p => p.FirstName == "Mary");
    }

    [Fact]
    public async Task SearchAsync_WithNoMatches_ReturnsEmptyList()
    {
        // Arrange
        using var context = new HospitalDbContext(_options);
        var repository = new PatientRepository(context, _mockLogger.Object);

        var patient = new Patient { FirstName = "Nancy", LastName = "Drew", Email = "nancy@test.com", IsActive = true };
        context.Patients.Add(patient);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.SearchAsync("NotFound");

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task SearchAsync_WithEmailMatch_ReturnsPatient()
    {
        // Arrange
        using var context = new HospitalDbContext(_options);
        var repository = new PatientRepository(context, _mockLogger.Object);

        var patient = new Patient { FirstName = "Oliver", LastName = "Twist", Email = "oliver@test.com", IsActive = true };
        context.Patients.Add(patient);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.SearchAsync("oliver@test.com");

        // Assert
        Assert.Single(result);
        Assert.Contains(result, p => p.Email == "oliver@test.com");
    }

    [Fact]
    public async Task SearchAsync_WithLastNameMatch_ReturnsPatient()
    {
        // Arrange
        using var context = new HospitalDbContext(_options);
        var repository = new PatientRepository(context, _mockLogger.Object);

        var patient = new Patient { FirstName = "Peter", LastName = "Parker", Email = "peter@test.com", IsActive = true };
        context.Patients.Add(patient);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.SearchAsync("Parker");

        // Assert
        Assert.Single(result);
        Assert.Contains(result, p => p.LastName == "Parker");
    }

    [Fact]
    public async Task GetAllAsync_WithMultipleActivePatients_ReturnsAll()
    {
        // Arrange
        using var context = new HospitalDbContext(_options);
        var repository = new PatientRepository(context, _mockLogger.Object);

        var patient1 = new Patient { FirstName = "Quinn", LastName = "Adams", Email = "quinn@test.com", IsActive = true };
        var patient2 = new Patient { FirstName = "Rachel", LastName = "Green", Email = "rachel@test.com", IsActive = true };
        var patient3 = new Patient { FirstName = "Steve", LastName = "Rogers", Email = "steve@test.com", IsActive = true };

        context.Patients.AddRange(patient1, patient2, patient3);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.GetAllAsync();

        // Assert
        Assert.Equal(3, result.Count());
    }
}
