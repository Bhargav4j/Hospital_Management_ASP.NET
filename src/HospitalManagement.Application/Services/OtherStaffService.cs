using AutoMapper;
using HospitalManagement.Application.DTOs;
using HospitalManagement.Application.Interfaces;
using HospitalManagement.Domain.Interfaces.Repositories;
using HospitalManagement.Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace HospitalManagement.Application.Services;

/// <summary>
/// Service implementation for OtherStaff operations
/// </summary>
public class OtherStaffService : IOtherStaffService
{
    private readonly IOtherStaffRepository _otherStaffRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<OtherStaffService> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="OtherStaffService"/> class
    /// </summary>
    /// <param name="otherStaffRepository">OtherStaff repository</param>
    /// <param name="mapper">AutoMapper instance</param>
    /// <param name="logger">Logger instance</param>
    public OtherStaffService(
        IOtherStaffRepository otherStaffRepository,
        IMapper mapper,
        ILogger<OtherStaffService> logger)
    {
        _otherStaffRepository = otherStaffRepository ?? throw new ArgumentNullException(nameof(otherStaffRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<OtherStaffDto>> GetAllStaffAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving all staff members");
            var staff = await _otherStaffRepository.GetAllAsync(cancellationToken);
            var staffDtos = _mapper.Map<IEnumerable<OtherStaffDto>>(staff);
            _logger.LogInformation("Successfully retrieved {Count} staff members", staffDtos.Count());
            return staffDtos;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving all staff members");
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<OtherStaffDto?> GetStaffByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving staff member with ID: {StaffId}", id);

            if (id <= 0)
            {
                _logger.LogWarning("Invalid staff ID provided: {StaffId}", id);
                throw new ArgumentException("Staff ID must be greater than zero", nameof(id));
            }

            var staff = await _otherStaffRepository.GetByIdAsync(id, cancellationToken);

            if (staff == null)
            {
                _logger.LogWarning("Staff member not found with ID: {StaffId}", id);
                return null;
            }

            var staffDto = _mapper.Map<OtherStaffDto>(staff);
            _logger.LogInformation("Successfully retrieved staff member with ID: {StaffId}", id);
            return staffDto;
        }
        catch (ArgumentException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving staff member with ID: {StaffId}", id);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<OtherStaffDto>> GetStaffByDesignationAsync(string designation, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving staff members with designation: {Designation}", designation);

            if (string.IsNullOrWhiteSpace(designation))
            {
                _logger.LogWarning("Invalid designation provided");
                throw new ArgumentException("Designation cannot be null or empty", nameof(designation));
            }

            var allStaff = await _otherStaffRepository.GetAllAsync(cancellationToken);
            var staff = allStaff.Where(s => s.Designation.Equals(designation, StringComparison.OrdinalIgnoreCase));
            var staffDtos = _mapper.Map<IEnumerable<OtherStaffDto>>(staff);
            _logger.LogInformation("Successfully retrieved {Count} staff members with designation: {Designation}", staffDtos.Count(), designation);
            return staffDtos;
        }
        catch (ArgumentException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving staff members with designation: {Designation}", designation);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<OtherStaffDto>> GetActiveStaffAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving active staff members");
            var staff = await _otherStaffRepository.GetAllAsync(cancellationToken);
            var activeStaff = staff.Where(s => s.IsActive);
            var staffDtos = _mapper.Map<IEnumerable<OtherStaffDto>>(activeStaff);
            _logger.LogInformation("Successfully retrieved {Count} active staff members", staffDtos.Count());
            return staffDtos;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving active staff members");
            throw;
        }
    }
}
