using Xunit;
using Moq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using HospitalManagement.Domain.Entities;
using HospitalManagement.Infrastructure.Repositories;
using HospitalManagement.Infrastructure.Data;

namespace HospitalManagement.Infrastructure.Repositories.Tests;

public class AppointmentRepositoryTests
{
    private readonly Mock<ILogger<AppointmentRepository>> _mockLogger;
    private readonly DbContextOptions<HospitalDbContext> _options;

    public AppointmentRepositoryTests()
    {
        _mockLogger = new Mock<ILogger<AppointmentRepository>>();
        _options = new DbContextOptionsBuilder<HospitalDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
    }

    [Fact]
    public void AppointmentRepository_Constructor_WithNullContext_ThrowsArgumentNullException()
    {
        // Arrange & Act & Assert
        Assert.Throws<ArgumentNullException>(() => new AppointmentRepository(null!, _mockLogger.Object));
    }

    [Fact]
    public void AppointmentRepository_Constructor_WithNullLogger_ThrowsArgumentNullException()
    {
        // Arrange
        using var context = new HospitalDbContext(_options);

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new AppointmentRepository(context, null!));
    }

    [Fact]
    public void AppointmentRepository_Constructor_WithValidParameters_CreatesInstance()
    {
        // Arrange
        using var context = new HospitalDbContext(_options);

        // Act
        var repository = new AppointmentRepository(context, _mockLogger.Object);

        // Assert
        Assert.NotNull(repository);
    }

    [Fact]
    public async Task GetAllAsync_WithNoAppointments_ReturnsEmptyList()
    {
        // Arrange
        using var context = new HospitalDbContext(_options);
        var repository = new AppointmentRepository(context, _mockLogger.Object);

        // Act
        var result = await repository.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetAllAsync_WithActiveAppointments_ReturnsActiveAppointmentsOnly()
    {
        // Arrange
        using var context = new HospitalDbContext(_options);
        var repository = new AppointmentRepository(context, _mockLogger.Object);

        var activeAppointment = new Appointment { Id = 1, PatientId = 1, DoctorId = 1, IsActive = true, Status = "Pending" };
        var inactiveAppointment = new Appointment { Id = 2, PatientId = 2, DoctorId = 2, IsActive = false, Status = "Cancelled" };

        context.Appointments.AddRange(activeAppointment, inactiveAppointment);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.GetAllAsync();

        // Assert
        Assert.Single(result);
        Assert.Contains(result, a => a.Id == 1);
    }

    [Fact]
    public async Task GetByIdAsync_WithNonExistentId_ReturnsNull()
    {
        // Arrange
        using var context = new HospitalDbContext(_options);
        var repository = new AppointmentRepository(context, _mockLogger.Object);

        // Act
        var result = await repository.GetByIdAsync(999);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetByIdAsync_WithExistingId_ReturnsAppointment()
    {
        // Arrange
        using var context = new HospitalDbContext(_options);
        var repository = new AppointmentRepository(context, _mockLogger.Object);

        var appointment = new Appointment { Id = 1, PatientId = 1, DoctorId = 1, IsActive = true, Status = "Approved" };
        context.Appointments.Add(appointment);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.GetByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("Approved", result.Status);
    }

    [Fact]
    public async Task GetByIdAsync_WithInactiveAppointment_ReturnsNull()
    {
        // Arrange
        using var context = new HospitalDbContext(_options);
        var repository = new AppointmentRepository(context, _mockLogger.Object);

        var appointment = new Appointment { Id = 1, PatientId = 1, DoctorId = 1, IsActive = false, Status = "Cancelled" };
        context.Appointments.Add(appointment);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.GetByIdAsync(1);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task AddAsync_WithValidAppointment_AddsToDatabase()
    {
        // Arrange
        using var context = new HospitalDbContext(_options);
        var repository = new AppointmentRepository(context, _mockLogger.Object);

        var appointment = new Appointment { PatientId = 10, DoctorId = 20, Status = "Pending" };

        // Act
        var result = await repository.AddAsync(appointment);

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
        var repository = new AppointmentRepository(context, _mockLogger.Object);

        var appointment = new Appointment { PatientId = 5, DoctorId = 10, Status = "Pending" };

        // Act
        var result = await repository.AddAsync(appointment);

        // Assert
        Assert.True(result.IsActive);
        Assert.True((DateTime.UtcNow - result.CreatedDate).TotalSeconds < 5);
    }

    [Fact]
    public async Task UpdateAsync_WithValidAppointment_UpdatesDatabase()
    {
        // Arrange
        using var context = new HospitalDbContext(_options);
        var repository = new AppointmentRepository(context, _mockLogger.Object);

        var appointment = new Appointment { PatientId = 1, DoctorId = 1, Status = "Pending", IsActive = true };
        context.Appointments.Add(appointment);
        await context.SaveChangesAsync();

        // Act
        appointment.Status = "Approved";
        await repository.UpdateAsync(appointment);

        var updated = await context.Appointments.FindAsync(appointment.Id);

        // Assert
        Assert.NotNull(updated);
        Assert.Equal("Approved", updated.Status);
        Assert.NotNull(updated.ModifiedDate);
    }

    [Fact]
    public async Task UpdateAsync_SetsModifiedDate()
    {
        // Arrange
        using var context = new HospitalDbContext(_options);
        var repository = new AppointmentRepository(context, _mockLogger.Object);

        var appointment = new Appointment { PatientId = 2, DoctorId = 2, Status = "Pending", IsActive = true };
        context.Appointments.Add(appointment);
        await context.SaveChangesAsync();

        // Act
        appointment.Status = "Completed";
        await repository.UpdateAsync(appointment);

        var updated = await context.Appointments.FindAsync(appointment.Id);

        // Assert
        Assert.NotNull(updated);
        Assert.NotNull(updated.ModifiedDate);
        Assert.True((DateTime.UtcNow - updated.ModifiedDate.Value).TotalSeconds < 5);
    }

    [Fact]
    public async Task DeleteAsync_WithExistingId_SoftDeletesAppointment()
    {
        // Arrange
        using var context = new HospitalDbContext(_options);
        var repository = new AppointmentRepository(context, _mockLogger.Object);

        var appointment = new Appointment { PatientId = 3, DoctorId = 3, Status = "Pending", IsActive = true };
        context.Appointments.Add(appointment);
        await context.SaveChangesAsync();

        // Act
        await repository.DeleteAsync(appointment.Id);

        var deleted = await context.Appointments.FindAsync(appointment.Id);

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
        var repository = new AppointmentRepository(context, _mockLogger.Object);

        // Act & Assert
        await repository.DeleteAsync(999);
    }

    [Fact]
    public async Task ExistsAsync_WithExistingId_ReturnsTrue()
    {
        // Arrange
        using var context = new HospitalDbContext(_options);
        var repository = new AppointmentRepository(context, _mockLogger.Object);

        var appointment = new Appointment { PatientId = 4, DoctorId = 4, Status = "Pending", IsActive = true };
        context.Appointments.Add(appointment);
        await context.SaveChangesAsync();

        // Act
        var exists = await repository.ExistsAsync(appointment.Id);

        // Assert
        Assert.True(exists);
    }

    [Fact]
    public async Task ExistsAsync_WithNonExistentId_ReturnsFalse()
    {
        // Arrange
        using var context = new HospitalDbContext(_options);
        var repository = new AppointmentRepository(context, _mockLogger.Object);

        // Act
        var exists = await repository.ExistsAsync(999);

        // Assert
        Assert.False(exists);
    }

    [Fact]
    public async Task ExistsAsync_WithInactiveAppointment_ReturnsFalse()
    {
        // Arrange
        using var context = new HospitalDbContext(_options);
        var repository = new AppointmentRepository(context, _mockLogger.Object);

        var appointment = new Appointment { PatientId = 5, DoctorId = 5, Status = "Cancelled", IsActive = false };
        context.Appointments.Add(appointment);
        await context.SaveChangesAsync();

        // Act
        var exists = await repository.ExistsAsync(appointment.Id);

        // Assert
        Assert.False(exists);
    }

    [Fact]
    public async Task GetByPatientIdAsync_WithMatchingAppointments_ReturnsAppointments()
    {
        // Arrange
        using var context = new HospitalDbContext(_options);
        var repository = new AppointmentRepository(context, _mockLogger.Object);

        var appointment1 = new Appointment { PatientId = 10, DoctorId = 1, Status = "Pending", IsActive = true };
        var appointment2 = new Appointment { PatientId = 10, DoctorId = 2, Status = "Approved", IsActive = true };
        var appointment3 = new Appointment { PatientId = 20, DoctorId = 1, Status = "Pending", IsActive = true };

        context.Appointments.AddRange(appointment1, appointment2, appointment3);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.GetByPatientIdAsync(10);

        // Assert
        Assert.Equal(2, result.Count());
        Assert.All(result, a => Assert.Equal(10, a.PatientId));
    }

    [Fact]
    public async Task GetByPatientIdAsync_WithNoMatches_ReturnsEmptyList()
    {
        // Arrange
        using var context = new HospitalDbContext(_options);
        var repository = new AppointmentRepository(context, _mockLogger.Object);

        var appointment = new Appointment { PatientId = 10, DoctorId = 1, Status = "Pending", IsActive = true };
        context.Appointments.Add(appointment);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.GetByPatientIdAsync(999);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetByDoctorIdAsync_WithMatchingAppointments_ReturnsAppointments()
    {
        // Arrange
        using var context = new HospitalDbContext(_options);
        var repository = new AppointmentRepository(context, _mockLogger.Object);

        var appointment1 = new Appointment { PatientId = 1, DoctorId = 50, Status = "Pending", IsActive = true };
        var appointment2 = new Appointment { PatientId = 2, DoctorId = 50, Status = "Approved", IsActive = true };
        var appointment3 = new Appointment { PatientId = 3, DoctorId = 60, Status = "Pending", IsActive = true };

        context.Appointments.AddRange(appointment1, appointment2, appointment3);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.GetByDoctorIdAsync(50);

        // Assert
        Assert.Equal(2, result.Count());
        Assert.All(result, a => Assert.Equal(50, a.DoctorId));
    }

    [Fact]
    public async Task GetByDoctorIdAsync_WithNoMatches_ReturnsEmptyList()
    {
        // Arrange
        using var context = new HospitalDbContext(_options);
        var repository = new AppointmentRepository(context, _mockLogger.Object);

        var appointment = new Appointment { PatientId = 1, DoctorId = 50, Status = "Pending", IsActive = true };
        context.Appointments.Add(appointment);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.GetByDoctorIdAsync(999);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetPendingAppointmentsAsync_WithPendingAppointments_ReturnsOnlyPending()
    {
        // Arrange
        using var context = new HospitalDbContext(_options);
        var repository = new AppointmentRepository(context, _mockLogger.Object);

        var appointment1 = new Appointment { PatientId = 1, DoctorId = 100, Status = "Pending", IsActive = true };
        var appointment2 = new Appointment { PatientId = 2, DoctorId = 100, Status = "Approved", IsActive = true };
        var appointment3 = new Appointment { PatientId = 3, DoctorId = 100, Status = "Pending", IsActive = true };

        context.Appointments.AddRange(appointment1, appointment2, appointment3);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.GetPendingAppointmentsAsync(100);

        // Assert
        Assert.Equal(2, result.Count());
        Assert.All(result, a => Assert.Equal("Pending", a.Status));
    }

    [Fact]
    public async Task GetPendingAppointmentsAsync_WithNoPendingAppointments_ReturnsEmptyList()
    {
        // Arrange
        using var context = new HospitalDbContext(_options);
        var repository = new AppointmentRepository(context, _mockLogger.Object);

        var appointment1 = new Appointment { PatientId = 1, DoctorId = 100, Status = "Approved", IsActive = true };
        var appointment2 = new Appointment { PatientId = 2, DoctorId = 100, Status = "Completed", IsActive = true };

        context.Appointments.AddRange(appointment1, appointment2);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.GetPendingAppointmentsAsync(100);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetAllAsync_WithMultipleActiveAppointments_ReturnsAll()
    {
        // Arrange
        using var context = new HospitalDbContext(_options);
        var repository = new AppointmentRepository(context, _mockLogger.Object);

        var appointment1 = new Appointment { PatientId = 1, DoctorId = 1, Status = "Pending", IsActive = true };
        var appointment2 = new Appointment { PatientId = 2, DoctorId = 2, Status = "Approved", IsActive = true };
        var appointment3 = new Appointment { PatientId = 3, DoctorId = 3, Status = "Completed", IsActive = true };

        context.Appointments.AddRange(appointment1, appointment2, appointment3);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.GetAllAsync();

        // Assert
        Assert.Equal(3, result.Count());
    }
}
