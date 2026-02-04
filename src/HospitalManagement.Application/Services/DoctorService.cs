using AutoMapper;
using HospitalManagement.Application.DTOs;
using HospitalManagement.Application.Interfaces;
using HospitalManagement.Domain.Entities;
using HospitalManagement.Domain.Interfaces.Repositories;
using HospitalManagement.Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace HospitalManagement.Application.Services;

/// <summary>
/// Service implementation for Doctor operations
/// </summary>
public class DoctorService : IDoctorService
{
    private readonly IDoctorRepository _doctorRepository;
    private readonly IDepartmentRepository _departmentRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<DoctorService> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="DoctorService"/> class
    /// </summary>
    /// <param name="doctorRepository">Doctor repository</param>
    /// <param name="departmentRepository">Department repository</param>
    /// <param name="mapper">AutoMapper instance</param>
    /// <param name="logger">Logger instance</param>
    public DoctorService(
        IDoctorRepository doctorRepository,
        IDepartmentRepository departmentRepository,
        IMapper mapper,
        ILogger<DoctorService> logger)
    {
        _doctorRepository = doctorRepository ?? throw new ArgumentNullException(nameof(doctorRepository));
        _departmentRepository = departmentRepository ?? throw new ArgumentNullException(nameof(departmentRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<DoctorDto>> GetAllDoctorsAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving all doctors");
            var doctors = await _doctorRepository.GetAllAsync(cancellationToken);
            var doctorDtos = _mapper.Map<IEnumerable<DoctorDto>>(doctors);
            _logger.LogInformation("Successfully retrieved {Count} doctors", doctorDtos.Count());
            return doctorDtos;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving all doctors");
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<DoctorDto?> GetDoctorByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving doctor with ID: {DoctorId}", id);

            if (id <= 0)
            {
                _logger.LogWarning("Invalid doctor ID provided: {DoctorId}", id);
                throw new ArgumentException("Doctor ID must be greater than zero", nameof(id));
            }

            var doctor = await _doctorRepository.GetByIdAsync(id, cancellationToken);

            if (doctor == null)
            {
                _logger.LogWarning("Doctor not found with ID: {DoctorId}", id);
                return null;
            }

            var doctorDto = _mapper.Map<DoctorDto>(doctor);
            _logger.LogInformation("Successfully retrieved doctor with ID: {DoctorId}", id);
            return doctorDto;
        }
        catch (ArgumentException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving doctor with ID: {DoctorId}", id);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<DoctorDto?> GetDoctorByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving doctor with email: {Email}", email);

            if (string.IsNullOrWhiteSpace(email))
            {
                _logger.LogWarning("Invalid email provided");
                throw new ArgumentException("Email cannot be null or empty", nameof(email));
            }

            var doctor = await _doctorRepository.GetByEmailAsync(email, cancellationToken);

            if (doctor == null)
            {
                _logger.LogWarning("Doctor not found with email: {Email}", email);
                return null;
            }

            var doctorDto = _mapper.Map<DoctorDto>(doctor);
            _logger.LogInformation("Successfully retrieved doctor with email: {Email}", email);
            return doctorDto;
        }
        catch (ArgumentException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving doctor with email: {Email}", email);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<DoctorDto>> GetDoctorsByDepartmentAsync(int deptNo, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving doctors for department: {DeptNo}", deptNo);

            if (deptNo <= 0)
            {
                _logger.LogWarning("Invalid department number provided: {DeptNo}", deptNo);
                throw new ArgumentException("Department number must be greater than zero", nameof(deptNo));
            }

            var doctors = await _doctorRepository.GetByDepartmentAsync(deptNo, cancellationToken);
            var doctorDtos = _mapper.Map<IEnumerable<DoctorDto>>(doctors);
            _logger.LogInformation("Successfully retrieved {Count} doctors for department: {DeptNo}", doctorDtos.Count(), deptNo);
            return doctorDtos;
        }
        catch (ArgumentException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving doctors for department: {DeptNo}", deptNo);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<DoctorDto>> GetDoctorsBySpecializationAsync(string specialization, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving doctors with specialization: {Specialization}", specialization);

