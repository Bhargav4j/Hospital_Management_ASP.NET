using HospitalManagement.Application.DTOs;

namespace HospitalManagement.Application.Interfaces;

/// <summary>
/// Service interface for Department operations
/// </summary>
public interface IDepartmentService
{
    /// <summary>
    /// Gets all departments asynchronously
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Collection of department DTOs</returns>
    Task<IEnumerable<DepartmentDto>> GetAllDepartmentsAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a department by ID asynchronously
    /// </summary>
    /// <param name="id">Department identifier</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Department DTO if found, null otherwise</returns>
    Task<DepartmentDto?> GetDepartmentByIdAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a department by name asynchronously
    /// </summary>
    /// <param name="name">Department name</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Department DTO if found, null otherwise</returns>
    Task<DepartmentDto?> GetDepartmentByNameAsync(string name, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets active departments asynchronously
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Collection of active department DTOs</returns>
    Task<IEnumerable<DepartmentDto>> GetActiveDepartmentsAsync(CancellationToken cancellationToken = default);
}
