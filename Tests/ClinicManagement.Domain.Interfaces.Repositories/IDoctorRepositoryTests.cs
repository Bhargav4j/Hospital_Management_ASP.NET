using Xunit;
using ClinicManagement.Domain.Entities;
using ClinicManagement.Domain.Interfaces.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ClinicManagement.Domain.Interfaces.Repositories.Tests;

public class IDoctorRepositoryTests
{
    private class MockDoctorRepository : IDoctorRepository
    {
        private readonly List<Doctor> _doctors = new List<Doctor>();

        public Task<IEnumerable<Doctor>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return Task.FromResult<IEnumerable<Doctor>>(_doctors);
        }

        public Task<Doctor?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(_doctors.Find(d => d.Id == id));
        }

        public Task<Doctor?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(_doctors.Find(d => d.Email == email));
        }

        public Task<IEnumerable<Doctor>> GetBySpecializationAsync(string specialization, CancellationToken cancellationToken = default)
        {
            return Task.FromResult<IEnumerable<Doctor>>(_doctors.FindAll(d => d.Specialization == specialization));
        }

        public Task<Doctor> AddAsync(Doctor doctor, CancellationToken cancellationToken = default)
        {
            _doctors.Add(doctor);
            return Task.FromResult(doctor);
        }

        public Task UpdateAsync(Doctor doctor, CancellationToken cancellationToken = default)
        {
            var existing = _doctors.Find(d => d.Id == doctor.Id);
            if (existing != null)
            {
                _doctors.Remove(existing);
                _doctors.Add(doctor);
            }
            return Task.CompletedTask;
        }

        public Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var doctor = _doctors.Find(d => d.Id == id);
            if (doctor != null) _doctors.Remove(doctor);
            return Task.CompletedTask;
        }

        public Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(_doctors.Exists(d => d.Id == id));
        }

        public Task<IEnumerable<Doctor>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
        {
            return Task.FromResult<IEnumerable<Doctor>>(_doctors.FindAll(d => d.Name.Contains(searchTerm) || d.Email.Contains(searchTerm)));
        }
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllDoctors()
    {
        var repository = new MockDoctorRepository();
        await repository.AddAsync(new Doctor { Id = 1, Name = "Dr. A" });
        await repository.AddAsync(new Doctor { Id = 2, Name = "Dr. B" });

        var result = await repository.GetAllAsync();

        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnDoctor_WhenExists()
    {
        var repository = new MockDoctorRepository();
        await repository.AddAsync(new Doctor { Id = 1, Name = "Dr. A" });

        var result = await repository.GetByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
    }

    [Fact]
    public async Task GetByEmailAsync_ShouldReturnDoctor_WhenExists()
    {
        var repository = new MockDoctorRepository();
        await repository.AddAsync(new Doctor { Id = 1, Email = "test@test.com" });

        var result = await repository.GetByEmailAsync("test@test.com");

        Assert.NotNull(result);
        Assert.Equal("test@test.com", result.Email);
    }

    [Fact]
    public async Task GetBySpecializationAsync_ShouldReturnMatchingDoctors()
    {
        var repository = new MockDoctorRepository();
        await repository.AddAsync(new Doctor { Id = 1, Specialization = "Cardiology" });
        await repository.AddAsync(new Doctor { Id = 2, Specialization = "Neurology" });

        var result = await repository.GetBySpecializationAsync("Cardiology");

        Assert.Single(result);
    }

    [Fact]
    public async Task SearchAsync_ShouldReturnMatchingDoctors()
    {
        var repository = new MockDoctorRepository();
        await repository.AddAsync(new Doctor { Id = 1, Name = "John Smith" });
        await repository.AddAsync(new Doctor { Id = 2, Name = "Jane Doe" });

        var result = await repository.SearchAsync("John");

        Assert.Single(result);
    }
}
