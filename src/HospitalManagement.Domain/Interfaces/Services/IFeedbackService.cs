using HospitalManagement.Domain.DTOs;

namespace HospitalManagement.Domain.Interfaces.Services;

public interface IFeedbackService
{
    Task<IEnumerable<FeedbackDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<FeedbackDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<FeedbackDto>> GetByPatientIdAsync(int patientId, CancellationToken cancellationToken = default);
    Task<FeedbackDto> CreateAsync(FeedbackCreateDto feedbackCreateDto, CancellationToken cancellationToken = default);
    Task UpdateAsync(int id, FeedbackUpdateDto feedbackUpdateDto, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<FeedbackDto>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);
}
