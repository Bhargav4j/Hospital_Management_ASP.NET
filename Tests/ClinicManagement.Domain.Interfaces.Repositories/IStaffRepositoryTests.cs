using Xunit;
using ClinicManagement.Domain.Entities;
using ClinicManagement.Domain.Interfaces.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ClinicManagement.Domain.Interfaces.Repositories.Tests;

public class IStaffRepositoryTests
{
    private class MockStaffRepository : IStaffRepository
    {
        private readonly List<Staff> _staff = new List<Staff>();

        public Task<IEnumerable<Staff>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return Task.FromResult<IEnumerable<Staff>>(_staff);
        }

        public Task<Staff?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(_staff.Find(s => s.Id == id));
        }

        public Task<Staff?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(_staff.Find(s => s.Email == email));
        }

        public Task<Staff> AddAsync(Staff staff, CancellationToken cancellationToken = default)
        {
            _staff.Add(staff);
            return Task.FromResult(staff);
        }

        public Task UpdateAsync(Staff staff, CancellationToken cancellationToken = default)
        {
            var existing = _staff.Find(s => s.Id == staff.Id);
            if (existing != null)
            {
                _staff.Remove(existing);
                _staff.Add(staff);
            }
            return Task.CompletedTask;
        }

        public Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var staffMember = _staff.Find(s => s.Id == id);
            if (staffMember != null) _staff.Remove(staffMember);
            return Task.CompletedTask;
        }

        public Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(_staff.Exists(s => s.Id == id));
        }
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllStaff()
    {
        var repository = new MockStaffRepository();
        await repository.AddAsync(new Staff { Id = 1, Name = "Staff A" });
        await repository.AddAsync(new Staff { Id = 2, Name = "Staff B" });

        var result = await repository.GetAllAsync();

        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnStaff_WhenExists()
    {
        var repository = new MockStaffRepository();
        await repository.AddAsync(new Staff { Id = 1, Name = "Test Staff" });

        var result = await repository.GetByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal("Test Staff", result.Name);
    }

    [Fact]
    public async Task GetByEmailAsync_ShouldReturnStaff_WhenExists()
    {
        var repository = new MockStaffRepository();
        await repository.AddAsync(new Staff { Id = 1, Email = "staff@test.com" });

        var result = await repository.GetByEmailAsync("staff@test.com");

        Assert.NotNull(result);
        Assert.Equal("staff@test.com", result.Email);
    }

    [Fact]
    public async Task ExistsAsync_ShouldReturnTrue_WhenExists()
    {
        var repository = new MockStaffRepository();
        await repository.AddAsync(new Staff { Id = 1 });

        var result = await repository.ExistsAsync(1);

        Assert.True(result);
    }
}
