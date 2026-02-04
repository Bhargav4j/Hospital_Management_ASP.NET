using Xunit;
using ClinicManagement.Domain.Entities;
using ClinicManagement.Domain.Interfaces.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ClinicManagement.Domain.Interfaces.Repositories.Tests;

public class IFeedbackRepositoryTests
{
    private class MockFeedbackRepository : IFeedbackRepository
    {
        private readonly List<Feedback> _feedbacks = new List<Feedback>();

        public Task<IEnumerable<Feedback>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return Task.FromResult<IEnumerable<Feedback>>(_feedbacks);
        }

        public Task<Feedback?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(_feedbacks.Find(f => f.Id == id));
        }

        public Task<IEnumerable<Feedback>> GetByPatientIdAsync(int patientId, CancellationToken cancellationToken = default)
        {
            return Task.FromResult<IEnumerable<Feedback>>(_feedbacks.FindAll(f => f.PatientId == patientId));
        }

        public Task<Feedback> AddAsync(Feedback feedback, CancellationToken cancellationToken = default)
        {
            _feedbacks.Add(feedback);
            return Task.FromResult(feedback);
        }

        public Task UpdateAsync(Feedback feedback, CancellationToken cancellationToken = default)
        {
            var existing = _feedbacks.Find(f => f.Id == feedback.Id);
            if (existing != null)
            {
                _feedbacks.Remove(existing);
                _feedbacks.Add(feedback);
            }
            return Task.CompletedTask;
        }

        public Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var feedback = _feedbacks.Find(f => f.Id == id);
            if (feedback != null) _feedbacks.Remove(feedback);
            return Task.CompletedTask;
        }

        public Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(_feedbacks.Exists(f => f.Id == id));
        }
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllFeedbacks()
    {
        var repository = new MockFeedbackRepository();
        await repository.AddAsync(new Feedback { Id = 1, Message = "Good" });
        await repository.AddAsync(new Feedback { Id = 2, Message = "Excellent" });

        var result = await repository.GetAllAsync();

        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnFeedback_WhenExists()
    {
        var repository = new MockFeedbackRepository();
        await repository.AddAsync(new Feedback { Id = 1, Message = "Great service" });

        var result = await repository.GetByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal("Great service", result.Message);
    }

    [Fact]
    public async Task GetByPatientIdAsync_ShouldReturnPatientFeedbacks()
    {
        var repository = new MockFeedbackRepository();
        await repository.AddAsync(new Feedback { Id = 1, PatientId = 1 });
        await repository.AddAsync(new Feedback { Id = 2, PatientId = 1 });
        await repository.AddAsync(new Feedback { Id = 3, PatientId = 2 });

        var result = await repository.GetByPatientIdAsync(1);

        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task ExistsAsync_ShouldReturnTrue_WhenExists()
    {
        var repository = new MockFeedbackRepository();
        await repository.AddAsync(new Feedback { Id = 1 });

        var result = await repository.ExistsAsync(1);

        Assert.True(result);
    }
}
