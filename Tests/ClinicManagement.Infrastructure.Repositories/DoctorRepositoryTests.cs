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
using System.Threading.Tasks;

namespace ClinicManagement.Infrastructure.Repositories.Tests;

/// <summary>
/// Unit tests for DoctorRepository
/// </summary>
public class DoctorRepositoryTests
{
    private readonly Mock<ILogger<DoctorRepository>> _mockLogger;

    public DoctorRepositoryTests()
    {
        _mockLogger = new Mock<ILogger<DoctorRepository>>();
    }

    private ClinicDbContext CreateInMemoryContext()
    {
        var options = new DbContextOptionsBuilder<ClinicDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        return new ClinicDbContext(options);
    }

    [Fact]
    public void DoctorRepository_Constructor_WithNullContext_ShouldThrowArgumentNullException()
    {
        // Arrange, Act & Assert
        Assert.Throws<ArgumentNullException>(() => new DoctorRepository(null!, _mockLogger.Object));
    }

    [Fact]
    public void DoctorRepository_Constructor_WithNullLogger_ShouldThrowArgumentNullException()
    {
        // Arrange
        using var context = CreateInMemoryContext();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new DoctorRepository(context, null!));
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllActiveDoctors()
    {
        // Arrange
        using var context = CreateInMemoryContext();
        var repository = new DoctorRepository(context, _mockLogger.Object);

        var doctors = new List<Doctor>
        {
            new Doctor { Id = 1, Name = "Dr. Smith", Email = "smith@test.com", Specialization = "Cardiology", IsActive = true },
            new Doctor { Id = 2, Name = "Dr. Jones", Email = "jones@test.com", Specialization = "Neurology", IsActive = true },
            new Doctor { Id = 3, Name = "Dr. Brown", Email = "brown@test.com", Specialization = "Orthopedics", IsActive = false }
        };

        await context.Doctors.AddRangeAsync(doctors);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.GetAllAsync();

        // Assert
        Assert.Equal(2, result.Count());
        Assert.All(result, d => Assert.True(d.IsActive));
    }

    [Fact]
    public async Task GetByIdAsync_WithValidId_ShouldReturnDoctor()
    {
        // Arrange
        using var context = CreateInMemoryContext();
        var repository = new DoctorRepository(context, _mockLogger.Object);

        var doctor = new Doctor { Id = 1, Name = "Dr. Test", Email = "test@test.com", Specialization = "General", IsActive = true };
        await context.Doctors.AddAsync(doctor);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.GetByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Dr. Test", result.Name);
    }

    [Fact]
    public async Task GetByIdAsync_WithInvalidId_ShouldReturnNull()
    {
        // Arrange
        using var context = CreateInMemoryContext();
        var repository = new DoctorRepository(context, _mockLogger.Object);

        // Act
        var result = await repository.GetByIdAsync(999);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetByEmailAsync_WithValidEmail_ShouldReturnDoctor()
    {
        // Arrange
        using var context = CreateInMemoryContext();
        var repository = new DoctorRepository(context, _mockLogger.Object);

        var doctor = new Doctor { Id = 1, Name = "Dr. Test", Email = "test@test.com", Specialization = "General", IsActive = true };
        await context.Doctors.AddAsync(doctor);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.GetByEmailAsync("test@test.com");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("test@test.com", result.Email);
    }

    [Fact]
    public async Task GetBySpecializationAsync_ShouldReturnDoctorsWithSpecialization()
    {
        // Arrange
        using var context = CreateInMemoryContext();
        var repository = new DoctorRepository(context, _mockLogger.Object);

        var doctors = new List<Doctor>
        {
            new Doctor { Id = 1, Name = "Dr. Heart", Email = "heart@test.com", Specialization = "Cardiology", IsActive = true },
            new Doctor { Id = 2, Name = "Dr. Heart2", Email = "heart2@test.com", Specialization = "Cardiology", IsActive = true },
            new Doctor { Id = 3, Name = "Dr. Brain", Email = "brain@test.com", Specialization = "Neurology", IsActive = true }
        };

        await context.Doctors.AddRangeAsync(doctors);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.GetBySpecializationAsync("Cardiology");

        // Assert
        Assert.Equal(2, result.Count());
        Assert.All(result, d => Assert.Equal("Cardiology", d.Specialization));
    }

    [Fact]
    public async Task AddAsync_ShouldAddDoctorAndSetDefaults()
    {
        // Arrange
        using var context = CreateInMemoryContext();
        var repository = new DoctorRepository(context, _mockLogger.Object);

        var doctor = new Doctor { Name = "Dr. New", Email = "new@test.com", Specialization = "General" };

        // Act
        var result = await repository.AddAsync(doctor);

        // Assert
        Assert.NotEqual(0, result.Id);
        Assert.True(result.IsActive);
        Assert.NotEqual(default, result.CreatedDate);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateDoctorAndSetModifiedDate()
    {
        // Arrange
        using var context = CreateInMemoryContext();
        var repository = new DoctorRepository(context, _mockLogger.Object);

        var doctor = new Doctor { Id = 1, Name = "Dr. Original", Email = "original@test.com", Specialization = "General", IsActive = true };
        await context.Doctors.AddAsync(doctor);
        await context.SaveChangesAsync();

        // Act
        doctor.Name = "Dr. Updated";
        await repository.UpdateAsync(doctor);

        // Assert
        var updated = await context.Doctors.FindAsync(1);
        Assert.NotNull(updated);
        Assert.Equal("Dr. Updated", updated.Name);
        Assert.NotNull(updated.ModifiedDate);
    }

    [Fact]
    public async Task DeleteAsync_ShouldSoftDeleteDoctor()
    {
        // Arrange
        using var context = CreateInMemoryContext();
        var repository = new DoctorRepository(context, _mockLogger.Object);

        var doctor = new Doctor { Id = 1, Name = "Dr. Test", Email = "test@test.com", Specialization = "General", IsActive = true };
        await context.Doctors.AddAsync(doctor);
        await context.SaveChangesAsync();

        // Act
        await repository.DeleteAsync(1);

        // Assert
        var deleted = await context.Doctors.FindAsync(1);
        Assert.NotNull(deleted);
        Assert.False(deleted.IsActive);
        Assert.NotNull(deleted.ModifiedDate);
    }

    [Fact]
    public async Task ExistsAsync_WithExistingId_ShouldReturnTrue()
    {
        // Arrange
        using var context = CreateInMemoryContext();
        var repository = new DoctorRepository(context, _mockLogger.Object);

        var doctor = new Doctor { Id = 1, Name = "Dr. Test", Email = "test@test.com", Specialization = "General", IsActive = true };
        await context.Doctors.AddAsync(doctor);
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
        var repository = new DoctorRepository(context, _mockLogger.Object);

        // Act
        var result = await repository.ExistsAsync(999);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task SearchAsync_WithMatchingTerm_ShouldReturnMatchingDoctors()
    {
        // Arrange
        using var context = CreateInMemoryContext();
        var repository = new DoctorRepository(context, _mockLogger.Object);

        var doctors = new List<Doctor>
        {
            new Doctor { Id = 1, Name = "Dr. Smith", Email = "smith@test.com", Specialization = "Cardiology", IsActive = true },
            new Doctor { Id = 2, Name = "Dr. Jones", Email = "jones@test.com", Specialization = "Neurology", IsActive = true }
        };

        await context.Doctors.AddRangeAsync(doctors);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.SearchAsync("smith");

        // Assert
        Assert.Single(result);
        Assert.Equal("Dr. Smith", result.First().Name);
    }
}
