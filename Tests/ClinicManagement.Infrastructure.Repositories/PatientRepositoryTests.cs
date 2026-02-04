using Xunit;
using ClinicManagement.Domain.Entities;
using ClinicManagement.Infrastructure.Data;
using ClinicManagement.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Threading.Tasks;

namespace ClinicManagement.Infrastructure.Repositories.Tests;

public class PatientRepositoryTests
{
    private readonly DbContextOptions<ClinicDbContext> _options;
    private readonly Mock<ILogger<PatientRepository>> _loggerMock;

    public PatientRepositoryTests()
    {
        _options = new DbContextOptionsBuilder<ClinicDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        _loggerMock = new Mock<ILogger<PatientRepository>>();
    }

    [Fact]
    public void PatientRepository_Constructor_ShouldThrowArgumentNullException_WhenContextIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => new PatientRepository(null!, _loggerMock.Object));
    }

    [Fact]
    public void PatientRepository_Constructor_ShouldThrowArgumentNullException_WhenLoggerIsNull()
    {
        using var context = new ClinicDbContext(_options);
        Assert.Throws<ArgumentNullException>(() => new PatientRepository(context, null!));
    }

    [Fact]
    public async Task AddAsync_ShouldAddPatient()
    {
        using var context = new ClinicDbContext(_options);
        var repository = new PatientRepository(context, _loggerMock.Object);
        var patient = new Patient { Name = "Test Patient", Email = "test@test.com" };

        var result = await repository.AddAsync(patient);

        Assert.NotNull(result);
        Assert.True(result.Id > 0);
        Assert.True(result.IsActive);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnPatient_WhenExists()
    {
        using var context = new ClinicDbContext(_options);
        var repository = new PatientRepository(context, _loggerMock.Object);
        var patient = new Patient { Name = "Test", Email = "test@example.com" };
        await repository.AddAsync(patient);

        var result = await repository.GetByIdAsync(patient.Id);

        Assert.NotNull(result);
        Assert.Equal(patient.Id, result.Id);
    }

    [Fact]
    public async Task GetByEmailAsync_ShouldReturnPatient_WhenExists()
    {
        using var context = new ClinicDbContext(_options);
        var repository = new PatientRepository(context, _loggerMock.Object);
        var patient = new Patient { Name = "Test", Email = "unique@example.com" };
        await repository.AddAsync(patient);

        var result = await repository.GetByEmailAsync("unique@example.com");

        Assert.NotNull(result);
        Assert.Equal("unique@example.com", result.Email);
    }

    [Fact]
    public async Task ExistsAsync_ShouldReturnTrue_WhenPatientExists()
    {
        using var context = new ClinicDbContext(_options);
        var repository = new PatientRepository(context, _loggerMock.Object);
        var patient = new Patient { Name = "Test", Email = "exists@test.com" };
        await repository.AddAsync(patient);

        var result = await repository.ExistsAsync(patient.Id);

        Assert.True(result);
    }

    [Fact]
    public async Task DeleteAsync_ShouldMarkPatientAsInactive()
    {
        using var context = new ClinicDbContext(_options);
        var repository = new PatientRepository(context, _loggerMock.Object);
        var patient = new Patient { Name = "Test", Email = "delete@test.com" };
        await repository.AddAsync(patient);

        await repository.DeleteAsync(patient.Id);
        var result = await repository.ExistsAsync(patient.Id);

        Assert.False(result);
    }
}
