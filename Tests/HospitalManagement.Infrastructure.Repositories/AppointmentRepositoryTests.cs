using HospitalManagement.Domain.Entities;
using HospitalManagement.Infrastructure.Data;
using HospitalManagement.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Tests.HospitalManagement.Infrastructure.Repositories;

public class AppointmentRepositoryTests
{
    private readonly Mock<ILogger<AppointmentRepository>> _mockLogger;
    private readonly DbContextOptions<ApplicationDbContext> _dbContextOptions;

    public AppointmentRepositoryTests()
    {
        _mockLogger = new Mock<ILogger<AppointmentRepository>>();
        _dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnActiveAppointments()
    {
        // Arrange
        using var context = new ApplicationDbContext(_dbContextOptions);
        var repository = new AppointmentRepository(context, _mockLogger.Object);

        var patient = new User { Id = 1, Name = "Patient", Email = "patient@test.com", IsActive = true };
        var doctor = new User { Id = 2, Name = "Doctor", Email = "doctor@test.com", IsActive = true };
        await context.Users.AddRangeAsync(patient, doctor);
        await context.SaveChangesAsync();

        var appointment1 = new Appointment { Id = 1, PatientId = 1, DoctorId = 2, IsActive = true };
        var appointment2 = new Appointment { Id = 2, PatientId = 1, DoctorId = 2, IsActive = false };

        await context.Appointments.AddRangeAsync(appointment1, appointment2);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.GetAllAsync();

        // Assert
        Assert.Single(result);
        Assert.Contains(result, a => a.Id == 1);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnAppointment_WhenExists()
    {
        // Arrange
        using var context = new ApplicationDbContext(_dbContextOptions);
        var repository = new AppointmentRepository(context, _mockLogger.Object);

        var patient = new User { Id = 1, Name = "Patient", Email = "patient@test.com", IsActive = true };
        var doctor = new User { Id = 2, Name = "Doctor", Email = "doctor@test.com", IsActive = true };
        await context.Users.AddRangeAsync(patient, doctor);
        await context.SaveChangesAsync();

        var appointment = new Appointment { Id = 1, PatientId = 1, DoctorId = 2, IsActive = true };
        await context.Appointments.AddAsync(appointment);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.GetByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
    }

    [Fact]
    public async Task GetByPatientIdAsync_ShouldReturnPatientAppointments()
    {
        // Arrange
        using var context = new ApplicationDbContext(_dbContextOptions);
        var repository = new AppointmentRepository(context, _mockLogger.Object);

        var patient = new User { Id = 1, Name = "Patient", Email = "patient@test.com", IsActive = true };
        var doctor = new User { Id = 2, Name = "Doctor", Email = "doctor@test.com", IsActive = true };
        await context.Users.AddRangeAsync(patient, doctor);
        await context.SaveChangesAsync();

        var appointment1 = new Appointment { Id = 1, PatientId = 1, DoctorId = 2, IsActive = true };
        var appointment2 = new Appointment { Id = 2, PatientId = 1, DoctorId = 2, IsActive = true };

        await context.Appointments.AddRangeAsync(appointment1, appointment2);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.GetByPatientIdAsync(1);

        // Assert
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetByDoctorIdAsync_ShouldReturnDoctorAppointments()
    {
        // Arrange
        using var context = new ApplicationDbContext(_dbContextOptions);
        var repository = new AppointmentRepository(context, _mockLogger.Object);

        var patient = new User { Id = 1, Name = "Patient", Email = "patient@test.com", IsActive = true };
        var doctor = new User { Id = 2, Name = "Doctor", Email = "doctor@test.com", IsActive = true };
        await context.Users.AddRangeAsync(patient, doctor);
        await context.SaveChangesAsync();

        var appointment1 = new Appointment { Id = 1, PatientId = 1, DoctorId = 2, IsActive = true };
        var appointment2 = new Appointment { Id = 2, PatientId = 1, DoctorId = 2, IsActive = true };

        await context.Appointments.AddRangeAsync(appointment1, appointment2);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.GetByDoctorIdAsync(2);

        // Assert
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task AddAsync_ShouldAddAppointment()
    {
        // Arrange
        using var context = new ApplicationDbContext(_dbContextOptions);
        var repository = new AppointmentRepository(context, _mockLogger.Object);

        var patient = new User { Id = 1, Name = "Patient", Email = "patient@test.com", IsActive = true };
        var doctor = new User { Id = 2, Name = "Doctor", Email = "doctor@test.com", IsActive = true };
        await context.Users.AddRangeAsync(patient, doctor);
        await context.SaveChangesAsync();

        var appointment = new Appointment
        {
            PatientId = 1,
            DoctorId = 2,
            AppointmentDate = DateTime.UtcNow,
            IsActive = true
        };

        // Act
        var result = await repository.AddAsync(appointment);

        // Assert
        Assert.NotEqual(0, result.Id);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateAppointment()
    {
        // Arrange
        using var context = new ApplicationDbContext(_dbContextOptions);
        var repository = new AppointmentRepository(context, _mockLogger.Object);

        var patient = new User { Id = 1, Name = "Patient", Email = "patient@test.com", IsActive = true };
        var doctor = new User { Id = 2, Name = "Doctor", Email = "doctor@test.com", IsActive = true };
        await context.Users.AddRangeAsync(patient, doctor);
        await context.SaveChangesAsync();

        var appointment = new Appointment { Id = 1, PatientId = 1, DoctorId = 2, Status = "Pending", IsActive = true };
        await context.Appointments.AddAsync(appointment);
        await context.SaveChangesAsync();

        context.Entry(appointment).State = EntityState.Detached;
        appointment.Status = "Confirmed";

        // Act
        await repository.UpdateAsync(appointment);

        // Assert
        var updatedAppointment = await context.Appointments.FindAsync(1);
        Assert.NotNull(updatedAppointment);
        Assert.Equal("Confirmed", updatedAppointment.Status);
    }

    [Fact]
    public async Task DeleteAsync_ShouldSetIsActiveToFalse()
    {
        // Arrange
        using var context = new ApplicationDbContext(_dbContextOptions);
        var repository = new AppointmentRepository(context, _mockLogger.Object);

        var patient = new User { Id = 1, Name = "Patient", Email = "patient@test.com", IsActive = true };
        var doctor = new User { Id = 2, Name = "Doctor", Email = "doctor@test.com", IsActive = true };
        await context.Users.AddRangeAsync(patient, doctor);
        await context.SaveChangesAsync();

        var appointment = new Appointment { Id = 1, PatientId = 1, DoctorId = 2, IsActive = true };
        await context.Appointments.AddAsync(appointment);
        await context.SaveChangesAsync();

        // Act
        await repository.DeleteAsync(1);

        // Assert
        var deletedAppointment = await context.Appointments.FindAsync(1);
        Assert.NotNull(deletedAppointment);
        Assert.False(deletedAppointment.IsActive);
    }

    [Fact]
    public async Task ExistsAsync_ShouldReturnTrue_WhenAppointmentExists()
    {
        // Arrange
        using var context = new ApplicationDbContext(_dbContextOptions);
        var repository = new AppointmentRepository(context, _mockLogger.Object);

        var patient = new User { Id = 1, Name = "Patient", Email = "patient@test.com", IsActive = true };
        var doctor = new User { Id = 2, Name = "Doctor", Email = "doctor@test.com", IsActive = true };
        await context.Users.AddRangeAsync(patient, doctor);
        await context.SaveChangesAsync();

        var appointment = new Appointment { Id = 1, PatientId = 1, DoctorId = 2, IsActive = true };
        await context.Appointments.AddAsync(appointment);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.ExistsAsync(1);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task SearchAsync_ShouldReturnMatchingAppointments()
    {
        // Arrange
        using var context = new ApplicationDbContext(_dbContextOptions);
        var repository = new AppointmentRepository(context, _mockLogger.Object);

        var patient = new User { Id = 1, Name = "John Doe", Email = "john@test.com", IsActive = true };
        var doctor = new User { Id = 2, Name = "Dr. Smith", Email = "smith@test.com", IsActive = true };
        await context.Users.AddRangeAsync(patient, doctor);
        await context.SaveChangesAsync();

        var appointment = new Appointment { Id = 1, PatientId = 1, DoctorId = 2, Status = "Confirmed", IsActive = true };
        await context.Appointments.AddAsync(appointment);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.SearchAsync("Confirmed");

        // Assert
        Assert.Single(result);
    }
}