            if (string.IsNullOrWhiteSpace(specialization))
            {
                _logger.LogWarning("Invalid specialization provided");
                throw new ArgumentException("Specialization cannot be null or empty", nameof(specialization));
            }

            var allDoctors = await _doctorRepository.GetAllAsync(cancellationToken);
            var doctors = allDoctors.Where(d => d.Specialization.Contains(specialization, StringComparison.OrdinalIgnoreCase));
            var doctorDtos = _mapper.Map<IEnumerable<DoctorDto>>(doctors);
            _logger.LogInformation("Successfully retrieved {Count} doctors with specialization: {Specialization}", doctorDtos.Count(), specialization);
            return doctorDtos;
        }
        catch (ArgumentException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving doctors with specialization: {Specialization}", specialization);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<DoctorDto> CreateDoctorAsync(DoctorCreateDto doctorCreateDto, string createdBy, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Creating new doctor with email: {Email}", doctorCreateDto.Email);

            if (doctorCreateDto == null)
            {
                throw new ArgumentNullException(nameof(doctorCreateDto));
            }

            if (string.IsNullOrWhiteSpace(createdBy))
            {
                throw new ArgumentException("CreatedBy cannot be null or empty", nameof(createdBy));
            }

            // Check if doctor with email already exists
            var existingDoctor = await _doctorRepository.GetByEmailAsync(doctorCreateDto.Email, cancellationToken);
            if (existingDoctor != null)
            {
                _logger.LogWarning("Doctor with email {Email} already exists", doctorCreateDto.Email);
                throw new InvalidOperationException($"Doctor with email {doctorCreateDto.Email} already exists");
            }

            // Validate department exists
            var departmentExists = await _departmentRepository.ExistsAsync(doctorCreateDto.DeptNo, cancellationToken);
            if (!departmentExists)
            {
                _logger.LogWarning("Department with number {DeptNo} does not exist", doctorCreateDto.DeptNo);
                throw new InvalidOperationException($"Department with number {doctorCreateDto.DeptNo} does not exist");
            }

            var doctor = _mapper.Map<Doctor>(doctorCreateDto);
            doctor.CreatedDate = DateTime.UtcNow;
            doctor.CreatedBy = createdBy;
            doctor.IsActive = true;
            doctor.Status = true;
            doctor.ReputationIndex = 0.0f;
            doctor.PatientsTreated = 0;

            var createdDoctor = await _doctorRepository.AddAsync(doctor, cancellationToken);
            var doctorDto = _mapper.Map<DoctorDto>(createdDoctor);

            _logger.LogInformation("Successfully created doctor with ID: {DoctorId}", createdDoctor.DoctorID);
            return doctorDto;
        }
        catch (ArgumentException)
        {
            throw;
        }
        catch (InvalidOperationException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while creating doctor");
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<DoctorDto?> UpdateDoctorAsync(DoctorUpdateDto doctorUpdateDto, string modifiedBy, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Updating doctor with ID: {DoctorId}", doctorUpdateDto.DoctorID);

            if (doctorUpdateDto == null)
            {
                throw new ArgumentNullException(nameof(doctorUpdateDto));
            }

            if (string.IsNullOrWhiteSpace(modifiedBy))
            {
                throw new ArgumentException("ModifiedBy cannot be null or empty", nameof(modifiedBy));
            }

            var existingDoctor = await _doctorRepository.GetByIdAsync(doctorUpdateDto.DoctorID, cancellationToken);

            if (existingDoctor == null)
            {
                _logger.LogWarning("Doctor not found with ID: {DoctorId}", doctorUpdateDto.DoctorID);
                return null;
            }

            // Validate department exists if changed
            if (existingDoctor.DeptNo != doctorUpdateDto.DeptNo)
            {
                var departmentExists = await _departmentRepository.ExistsAsync(doctorUpdateDto.DeptNo, cancellationToken);
                if (!departmentExists)
                {
                    _logger.LogWarning("Department with number {DeptNo} does not exist", doctorUpdateDto.DeptNo);
                    throw new InvalidOperationException($"Department with number {doctorUpdateDto.DeptNo} does not exist");
                }
            }

            _mapper.Map(doctorUpdateDto, existingDoctor);
            existingDoctor.ModifiedDate = DateTime.UtcNow;
            existingDoctor.ModifiedBy = modifiedBy;

            await _doctorRepository.UpdateAsync(existingDoctor, cancellationToken);
            var doctorDto = _mapper.Map<DoctorDto>(existingDoctor);

            _logger.LogInformation("Successfully updated doctor with ID: {DoctorId}", doctorUpdateDto.DoctorID);
            return doctorDto;
        }
        catch (ArgumentException)
        {
            throw;
        }
        catch (InvalidOperationException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while updating doctor with ID: {DoctorId}", doctorUpdateDto.DoctorID);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<bool> DeleteDoctorAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Deleting doctor with ID: {DoctorId}", id);

            if (id <= 0)
            {
                _logger.LogWarning("Invalid doctor ID provided: {DoctorId}", id);
                throw new ArgumentException("Doctor ID must be greater than zero", nameof(id));
            }

            var exists = await _doctorRepository.ExistsAsync(id, cancellationToken);

            if (!exists)
            {
                _logger.LogWarning("Doctor not found with ID: {DoctorId}", id);
                return false;
            }

            await _doctorRepository.DeleteAsync(id, cancellationToken);
            _logger.LogInformation("Successfully deleted doctor with ID: {DoctorId}", id);
            return true;
        }
        catch (ArgumentException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while deleting doctor with ID: {DoctorId}", id);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<DoctorDto>> GetActiveDoctorsAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving active doctors");
            var doctors = await _doctorRepository.GetAllAsync(cancellationToken);
            var activeDoctors = doctors.Where(d => d.IsActive && d.Status);
            var doctorDtos = _mapper.Map<IEnumerable<DoctorDto>>(activeDoctors);
            _logger.LogInformation("Successfully retrieved {Count} active doctors", doctorDtos.Count());
            return doctorDtos;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving active doctors");
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<bool> UpdateDoctorReputationAsync(int doctorId, float newReputation, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Updating reputation for doctor ID: {DoctorId} to {Reputation}", doctorId, newReputation);

            if (doctorId <= 0)
            {
                _logger.LogWarning("Invalid doctor ID provided: {DoctorId}", doctorId);
                throw new ArgumentException("Doctor ID must be greater than zero", nameof(doctorId));
            }

            if (newReputation < 0 || newReputation > 5)
            {
                _logger.LogWarning("Invalid reputation value provided: {Reputation}", newReputation);
                throw new ArgumentException("Reputation must be between 0 and 5", nameof(newReputation));
            }

            var doctor = await _doctorRepository.GetByIdAsync(doctorId, cancellationToken);

            if (doctor == null)
            {
                _logger.LogWarning("Doctor not found with ID: {DoctorId}", doctorId);
                return false;
            }

            doctor.ReputationIndex = newReputation;
            doctor.ModifiedDate = DateTime.UtcNow;

            await _doctorRepository.UpdateAsync(doctor, cancellationToken);
            _logger.LogInformation("Successfully updated reputation for doctor ID: {DoctorId}", doctorId);
            return true;
        }
        catch (ArgumentException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while updating reputation for doctor ID: {DoctorId}", doctorId);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<bool> IncrementPatientsTreatedAsync(int doctorId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Incrementing patients treated count for doctor ID: {DoctorId}", doctorId);

            if (doctorId <= 0)
            {
                _logger.LogWarning("Invalid doctor ID provided: {DoctorId}", doctorId);
                throw new ArgumentException("Doctor ID must be greater than zero", nameof(doctorId));
            }

            var doctor = await _doctorRepository.GetByIdAsync(doctorId, cancellationToken);

            if (doctor == null)
            {
                _logger.LogWarning("Doctor not found with ID: {DoctorId}", doctorId);
                return false;
            }

            doctor.PatientsTreated++;
            doctor.ModifiedDate = DateTime.UtcNow;

            await _doctorRepository.UpdateAsync(doctor, cancellationToken);
            _logger.LogInformation("Successfully incremented patients treated count for doctor ID: {DoctorId}", doctorId);
            return true;
        }
        catch (ArgumentException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while incrementing patients treated for doctor ID: {DoctorId}", doctorId);
            throw;
        }
    }
}
