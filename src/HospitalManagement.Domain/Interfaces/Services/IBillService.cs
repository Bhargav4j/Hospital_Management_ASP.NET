namespace HospitalManagement.Domain.Interfaces.Services;

/// <summary>
/// Service interface for billing business logic operations.
/// Provides methods for managing billing data using DTOs for data transfer.
/// </summary>
public interface IBillService
{
    /// <summary>
    /// Retrieves all bills.
    /// </summary>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>A collection of bill DTOs.</returns>
    Task<IEnumerable<BillDto>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a bill by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the bill.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>The bill DTO if found; otherwise, null.</returns>
    Task<BillDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new bill.
    /// </summary>
    /// <param name="dto">The bill creation DTO containing billing information.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>The created bill DTO with generated identifier.</returns>
    Task<BillDto> CreateAsync(BillCreateDto dto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing bill.
    /// </summary>
    /// <param name="id">The unique identifier of the bill to update.</param>
    /// <param name="dto">The bill update DTO containing updated information.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>The updated bill DTO.</returns>
    Task<BillDto> UpdateAsync(int id, BillUpdateDto dto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a bill.
    /// </summary>
    /// <param name="id">The unique identifier of the bill to delete.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>True if the bill was deleted; otherwise, false.</returns>
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Searches for bills based on a search term.
    /// </summary>
    /// <param name="searchTerm">The search term to match against bill properties.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>A collection of bill DTOs matching the search criteria.</returns>
    Task<IEnumerable<BillDto>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);
}

/// <summary>
/// Data Transfer Object for bill information.
/// </summary>
public record BillDto
{
    public int BillId { get; init; }
    public int PatientId { get; init; }
    public int? AppointmentId { get; init; }
    public DateTime BillDate { get; init; }
    public decimal ConsultationCharges { get; init; }
    public decimal MedicineCharges { get; init; }
    public decimal LabCharges { get; init; }
    public decimal OtherCharges { get; init; }
    public decimal TotalAmount { get; init; }
    public required string PaymentStatus { get; init; }
    public string? PaymentMethod { get; init; }
    public DateTime? PaymentDate { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime? UpdatedAt { get; init; }
}

/// <summary>
/// Data Transfer Object for creating a new bill.
/// </summary>
public record BillCreateDto
{
    public int PatientId { get; init; }
    public int? AppointmentId { get; init; }
    public DateTime BillDate { get; init; }
    public decimal ConsultationCharges { get; init; }
    public decimal MedicineCharges { get; init; }
    public decimal LabCharges { get; init; }
    public decimal OtherCharges { get; init; }
    public required string PaymentStatus { get; init; }
    public string? PaymentMethod { get; init; }
    public DateTime? PaymentDate { get; init; }
}

/// <summary>
/// Data Transfer Object for updating an existing bill.
/// </summary>
public record BillUpdateDto
{
    public int PatientId { get; init; }
    public int? AppointmentId { get; init; }
    public DateTime BillDate { get; init; }
    public decimal ConsultationCharges { get; init; }
    public decimal MedicineCharges { get; init; }
    public decimal LabCharges { get; init; }
    public decimal OtherCharges { get; init; }
    public required string PaymentStatus { get; init; }
    public string? PaymentMethod { get; init; }
    public DateTime? PaymentDate { get; init; }
}
