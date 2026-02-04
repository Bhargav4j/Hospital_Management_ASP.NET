using HospitalManagement.Application.DTOs;

namespace HospitalManagement.Application.Interfaces;

/// <summary>
/// Service interface for OtherStaff operations
/// </summary>
public interface IOtherStaffService
{
    /// <summary>
    /// Gets all staff members asynchronously
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Collection of staff DTOs</returns>
    Task<IEnumerable<OtherStaffDto>> GetAllStaffAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a staff member by ID asynchronously
    /// </summary>
    /// <param name="id">Staff identifier</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Staff DTO if found, null otherwise</returns>
    Task<OtherStaffDto?> GetStaffByIdAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets staff members by designation asynchronously
    /// </summary>
    /// <param name="designation">Staff designation</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Collection of staff DTOs</returns>
    Task<IEnumerable<OtherStaffDto>> GetStaffByDesignationAsync(string designation, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets active staff members asynchronously
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Collection of active staff DTOs</returns>
    Task<IEnumerable<OtherStaffDto>> GetActiveStaffAsync(CancellationToken cancellationToken = default);
}
