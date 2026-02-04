using AutoMapper;
using HospitalManagement.Application.DTOs;
using HospitalManagement.Application.Interfaces;
using HospitalManagement.Domain.Interfaces.Repositories;
using HospitalManagement.Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace HospitalManagement.Application.Services;

/// <summary>
/// Service implementation for Department operations
/// </summary>
public class DepartmentService : IDepartmentService
{
    private readonly IDepartmentRepository _departmentRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<DepartmentService> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="DepartmentService"/> class
    /// </summary>
    /// <param name="departmentRepository">Department repository</param>
    /// <param name="mapper">AutoMapper instance</param>
    /// <param name="logger">Logger instance</param>
    public DepartmentService(
        IDepartmentRepository departmentRepository,
        IMapper mapper,
        ILogger<DepartmentService> logger)
    {
        _departmentRepository = departmentRepository ?? throw new ArgumentNullException(nameof(departmentRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<DepartmentDto>> GetAllDepartmentsAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving all departments");
            var departments = await _departmentRepository.GetAllAsync(cancellationToken);
            var departmentDtos = _mapper.Map<IEnumerable<DepartmentDto>>(departments);
            _logger.LogInformation("Successfully retrieved {Count} departments", departmentDtos.Count());
            return departmentDtos;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving all departments");
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<DepartmentDto?> GetDepartmentByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving department with ID: {DeptNo}", id);

            if (id <= 0)
            {
                _logger.LogWarning("Invalid department ID provided: {DeptNo}", id);
                throw new ArgumentException("Department ID must be greater than zero", nameof(id));
            }

            var department = await _departmentRepository.GetByIdAsync(id, cancellationToken);

            if (department == null)
            {
                _logger.LogWarning("Department not found with ID: {DeptNo}", id);
                return null;
            }

            var departmentDto = _mapper.Map<DepartmentDto>(department);
            _logger.LogInformation("Successfully retrieved department with ID: {DeptNo}", id);
            return departmentDto;
        }
        catch (ArgumentException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving department with ID: {DeptNo}", id);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<DepartmentDto?> GetDepartmentByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving department with name: {DeptName}", name);

            if (string.IsNullOrWhiteSpace(name))
            {
                _logger.LogWarning("Invalid department name provided");
                throw new ArgumentException("Department name cannot be null or empty", nameof(name));
            }

            var departments = await _departmentRepository.GetAllAsync(cancellationToken);
            var department = departments.FirstOrDefault(d => d.DeptName.Equals(name, StringComparison.OrdinalIgnoreCase));

            if (department == null)
            {
                _logger.LogWarning("Department not found with name: {DeptName}", name);
                return null;
            }

            var departmentDto = _mapper.Map<DepartmentDto>(department);
            _logger.LogInformation("Successfully retrieved department with name: {DeptName}", name);
            return departmentDto;
        }
        catch (ArgumentException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving department with name: {DeptName}", name);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<DepartmentDto>> GetActiveDepartmentsAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving active departments");
            var departments = await _departmentRepository.GetAllAsync(cancellationToken);
            var activeDepartments = departments.Where(d => d.IsActive);
            var departmentDtos = _mapper.Map<IEnumerable<DepartmentDto>>(activeDepartments);
            _logger.LogInformation("Successfully retrieved {Count} active departments", departmentDtos.Count());
            return departmentDtos;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving active departments");
            throw;
        }
    }
}
