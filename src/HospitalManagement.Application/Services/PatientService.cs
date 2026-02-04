using AutoMapper;
using HospitalManagement.Application.DTOs;
using HospitalManagement.Application.Interfaces;
using HospitalManagement.Domain.Entities;
using HospitalManagement.Domain.Interfaces.Repositories;
using HospitalManagement.Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace HospitalManagement.Application.Services;

/// <summary>
/// Service implementation for Patient operations
/// </summary>
public class PatientService : IPatientService
{
    private readonly IPatientRepository _patientRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<PatientService> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="PatientService"/> class
    /// </summary>
    /// <param name="patientRepository">Patient repository</param>
    /// <param name="mapper">AutoMapper instance</param>
    /// <param name="logger">Logger instance</param>
    public PatientService(
        IPatientRepository patientRepository,
        IMapper mapper,
        ILogger<PatientService> logger)
    {
        _patientRepository = patientRepository ?? throw new ArgumentNullException(nameof(patientRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<PatientDto>> GetAllPatientsAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving all patients");
            var patients = await _patientRepository.GetAllAsync(cancellationToken);
            var patientDtos = _mapper.Map<IEnumerable<PatientDto>>(patients);
            _logger.LogInformation("Successfully retrieved {Count} patients", patientDtos.Count());
            return patientDtos;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving all patients");
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<PatientDto?> GetPatientByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving patient with ID: {PatientId}", id);

            if (id <= 0)
            {
                _logger.LogWarning("Invalid patient ID provided: {PatientId}", id);
                throw new ArgumentException("Patient ID must be greater than zero", nameof(id));
            }

            var patient = await _patientRepository.GetByIdAsync(id, cancellationToken);

            if (patient == null)
            {
                _logger.LogWarning("Patient not found with ID: {PatientId}", id);
                return null;
            }

            var patientDto = _mapper.Map<PatientDto>(patient);
            _logger.LogInformation("Successfully retrieved patient with ID: {PatientId}", id);
            return patientDto;
        }
        catch (ArgumentException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving patient with ID: {PatientId}", id);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<PatientDto?> GetPatientByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving patient with email: {Email}", email);

            if (string.IsNullOrWhiteSpace(email))
            {
                _logger.LogWarning("Invalid email provided");
                throw new ArgumentException("Email cannot be null or empty", nameof(email));
            }

            var patient = await _patientRepository.GetByEmailAsync(email, cancellationToken);

            if (patient == null)
            {
                _logger.LogWarning("Patient not found with email: {Email}", email);
                return null;
            }

            var patientDto = _mapper.Map<PatientDto>(patient);
            _logger.LogInformation("Successfully retrieved patient with email: {Email}", email);
            return patientDto;
        }
        catch (ArgumentException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving patient with email: {Email}", email);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<PatientDto> CreatePatientAsync(PatientCreateDto patientCreateDto, string createdBy, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Creating new patient with email: {Email}", patientCreateDto.Email);

            if (patientCreateDto == null)
            {
                throw new ArgumentNullException(nameof(patientCreateDto));
            }

            if (string.IsNullOrWhiteSpace(createdBy))
            {
                throw new ArgumentException("CreatedBy cannot be null or empty", nameof(createdBy));
            }

            // Check if patient with email already exists
            var existingPatient = await _patientRepository.GetByEmailAsync(patientCreateDto.Email, cancellationToken);
            if (existingPatient != null)
            {
                _logger.LogWarning("Patient with email {Email} already exists", patientCreateDto.Email);
                throw new InvalidOperationException($"Patient with email {patientCreateDto.Email} already exists");
            }

            var patient = _mapper.Map<Patient>(patientCreateDto);
            patient.CreatedDate = DateTime.UtcNow;
            patient.CreatedBy = createdBy;
            patient.IsActive = true;

            var createdPatient = await _patientRepository.AddAsync(patient, cancellationToken);
            var patientDto = _mapper.Map<PatientDto>(createdPatient);

            _logger.LogInformation("Successfully created patient with ID: {PatientId}", createdPatient.PatientID);
            return patientDto;
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
            _logger.LogError(ex, "Error occurred while creating patient");
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<PatientDto?> UpdatePatientAsync(PatientUpdateDto patientUpdateDto, string modifiedBy, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Updating patient with ID: {PatientId}", patientUpdateDto.PatientID);

            if (patientUpdateDto == null)
            {
                throw new ArgumentNullException(nameof(patientUpdateDto));
            }

            if (string.IsNullOrWhiteSpace(modifiedBy))
            {
                throw new ArgumentException("ModifiedBy cannot be null or empty", nameof(modifiedBy));
            }

            var existingPatient = await _patientRepository.GetByIdAsync(patientUpdateDto.PatientID, cancellationToken);

            if (existingPatient == null)
            {
                _logger.LogWarning("Patient not found with ID: {PatientId}", patientUpdateDto.PatientID);
                return null;
            }

            _mapper.Map(patientUpdateDto, existingPatient);
            existingPatient.ModifiedDate = DateTime.UtcNow;
            existingPatient.ModifiedBy = modifiedBy;

            await _patientRepository.UpdateAsync(existingPatient, cancellationToken);
            var patientDto = _mapper.Map<PatientDto>(existingPatient);

            _logger.LogInformation("Successfully updated patient with ID: {PatientId}", patientUpdateDto.PatientID);
            return patientDto;
        }
        catch (ArgumentException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while updating patient with ID: {PatientId}", patientUpdateDto.PatientID);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<bool> DeletePatientAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Deleting patient with ID: {PatientId}", id);

            if (id <= 0)
            {
                _logger.LogWarning("Invalid patient ID provided: {PatientId}", id);
                throw new ArgumentException("Patient ID must be greater than zero", nameof(id));
            }

            var exists = await _patientRepository.ExistsAsync(id, cancellationToken);

            if (!exists)
            {
                _logger.LogWarning("Patient not found with ID: {PatientId}", id);
                return false;
            }

            await _patientRepository.DeleteAsync(id, cancellationToken);
            _logger.LogInformation("Successfully deleted patient with ID: {PatientId}", id);
            return true;
        }
        catch (ArgumentException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while deleting patient with ID: {PatientId}", id);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<PatientDto>> GetActivePatientsAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving active patients");
            var patients = await _patientRepository.GetAllAsync(cancellationToken);
            var activePatients = patients.Where(p => p.IsActive);
            var patientDtos = _mapper.Map<IEnumerable<PatientDto>>(activePatients);
            _logger.LogInformation("Successfully retrieved {Count} active patients", patientDtos.Count());
            return patientDtos;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving active patients");
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<PatientDto>> SearchPatientsByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Searching patients by name: {Name}", name);

            if (string.IsNullOrWhiteSpace(name))
            {
                _logger.LogWarning("Invalid name provided for search");
                throw new ArgumentException("Name cannot be null or empty", nameof(name));
            }

            var patients = await _patientRepository.SearchAsync(name, cancellationToken);
            var patientDtos = _mapper.Map<IEnumerable<PatientDto>>(patients);
            _logger.LogInformation("Successfully found {Count} patients matching name: {Name}", patientDtos.Count(), name);
            return patientDtos;
        }
        catch (ArgumentException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while searching patients by name: {Name}", name);
            throw;
        }
    }
}
