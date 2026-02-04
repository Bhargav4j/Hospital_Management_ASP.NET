using Xunit;
using ClinicManagement.Domain.Entities;
using ClinicManagement.Domain.Interfaces.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ClinicManagement.Domain.Interfaces.Repositories.Tests;

public class IBillRepositoryTests
{
    private class MockBillRepository : IBillRepository
    {
        private readonly List<Bill> _bills = new List<Bill>();

        public Task<IEnumerable<Bill>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return Task.FromResult<IEnumerable<Bill>>(_bills);
        }

        public Task<Bill?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(_bills.Find(b => b.Id == id));
        }

        public Task<IEnumerable<Bill>> GetByPatientIdAsync(int patientId, CancellationToken cancellationToken = default)
        {
            return Task.FromResult<IEnumerable<Bill>>(_bills.FindAll(b => b.PatientId == patientId));
        }

        public Task<Bill> AddAsync(Bill bill, CancellationToken cancellationToken = default)
        {
            _bills.Add(bill);
            return Task.FromResult(bill);
        }

        public Task UpdateAsync(Bill bill, CancellationToken cancellationToken = default)
        {
            var existing = _bills.Find(b => b.Id == bill.Id);
            if (existing != null)
            {
                _bills.Remove(existing);
                _bills.Add(bill);
            }
            return Task.CompletedTask;
        }

        public Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var bill = _bills.Find(b => b.Id == id);
            if (bill != null) _bills.Remove(bill);
            return Task.CompletedTask;
        }

        public Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(_bills.Exists(b => b.Id == id));
        }
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllBills()
    {
        var repository = new MockBillRepository();
        await repository.AddAsync(new Bill { Id = 1, Amount = 100 });
        await repository.AddAsync(new Bill { Id = 2, Amount = 200 });

        var result = await repository.GetAllAsync();

        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnBill_WhenExists()
    {
        var repository = new MockBillRepository();
        await repository.AddAsync(new Bill { Id = 1, Amount = 150 });

        var result = await repository.GetByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal(150, result.Amount);
    }

    [Fact]
    public async Task GetByPatientIdAsync_ShouldReturnPatientBills()
    {
        var repository = new MockBillRepository();
        await repository.AddAsync(new Bill { Id = 1, PatientId = 1 });
        await repository.AddAsync(new Bill { Id = 2, PatientId = 1 });
        await repository.AddAsync(new Bill { Id = 3, PatientId = 2 });

        var result = await repository.GetByPatientIdAsync(1);

        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task ExistsAsync_ShouldReturnTrue_WhenExists()
    {
        var repository = new MockBillRepository();
        await repository.AddAsync(new Bill { Id = 1 });

        var result = await repository.ExistsAsync(1);

        Assert.True(result);
    }
}
