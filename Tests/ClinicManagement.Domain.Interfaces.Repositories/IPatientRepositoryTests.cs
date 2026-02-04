using Xunit;
using ClinicManagement.Domain.Entities;
using ClinicManagement.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ClinicManagement.Domain.Interfaces.Repositories.Tests;

/// <summary>
/// Test class for IPatientRepository interface implementation
/// </summary>
public class IPatientRepositoryTests
{
    private class MockPatientRepository : IPatientRepository
    {
        private readonly List<Patient> _patients = new List<Patient>();

        public Task<IEnumerable<Patient>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return Task.FromResult<IEnumerable<Patient>>(_patients);
        }

        public Task<Patient?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var patient = _patients.Find(p => p.Id == id);
            return Task.FromResult(patient);
        }

        public Task<Patient?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            var patient = _patients.Find(p => p.Email == email);
            return Task.FromResult(patient);
        }

        public Task<Patient> AddAsync(Patient patient, CancellationToken cancellationToken = default)
        {
            _patients.Add(patient);
            return Task.FromResult(patient);
        }

        public Task UpdateAsync(Patient patient, CancellationToken cancellationToken = default)
        {
            var existing = _patients.Find(p => p.Id == patient.Id);
            if (existing != null)
            {
                _patients.Remove(existing);
                _patients.Add(patient);
            }
            return Task.CompletedTask;
        }

        public Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var patient = _patients.Find(p => p.Id == id);
            if (patient != null)
            {
                _patients.Remove(patient);
            }
            return Task.CompletedTask;
        }

        public Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
        {
            var exists = _patients.Exists(p => p.Id == id);
            return Task.FromResult(exists);
        }

        public Task<IEnumerable<Patient>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
        {
            var results = _patients.FindAll(p => p.Name.Contains(searchTerm) || p.Email.Contains(searchTerm));
            return Task.FromResult<IEnumerable<Patient>>(results);
        }
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllPatients()
    {
        // Arrange
        var repository = new MockPatientRepository();
        await repository.AddAsync(new Patient { Id = 1, Name = "Patient 1" });
        await repository.AddAsync(new Patient { Id = 2, Name = "Patient 2" });

        // Act
        var result = await repository.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, ((List<Patient>)result).Count);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnPatient_WhenExists()
    {
        // Arrange
        var repository = new MockPatientRepository();
        var patient = new Patient { Id = 1, Name = "John Doe" };
        await repository.AddAsync(patient);

        // Act
        var result = await repository.GetByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("John Doe", result.Name);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenNotExists()
    {
        // Arrange
        var repository = new MockPatientRepository();

        // Act
        var result = await repository.GetByIdAsync(999);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetByEmailAsync_ShouldReturnPatient_WhenExists()
    {
        // Arrange
        var repository = new MockPatientRepository();
        var patient = new Patient { Id = 1, Email = "test@example.com" };
        await repository.AddAsync(patient);

        // Act
        var result = await repository.GetByEmailAsync("test@example.com");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("test@example.com", result.Email);
    }

    [Fact]
    public async Task GetByEmailAsync_ShouldReturnNull_WhenNotExists()
    {
        // Arrange
        var repository = new MockPatientRepository();

        // Act
        var result = await repository.GetByEmailAsync("nonexistent@example.com");

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task AddAsync_ShouldAddPatient()
    {
        // Arrange
        var repository = new MockPatientRepository();
        var patient = new Patient { Id = 1, Name = "New Patient" };

        // Act
        var result = await repository.AddAsync(patient);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("New Patient", result.Name);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdatePatient()
    {
        // Arrange
        var repository = new MockPatientRepository();
        var patient = new Patient { Id = 1, Name = "Original Name" };
        await repository.AddAsync(patient);

        // Act
        patient.Name = "Updated Name";
        await repository.UpdateAsync(patient);
        var result = await repository.GetByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Updated Name", result.Name);
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemovePatient()
    {
        // Arrange
        var repository = new MockPatientRepository();
        var patient = new Patient { Id = 1, Name = "To Delete" };
        await repository.AddAsync(patient);

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
        var repository = new MockPatientRepository();
        await repository.AddAsync(new Patient { Id = 1, Name = "Exists" });

        // Act
        var result = await repository.ExistsAsync(1);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task ExistsAsync_ShouldReturnFalse_WhenNotExists()
    {
        // Arrange
        var repository = new MockPatientRepository();

        // Act
        var result = await repository.ExistsAsync(999);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task SearchAsync_ShouldReturnMatchingPatients()
    {
        // Arrange
        var repository = new MockPatientRepository();
        await repository.AddAsync(new Patient { Id = 1, Name = "John Doe", Email = "john@example.com" });
        await repository.AddAsync(new Patient { Id = 2, Name = "Jane Smith", Email = "jane@example.com" });

        // Act
        var result = await repository.SearchAsync("John");

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
    }
}
