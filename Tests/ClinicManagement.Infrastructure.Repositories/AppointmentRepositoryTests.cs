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
/// Unit tests for AppointmentRepository
/// </summary>
public class AppointmentRepositoryTests
{
    private readonly Mock<ILogger<AppointmentRepository>> _mockLogger;

    public AppointmentRepositoryTests()
    {
        _mockLogger = new Mock<ILogger<AppointmentRepository>>();
    }

    private ClinicDbContext CreateInMemoryContext()
    {
        var options = new DbContextOptionsBuilder<ClinicDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        return new ClinicDbContext(options);
    }

    [Fact]
    public void AppointmentRepository_Constructor_WithNullContext_ShouldThrowArgumentNullException()
    {
        // Arrange, Act & Assert
        Assert.Throws<ArgumentNullException>(() => new AppointmentRepository(null!, _mockLogger.Object));
    }

    [Fact]
    public void AppointmentRepository_Constructor_WithNullLogger_ShouldThrowArgumentNullException()
    {
        // Arrange
        using var context = CreateInMemoryContext();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new AppointmentRepository(context, null!));
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllActiveAppointments()
    {
        // Arrange
        using var context = CreateInMemoryContext();
        var repository = new AppointmentRepository(context, _mockLogger.Object);

        var appointments = new List<Appointment>
        {
            new Appointment { Id = 1, PatientId = 1, DoctorId = 1, Status = "Pending", IsActive = true },
            new Appointment { Id = 2, PatientId = 2, DoctorId = 2, Status = "Confirmed", IsActive = true },
            new Appointment { Id = 3, PatientId = 3, DoctorId = 3, Status = "Cancelled", IsActive = false }
        };

        await context.Appointments.AddRangeAsync(appointments);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.GetAllAsync();

        // Assert
        Assert.Equal(2, result.Count());
        Assert.All(result, a => Assert.True(a.IsActive));
    }

    [Fact]
    public async Task GetByIdAsync_WithValidId_ShouldReturnAppointment()
    {
        // Arrange
        using var context = CreateInMemoryContext();
        var repository = new AppointmentRepository(context, _mockLogger.Object);

        var appointment = new Appointment { Id = 1, PatientId = 1, DoctorId = 1, Status = "Pending", IsActive = true };
        await context.Appointments.AddAsync(appointment);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.GetByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
    }

