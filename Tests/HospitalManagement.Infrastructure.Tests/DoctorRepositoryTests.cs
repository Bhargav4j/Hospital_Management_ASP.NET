using Xunit;
using Moq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using HospitalManagement.Domain.Entities;
using HospitalManagement.Infrastructure.Repositories;
using HospitalManagement.Infrastructure.Data;

namespace HospitalManagement.Infrastructure.Repositories.Tests;

public class DoctorRepositoryTests
{
    private readonly Mock<ILogger<DoctorRepository>> _mockLogger;
    private readonly DbContextOptions<HospitalDbContext> _options;

    public DoctorRepositoryTests()
    {
        _mockLogger = new Mock<ILogger<DoctorRepository>>();
        _options = new DbContextOptionsBuilder<HospitalDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
    }

    [Fact]
    public void DoctorRepository_Constructor_WithNullContext_ThrowsArgumentNullException()
    {
        // Arrange & Act & Assert
        Assert.Throws<ArgumentNullException>(() => new DoctorRepository(null!, _mockLogger.Object));
    }

    [Fact]
    public void DoctorRepository_Constructor_WithNullLogger_ThrowsArgumentNullException()
    {
        // Arrange
        using var context = new HospitalDbContext(_options);

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new DoctorRepository(context, null!));
    }

    [Fact]
    public void DoctorRepository_Constructor_WithValidParameters_CreatesInstance()
    {
        // Arrange
        using var context = new HospitalDbContext(_options);

        // Act
        var repository = new DoctorRepository(context, _mockLogger.Object);

        // Assert
        Assert.NotNull(repository);
    }

    [Fact]
    public async Task GetAllAsync_WithNoDoctors_ReturnsEmptyList()
    {
        // Arrange
        using var context = new HospitalDbContext(_options);
        var repository = new DoctorRepository(context, _mockLogger.Object);

        // Act
        var result = await repository.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetAllAsync_WithActiveDoctors_ReturnsActiveDoctorsOnly()
    {
        // Arrange
        using var context = new HospitalDbContext(_options);
        var repository = new DoctorRepository(context, _mockLogger.Object);

        var activeDoctor = new Doctor { Id = 1, FirstName = "John", LastName = "Doe", Email = "john@test.com", IsActive = true };
        var inactiveDoctor = new Doctor { Id = 2, FirstName = "Jane", LastName = "Smith", Email = "jane@test.com", IsActive = false };

        context.Doctors.AddRange(activeDoctor, inactiveDoctor);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.GetAllAsync();

        // Assert
        Assert.Single(result);
        Assert.Contains(result, d => d.Id == 1);
    }

    [Fact]
    public async Task GetByIdAsync_WithNonExistentId_ReturnsNull()
    {
        // Arrange
        using var context = new HospitalDbContext(_options);
        var repository = new DoctorRepository(context, _mockLogger.Object);

        // Act
        var result = await repository.GetByIdAsync(999);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetByIdAsync_WithExistingId_ReturnsDoctor()
    {
        // Arrange
        using var context = new HospitalDbContext(_options);
        var repository = new DoctorRepository(context, _mockLogger.Object);

        var doctor = new Doctor { Id = 1, FirstName = "John", LastName = "Doe", Email = "john@test.com", IsActive = true };
        context.Doctors.Add(doctor);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.GetByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("John", result.FirstName);
    }

    [Fact]
    public async Task GetByIdAsync_WithInactiveDoctor_ReturnsNull()
    {
        // Arrange
        using var context = new HospitalDbContext(_options);
        var repository = new DoctorRepository(context, _mockLogger.Object);

        var doctor = new Doctor { Id = 1, FirstName = "John", LastName = "Doe", Email = "john@test.com", IsActive = false };
        context.Doctors.Add(doctor);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.GetByIdAsync(1);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task AddAsync_WithValidDoctor_AddsToDatabase()
    {
        // Arrange
        using var context = new HospitalDbContext(_options);
        var repository = new DoctorRepository(context, _mockLogger.Object);

        var doctor = new Doctor { FirstName = "Alice", LastName = "Brown", Email = "alice@test.com" };

        // Act
        var result = await repository.AddAsync(doctor);

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
        var repository = new DoctorRepository(context, _mockLogger.Object);

        var doctor = new Doctor { FirstName = "Bob", LastName = "Green", Email = "bob@test.com" };

        // Act
        var result = await repository.AddAsync(doctor);

        // Assert
        Assert.True(result.IsActive);
        Assert.True((DateTime.UtcNow - result.CreatedDate).TotalSeconds < 5);
    }

    [Fact]
    public async Task UpdateAsync_WithValidDoctor_UpdatesDatabase()
    {
        // Arrange
        using var context = new HospitalDbContext(_options);
        var repository = new DoctorRepository(context, _mockLogger.Object);

        var doctor = new Doctor { FirstName = "Charlie", LastName = "White", Email = "charlie@test.com", IsActive = true };
        context.Doctors.Add(doctor);
        await context.SaveChangesAsync();

        // Act
        doctor.FirstName = "Charles";
        await repository.UpdateAsync(doctor);

        var updated = await context.Doctors.FindAsync(doctor.Id);

        // Assert
        Assert.NotNull(updated);
        Assert.Equal("Charles", updated.FirstName);
        Assert.NotNull(updated.ModifiedDate);
    }

    [Fact]
    public async Task UpdateAsync_SetsModifiedDate()
    {
        // Arrange
        using var context = new HospitalDbContext(_options);
        var repository = new DoctorRepository(context, _mockLogger.Object);

        var doctor = new Doctor { FirstName = "David", LastName = "Black", Email = "david@test.com", IsActive = true };
        context.Doctors.Add(doctor);
        await context.SaveChangesAsync();

        // Act
        doctor.Email = "david.new@test.com";
        await repository.UpdateAsync(doctor);

        var updated = await context.Doctors.FindAsync(doctor.Id);

        // Assert
        Assert.NotNull(updated);
        Assert.NotNull(updated.ModifiedDate);
        Assert.True((DateTime.UtcNow - updated.ModifiedDate.Value).TotalSeconds < 5);
    }

    [Fact]
    public async Task DeleteAsync_WithExistingId_SoftDeletesDoctor()
    {
        // Arrange
        using var context = new HospitalDbContext(_options);
        var repository = new DoctorRepository(context, _mockLogger.Object);

        var doctor = new Doctor { FirstName = "Eve", LastName = "Red", Email = "eve@test.com", IsActive = true };
        context.Doctors.Add(doctor);
        await context.SaveChangesAsync();

        // Act
        await repository.DeleteAsync(doctor.Id);

        var deleted = await context.Doctors.FindAsync(doctor.Id);

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
        var repository = new DoctorRepository(context, _mockLogger.Object);

        // Act & Assert
        await repository.DeleteAsync(999);
    }

    [Fact]
    public async Task ExistsAsync_WithExistingId_ReturnsTrue()
    {
        // Arrange
        using var context = new HospitalDbContext(_options);
        var repository = new DoctorRepository(context, _mockLogger.Object);

        var doctor = new Doctor { FirstName = "Frank", LastName = "Blue", Email = "frank@test.com", IsActive = true };
        context.Doctors.Add(doctor);
        await context.SaveChangesAsync();

        // Act
        var exists = await repository.ExistsAsync(doctor.Id);

        // Assert
        Assert.True(exists);
    }

    [Fact]
    public async Task ExistsAsync_WithNonExistentId_ReturnsFalse()
    {
        // Arrange
        using var context = new HospitalDbContext(_options);
        var repository = new DoctorRepository(context, _mockLogger.Object);

        // Act
        var exists = await repository.ExistsAsync(999);

        // Assert
        Assert.False(exists);
    }

    [Fact]
    public async Task ExistsAsync_WithInactiveDoctor_ReturnsFalse()
    {
        // Arrange
        using var context = new HospitalDbContext(_options);
        var repository = new DoctorRepository(context, _mockLogger.Object);

        var doctor = new Doctor { FirstName = "Grace", LastName = "Yellow", Email = "grace@test.com", IsActive = false };
        context.Doctors.Add(doctor);
        await context.SaveChangesAsync();

        // Act
        var exists = await repository.ExistsAsync(doctor.Id);

        // Assert
        Assert.False(exists);
    }

    [Fact]
    public async Task EmailExistsAsync_WithExistingEmail_ReturnsTrue()
    {
        // Arrange
        using var context = new HospitalDbContext(_options);
        var repository = new DoctorRepository(context, _mockLogger.Object);

        var doctor = new Doctor { FirstName = "Henry", LastName = "Orange", Email = "henry@test.com", IsActive = true };
        context.Doctors.Add(doctor);
        await context.SaveChangesAsync();

        // Act
        var exists = await repository.EmailExistsAsync("henry@test.com");

        // Assert
        Assert.True(exists);
    }

    [Fact]
    public async Task EmailExistsAsync_WithNonExistentEmail_ReturnsFalse()
    {
        // Arrange
        using var context = new HospitalDbContext(_options);
        var repository = new DoctorRepository(context, _mockLogger.Object);

        // Act
        var exists = await repository.EmailExistsAsync("nonexistent@test.com");

        // Assert
        Assert.False(exists);
    }

    [Fact]
    public async Task SearchAsync_WithMatchingTerm_ReturnsDoctors()
    {
        // Arrange
        using var context = new HospitalDbContext(_options);
        var repository = new DoctorRepository(context, _mockLogger.Object);

        var doctor1 = new Doctor { FirstName = "John", LastName = "Smith", Email = "john.smith@test.com", IsActive = true };
        var doctor2 = new Doctor { FirstName = "Jane", LastName = "Doe", Email = "jane.doe@test.com", IsActive = true };
        context.Doctors.AddRange(doctor1, doctor2);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.SearchAsync("John");

        // Assert
        Assert.Single(result);
        Assert.Contains(result, d => d.FirstName == "John");
    }

    [Fact]
    public async Task SearchAsync_WithNoMatches_ReturnsEmptyList()
    {
        // Arrange
        using var context = new HospitalDbContext(_options);
        var repository = new DoctorRepository(context, _mockLogger.Object);

        var doctor = new Doctor { FirstName = "Isaac", LastName = "Purple", Email = "isaac@test.com", IsActive = true };
        context.Doctors.Add(doctor);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.SearchAsync("NotFound");

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetByDepartmentAsync_WithMatchingDepartment_ReturnsDoctors()
    {
        // Arrange
        using var context = new HospitalDbContext(_options);
        var repository = new DoctorRepository(context, _mockLogger.Object);

        var doctor1 = new Doctor { FirstName = "Jack", LastName = "Gray", Email = "jack@test.com", DepartmentId = 1, IsActive = true };
        var doctor2 = new Doctor { FirstName = "Jill", LastName = "Silver", Email = "jill@test.com", DepartmentId = 2, IsActive = true };
        context.Doctors.AddRange(doctor1, doctor2);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.GetByDepartmentAsync(1);

        // Assert
        Assert.Single(result);
        Assert.Contains(result, d => d.DepartmentId == 1);
    }

    [Fact]
    public async Task GetByDepartmentAsync_WithNoMatches_ReturnsEmptyList()
    {
        // Arrange
        using var context = new HospitalDbContext(_options);
        var repository = new DoctorRepository(context, _mockLogger.Object);

        var doctor = new Doctor { FirstName = "Kevin", LastName = "Gold", Email = "kevin@test.com", DepartmentId = 1, IsActive = true };
        context.Doctors.Add(doctor);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.GetByDepartmentAsync(999);

        // Assert
        Assert.Empty(result);
    }
}
