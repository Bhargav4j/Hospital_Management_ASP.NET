using Xunit;
using ClinicManagement.Domain.Entities;
using ClinicManagement.Domain.Enums;
using ClinicManagement.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ClinicManagement.Domain.Interfaces.Repositories.Tests;

/// <summary>
/// Test class for IAppointmentRepository interface implementation
/// </summary>
public class IAppointmentRepositoryTests
{
    private class MockAppointmentRepository : IAppointmentRepository
    {
        private readonly List<Appointment> _appointments = new List<Appointment>();

        public Task<IEnumerable<Appointment>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return Task.FromResult<IEnumerable<Appointment>>(_appointments);
        }

        public Task<Appointment?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var appointment = _appointments.Find(a => a.Id == id);
            return Task.FromResult(appointment);
        }

        public Task<IEnumerable<Appointment>> GetByPatientIdAsync(int patientId, CancellationToken cancellationToken = default)
        {
            var results = _appointments.FindAll(a => a.PatientId == patientId);
            return Task.FromResult<IEnumerable<Appointment>>(results);
        }

        public Task<IEnumerable<Appointment>> GetByDoctorIdAsync(int doctorId, CancellationToken cancellationToken = default)
        {
            var results = _appointments.FindAll(a => a.DoctorId == doctorId);
            return Task.FromResult<IEnumerable<Appointment>>(results);
        }

        public Task<IEnumerable<Appointment>> GetByStatusAsync(AppointmentStatus status, CancellationToken cancellationToken = default)
        {
            var results = _appointments.FindAll(a => a.Status == status);
            return Task.FromResult<IEnumerable<Appointment>>(results);
        }

        public Task<Appointment> AddAsync(Appointment appointment, CancellationToken cancellationToken = default)
        {
            _appointments.Add(appointment);
            return Task.FromResult(appointment);
        }

        public Task UpdateAsync(Appointment appointment, CancellationToken cancellationToken = default)
        {
            var existing = _appointments.Find(a => a.Id == appointment.Id);
            if (existing != null)
            {
                _appointments.Remove(existing);
                _appointments.Add(appointment);
            }
            return Task.CompletedTask;
        }

        public Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var appointment = _appointments.Find(a => a.Id == id);
            if (appointment != null)
            {
                _appointments.Remove(appointment);
            }
            return Task.CompletedTask;
        }

        public Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
        {
            var exists = _appointments.Exists(a => a.Id == id);
            return Task.FromResult(exists);
        }
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllAppointments()
    {
        // Arrange
        var repository = new MockAppointmentRepository();
        await repository.AddAsync(new Appointment { Id = 1, PatientId = 1, DoctorId = 1 });
        await repository.AddAsync(new Appointment { Id = 2, PatientId = 2, DoctorId = 1 });

        // Act
        var result = await repository.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnAppointment_WhenExists()
    {
        // Arrange
        var repository = new MockAppointmentRepository();
        var appointment = new Appointment { Id = 1, PatientId = 1, DoctorId = 1 };
        await repository.AddAsync(appointment);

        // Act
        var result = await repository.GetByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenNotExists()
    {
        // Arrange
        var repository = new MockAppointmentRepository();

        // Act
        var result = await repository.GetByIdAsync(999);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetByPatientIdAsync_ShouldReturnPatientAppointments()
    {
        // Arrange
        var repository = new MockAppointmentRepository();
        await repository.AddAsync(new Appointment { Id = 1, PatientId = 1, DoctorId = 1 });
        await repository.AddAsync(new Appointment { Id = 2, PatientId = 1, DoctorId = 2 });
        await repository.AddAsync(new Appointment { Id = 3, PatientId = 2, DoctorId = 1 });

        // Act
        var result = await repository.GetByPatientIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        Assert.All(result, a => Assert.Equal(1, a.PatientId));
    }

    [Fact]
    public async Task GetByDoctorIdAsync_ShouldReturnDoctorAppointments()
    {
        // Arrange
        var repository = new MockAppointmentRepository();
        await repository.AddAsync(new Appointment { Id = 1, PatientId = 1, DoctorId = 1 });
        await repository.AddAsync(new Appointment { Id = 2, PatientId = 2, DoctorId = 1 });
        await repository.AddAsync(new Appointment { Id = 3, PatientId = 1, DoctorId = 2 });

        // Act
        var result = await repository.GetByDoctorIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        Assert.All(result, a => Assert.Equal(1, a.DoctorId));
    }

    [Fact]
    public async Task GetByStatusAsync_ShouldReturnAppointmentsByStatus()
    {
        // Arrange
        var repository = new MockAppointmentRepository();
        await repository.AddAsync(new Appointment { Id = 1, Status = AppointmentStatus.Pending });
        await repository.AddAsync(new Appointment { Id = 2, Status = AppointmentStatus.Approved });
        await repository.AddAsync(new Appointment { Id = 3, Status = AppointmentStatus.Pending });

        // Act
        var result = await repository.GetByStatusAsync(AppointmentStatus.Pending);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        Assert.All(result, a => Assert.Equal(AppointmentStatus.Pending, a.Status));
    }

    [Fact]
    public async Task AddAsync_ShouldAddAppointment()
    {
        // Arrange
        var repository = new MockAppointmentRepository();
        var appointment = new Appointment { Id = 1, PatientId = 1, DoctorId = 1 };

        // Act
        var result = await repository.AddAsync(appointment);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateAppointment()
    {
        // Arrange
        var repository = new MockAppointmentRepository();
        var appointment = new Appointment { Id = 1, Status = AppointmentStatus.Pending };
        await repository.AddAsync(appointment);

        // Act
        appointment.Status = AppointmentStatus.Approved;
        await repository.UpdateAsync(appointment);
        var result = await repository.GetByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(AppointmentStatus.Approved, result.Status);
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveAppointment()
    {
        // Arrange
        var repository = new MockAppointmentRepository();
        var appointment = new Appointment { Id = 1, PatientId = 1, DoctorId = 1 };
        await repository.AddAsync(appointment);

        // Act
        await repository.DeleteAsync(1);
        var result = await repository.GetByIdAsync(1);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task ExistsAsync_ShouldReturnTrue_WhenExists()
    {
        // Arrange
        var repository = new MockAppointmentRepository();
        await repository.AddAsync(new Appointment { Id = 1 });

        // Act
        var result = await repository.ExistsAsync(1);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task ExistsAsync_ShouldReturnFalse_WhenNotExists()
    {
        // Arrange
        var repository = new MockAppointmentRepository();

        // Act
        var result = await repository.ExistsAsync(999);

        // Assert
        Assert.False(result);
    }
}