    [Fact]
    public async Task GetByIdAsync_WithInvalidId_ShouldReturnNull()
    {
        // Arrange
        using var context = CreateInMemoryContext();
        var repository = new AppointmentRepository(context, _mockLogger.Object);

        // Act
        var result = await repository.GetByIdAsync(999);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetByPatientIdAsync_ShouldReturnPatientAppointments()
    {
        // Arrange
        using var context = CreateInMemoryContext();
        var repository = new AppointmentRepository(context, _mockLogger.Object);

        var appointments = new List<Appointment>
        {
            new Appointment { Id = 1, PatientId = 1, DoctorId = 1, Status = "Pending", IsActive = true },
            new Appointment { Id = 2, PatientId = 1, DoctorId = 2, Status = "Confirmed", IsActive = true },
            new Appointment { Id = 3, PatientId = 2, DoctorId = 1, Status = "Pending", IsActive = true }
        };

        await context.Appointments.AddRangeAsync(appointments);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.GetByPatientIdAsync(1);

        // Assert
        Assert.Equal(2, result.Count());
        Assert.All(result, a => Assert.Equal(1, a.PatientId));
    }

    [Fact]
    public async Task GetByDoctorIdAsync_ShouldReturnDoctorAppointments()
    {
        // Arrange
        using var context = CreateInMemoryContext();
        var repository = new AppointmentRepository(context, _mockLogger.Object);

        var appointments = new List<Appointment>
        {
            new Appointment { Id = 1, PatientId = 1, DoctorId = 1, Status = "Pending", IsActive = true },
            new Appointment { Id = 2, PatientId = 2, DoctorId = 1, Status = "Confirmed", IsActive = true },
            new Appointment { Id = 3, PatientId = 3, DoctorId = 2, Status = "Pending", IsActive = true }
        };

        await context.Appointments.AddRangeAsync(appointments);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.GetByDoctorIdAsync(1);

        // Assert
        Assert.Equal(2, result.Count());
        Assert.All(result, a => Assert.Equal(1, a.DoctorId));
    }

    [Fact]
    public async Task GetPendingAppointmentsAsync_ShouldReturnOnlyPendingAppointments()
    {
        // Arrange
        using var context = CreateInMemoryContext();
        var repository = new AppointmentRepository(context, _mockLogger.Object);

        var appointments = new List<Appointment>
        {
            new Appointment { Id = 1, PatientId = 1, DoctorId = 1, Status = "Pending", IsActive = true },
            new Appointment { Id = 2, PatientId = 2, DoctorId = 1, Status = "Confirmed", IsActive = true },
            new Appointment { Id = 3, PatientId = 3, DoctorId = 1, Status = "Pending", IsActive = true }
        };

        await context.Appointments.AddRangeAsync(appointments);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.GetPendingAppointmentsAsync(1);

        // Assert
        Assert.Equal(2, result.Count());
        Assert.All(result, a => Assert.Equal("Pending", a.Status));
    }

    [Fact]
    public async Task AddAsync_ShouldAddAppointmentAndSetDefaults()
    {
        // Arrange
        using var context = CreateInMemoryContext();
        var repository = new AppointmentRepository(context, _mockLogger.Object);

        var appointment = new Appointment { PatientId = 1, DoctorId = 1, Status = "Pending" };

        // Act
        var result = await repository.AddAsync(appointment);

        // Assert
        Assert.NotEqual(0, result.Id);
        Assert.True(result.IsActive);
        Assert.NotEqual(default, result.CreatedDate);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateAppointmentAndSetModifiedDate()
    {
        // Arrange
        using var context = CreateInMemoryContext();
        var repository = new AppointmentRepository(context, _mockLogger.Object);

        var appointment = new Appointment { Id = 1, PatientId = 1, DoctorId = 1, Status = "Pending", IsActive = true };
        await context.Appointments.AddAsync(appointment);
        await context.SaveChangesAsync();

        // Act
        appointment.Status = "Confirmed";
        await repository.UpdateAsync(appointment);

        // Assert
        var updated = await context.Appointments.FindAsync(1);
        Assert.NotNull(updated);
        Assert.Equal("Confirmed", updated.Status);
        Assert.NotNull(updated.ModifiedDate);
    }

    [Fact]
    public async Task DeleteAsync_ShouldSoftDeleteAppointment()
    {
        // Arrange
        using var context = CreateInMemoryContext();
        var repository = new AppointmentRepository(context, _mockLogger.Object);

        var appointment = new Appointment { Id = 1, PatientId = 1, DoctorId = 1, Status = "Pending", IsActive = true };
        await context.Appointments.AddAsync(appointment);
        await context.SaveChangesAsync();

        // Act
        await repository.DeleteAsync(1);

        // Assert
        var deleted = await context.Appointments.FindAsync(1);
        Assert.NotNull(deleted);
        Assert.False(deleted.IsActive);
        Assert.NotNull(deleted.ModifiedDate);
    }

    [Fact]
    public async Task ExistsAsync_WithExistingId_ShouldReturnTrue()
    {
        // Arrange
        using var context = CreateInMemoryContext();
        var repository = new AppointmentRepository(context, _mockLogger.Object);

        var appointment = new Appointment { Id = 1, PatientId = 1, DoctorId = 1, Status = "Pending", IsActive = true };
        await context.Appointments.AddAsync(appointment);
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
        var repository = new AppointmentRepository(context, _mockLogger.Object);

        // Act
        var result = await repository.ExistsAsync(999);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task SearchAsync_WithEmptyTerm_ShouldReturnAllActiveAppointments()
    {
        // Arrange
        using var context = CreateInMemoryContext();
        var repository = new AppointmentRepository(context, _mockLogger.Object);

        var appointments = new List<Appointment>
        {
            new Appointment { Id = 1, PatientId = 1, DoctorId = 1, Status = "Pending", IsActive = true },
            new Appointment { Id = 2, PatientId = 2, DoctorId = 2, Status = "Confirmed", IsActive = true }
        };

        await context.Appointments.AddRangeAsync(appointments);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.SearchAsync("");

        // Assert
        Assert.Equal(2, result.Count());
    }
}
