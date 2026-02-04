using Xunit;
using ClinicManagement.Domain.Entities;
using ClinicManagement.Domain.Interfaces.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ClinicManagement.Domain.Interfaces.Repositories.Tests;

public class IClinicRepositoryTests
{
    private class MockClinicRepository : IClinicRepository
    {
        private readonly List<Clinic> _clinics = new List<Clinic>();

        public Task<IEnumerable<Clinic>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return Task.FromResult<IEnumerable<Clinic>>(_clinics);
        }

        public Task<Clinic?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(_clinics.Find(c => c.Id == id));
        }

        public Task<Clinic> AddAsync(Clinic clinic, CancellationToken cancellationToken = default)
        {
            _clinics.Add(clinic);
            return Task.FromResult(clinic);
        }

        public Task UpdateAsync(Clinic clinic, CancellationToken cancellationToken = default)
        {
            var existing = _clinics.Find(c => c.Id == clinic.Id);
            if (existing != null)
            {
                _clinics.Remove(existing);
                _clinics.Add(clinic);
            }
            return Task.CompletedTask;
        }

        public Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var clinic = _clinics.Find(c => c.Id == id);
            if (clinic != null) _clinics.Remove(clinic);
            return Task.CompletedTask;
        }

        public Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(_clinics.Exists(c => c.Id == id));
        }

        public Task<IEnumerable<Clinic>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
        {
            return Task.FromResult<IEnumerable<Clinic>>(_clinics.FindAll(c => c.Name.Contains(searchTerm)));
        }
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllClinics()
    {
        var repository = new MockClinicRepository();
        await repository.AddAsync(new Clinic { Id = 1, Name = "Clinic A" });
        await repository.AddAsync(new Clinic { Id = 2, Name = "Clinic B" });

        var result = await repository.GetAllAsync();

        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnClinic_WhenExists()
    {
        var repository = new MockClinicRepository();
        await repository.AddAsync(new Clinic { Id = 1, Name = "Test Clinic" });

        var result = await repository.GetByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal("Test Clinic", result.Name);
    }

    [Fact]
    public async Task AddAsync_ShouldAddClinic()
    {
        var repository = new MockClinicRepository();
        var clinic = new Clinic { Id = 1, Name = "New Clinic" };

        var result = await repository.AddAsync(clinic);

        Assert.NotNull(result);
        Assert.Equal("New Clinic", result.Name);
    }

    [Fact]
    public async Task ExistsAsync_ShouldReturnTrue_WhenExists()
    {
        var repository = new MockClinicRepository();
        await repository.AddAsync(new Clinic { Id = 1, Name = "Test" });

        var result = await repository.ExistsAsync(1);

        Assert.True(result);
    }

    [Fact]
    public async Task SearchAsync_ShouldReturnMatchingClinics()
    {
        var repository = new MockClinicRepository();
        await repository.AddAsync(new Clinic { Id = 1, Name = "City Clinic" });
        await repository.AddAsync(new Clinic { Id = 2, Name = "Town Clinic" });

        var result = await repository.SearchAsync("City");

        Assert.Single(result);
    }
}
