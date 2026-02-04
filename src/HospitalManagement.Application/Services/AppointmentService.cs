using AutoMapper;
using HospitalManagement.Application.DTOs;
using HospitalManagement.Application.Interfaces;
using HospitalManagement.Domain.Entities;
using HospitalManagement.Domain.Interfaces.Repositories;
using HospitalManagement.Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace HospitalManagement.Application.Services;

/// <summary>
/// Service implementation for Appointment operations
/// </summary>
public class AppointmentService : IAppointmentService
{
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly IPatientRepository _patientRepository;
    private readonly IDoctorRepository _doctorRepository;
    private readonly IFreeSlotRepository _freeSlotRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<AppointmentService> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="AppointmentService"/> class
    /// </summary>
    /// <param name="appointmentRepository">Appointment repository</param>
    /// <param name="patientRepository">Patient repository</param>
    /// <param name="doctorRepository">Doctor repository</param>
    /// <param name="freeSlotRepository">FreeSlot repository</param>
    /// <param name="mapper">AutoMapper instance</param>
    /// <param name="logger">Logger instance</param>
    public AppointmentService(
        IAppointmentRepository appointmentRepository,
        IPatientRepository patientRepository,
        IDoctorRepository doctorRepository,
        IFreeSlotRepository freeSlotRepository,
        IMapper mapper,
        ILogger<AppointmentService> logger)
    {
        _appointmentRepository = appointmentRepository ?? throw new ArgumentNullException(nameof(appointmentRepository));
        _patientRepository = patientRepository ?? throw new ArgumentNullException(nameof(patientRepository));
        _doctorRepository = doctorRepository ?? throw new ArgumentNullException(nameof(doctorRepository));
        _freeSlotRepository = freeSlotRepository ?? throw new ArgumentNullException(nameof(freeSlotRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<AppointmentDto>> GetAllAppointmentsAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving all appointments");
            var appointments = await _appointmentRepository.GetAllAsync(cancellationToken);
            var appointmentDtos = _mapper.Map<IEnumerable<AppointmentDto>>(appointments);
            _logger.LogInformation("Successfully retrieved {Count} appointments", appointmentDtos.Count());
            return appointmentDtos;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving all appointments");
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<AppointmentDto?> GetAppointmentByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving appointment with ID: {AppointmentId}", id);

            if (id <= 0)
            {
                _logger.LogWarning("Invalid appointment ID provided: {AppointmentId}", id);
                throw new ArgumentException("Appointment ID must be greater than zero", nameof(id));
            }

            var appointment = await _appointmentRepository.GetByIdAsync(id, cancellationToken);

            if (appointment == null)
            {
                _logger.LogWarning("Appointment not found with ID: {AppointmentId}", id);
                return null;
            }

            var appointmentDto = _mapper.Map<AppointmentDto>(appointment);
            _logger.LogInformation("Successfully retrieved appointment with ID: {AppointmentId}", id);
            return appointmentDto;
        }
        catch (ArgumentException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving appointment with ID: {AppointmentId}", id);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<AppointmentDto>> GetAppointmentsByPatientIdAsync(int patientId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving appointments for patient ID: {PatientId}", patientId);

            if (patientId <= 0)
            {
                _logger.LogWarning("Invalid patient ID provided: {PatientId}", patientId);
                throw new ArgumentException("Patient ID must be greater than zero", nameof(patientId));
            }

            var appointments = await _appointmentRepository.GetByPatientIdAsync(patientId, cancellationToken);
            var appointmentDtos = _mapper.Map<IEnumerable<AppointmentDto>>(appointments);
            _logger.LogInformation("Successfully retrieved {Count} appointments for patient ID: {PatientId}", appointmentDtos.Count(), patientId);
            return appointmentDtos;
        }
        catch (ArgumentException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving appointments for patient ID: {PatientId}", patientId);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<AppointmentDto>> GetAppointmentsByDoctorIdAsync(int doctorId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving appointments for doctor ID: {DoctorId}", doctorId);

            if (doctorId <= 0)
            {
                _logger.LogWarning("Invalid doctor ID provided: {DoctorId}", doctorId);
                throw new ArgumentException("Doctor ID must be greater than zero", nameof(doctorId));
            }

            var appointments = await _appointmentRepository.GetByDoctorIdAsync(doctorId, cancellationToken);
            var appointmentDtos = _mapper.Map<IEnumerable<AppointmentDto>>(appointments);
            _logger.LogInformation("Successfully retrieved {Count} appointments for doctor ID: {DoctorId}", appointmentDtos.Count(), doctorId);
            return appointmentDtos;
        }
        catch (ArgumentException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving appointments for doctor ID: {DoctorId}", doctorId);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<AppointmentDto>> GetAppointmentsByDateAsync(DateTime date, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving appointments for date: {Date}", date.ToString("yyyy-MM-dd"));

            var allAppointments = await _appointmentRepository.GetAllAsync(cancellationToken);
            var appointments = allAppointments.Where(a => a.AppointmentDate.Date == date.Date);
            var appointmentDtos = _mapper.Map<IEnumerable<AppointmentDto>>(appointments);
            _logger.LogInformation("Successfully retrieved {Count} appointments for date: {Date}", appointmentDtos.Count(), date.ToString("yyyy-MM-dd"));
            return appointmentDtos;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving appointments for date: {Date}", date);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<AppointmentDto>> GetAppointmentsByStatusAsync(string status, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving appointments with status: {Status}", status);

            if (string.IsNullOrWhiteSpace(status))
            {
                _logger.LogWarning("Invalid status provided");
                throw new ArgumentException("Status cannot be null or empty", nameof(status));
            }

            var allAppointments = await _appointmentRepository.GetAllAsync(cancellationToken);
            var appointments = allAppointments.Where(a => a.Status.Equals(status, StringComparison.OrdinalIgnoreCase));
            var appointmentDtos = _mapper.Map<IEnumerable<AppointmentDto>>(appointments);
            _logger.LogInformation("Successfully retrieved {Count} appointments with status: {Status}", appointmentDtos.Count(), status);
            return appointmentDtos;
        }
        catch (ArgumentException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving appointments with status: {Status}", status);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<AppointmentDto> CreateAppointmentAsync(AppointmentCreateDto appointmentCreateDto, string createdBy, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Creating new appointment for patient ID: {PatientId} with doctor ID: {DoctorId}",
                appointmentCreateDto.PatientID, appointmentCreateDto.DoctorID);

            if (appointmentCreateDto == null)
            {
                throw new ArgumentNullException(nameof(appointmentCreateDto));
            }

            if (string.IsNullOrWhiteSpace(createdBy))
            {
                throw new ArgumentException("CreatedBy cannot be null or empty", nameof(createdBy));
            }

            // Validate patient exists
            var patientExists = await _patientRepository.ExistsAsync(appointmentCreateDto.PatientID, cancellationToken);
            if (!patientExists)
            {
                _logger.LogWarning("Patient with ID {PatientId} does not exist", appointmentCreateDto.PatientID);
                throw new InvalidOperationException($"Patient with ID {appointmentCreateDto.PatientID} does not exist");
            }

            // Validate doctor exists
            var doctorExists = await _doctorRepository.ExistsAsync(appointmentCreateDto.DoctorID, cancellationToken);
            if (!doctorExists)
            {
                _logger.LogWarning("Doctor with ID {DoctorId} does not exist", appointmentCreateDto.DoctorID);
                throw new InvalidOperationException($"Doctor with ID {appointmentCreateDto.DoctorID} does not exist");
            }

            // Validate free slot exists
            var freeSlotExists = await _freeSlotRepository.ExistsAsync(appointmentCreateDto.FreeSlotID, cancellationToken);
            if (!freeSlotExists)
            {
                _logger.LogWarning("Free slot with ID {FreeSlotId} does not exist", appointmentCreateDto.FreeSlotID);
                throw new InvalidOperationException($"Free slot with ID {appointmentCreateDto.FreeSlotID} does not exist");
            }

            var appointment = _mapper.Map<Appointment>(appointmentCreateDto);
            appointment.CreatedDate = DateTime.UtcNow;
            appointment.CreatedBy = createdBy;
            appointment.IsActive = true;
            appointment.IsPaid = false;
            appointment.FeedbackGiven = false;

            var createdAppointment = await _appointmentRepository.AddAsync(appointment, cancellationToken);
            var appointmentDto = _mapper.Map<AppointmentDto>(createdAppointment);

            _logger.LogInformation("Successfully created appointment with ID: {AppointmentId}", createdAppointment.AppointmentID);
            return appointmentDto;
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
            _logger.LogError(ex, "Error occurred while creating appointment");
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<AppointmentDto?> UpdateAppointmentAsync(AppointmentUpdateDto appointmentUpdateDto, string modifiedBy, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Updating appointment with ID: {AppointmentId}", appointmentUpdateDto.AppointmentID);

            if (appointmentUpdateDto == null)
            {
                throw new ArgumentNullException(nameof(appointmentUpdateDto));
            }

            if (string.IsNullOrWhiteSpace(modifiedBy))
            {
                throw new ArgumentException("ModifiedBy cannot be null or empty", nameof(modifiedBy));
            }

            var existingAppointment = await _appointmentRepository.GetByIdAsync(appointmentUpdateDto.AppointmentID, cancellationToken);

            if (existingAppointment == null)
            {
                _logger.LogWarning("Appointment not found with ID: {AppointmentId}", appointmentUpdateDto.AppointmentID);
                return null;
            }

            _mapper.Map(appointmentUpdateDto, existingAppointment);
            existingAppointment.ModifiedDate = DateTime.UtcNow;
            existingAppointment.ModifiedBy = modifiedBy;

            await _appointmentRepository.UpdateAsync(existingAppointment, cancellationToken);
            var appointmentDto = _mapper.Map<AppointmentDto>(existingAppointment);

            _logger.LogInformation("Successfully updated appointment with ID: {AppointmentId}", appointmentUpdateDto.AppointmentID);
            return appointmentDto;
        }
        catch (ArgumentException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while updating appointment with ID: {AppointmentId}", appointmentUpdateDto.AppointmentID);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<bool> DeleteAppointmentAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Deleting appointment with ID: {AppointmentId}", id);

            if (id <= 0)
            {
                _logger.LogWarning("Invalid appointment ID provided: {AppointmentId}", id);
                throw new ArgumentException("Appointment ID must be greater than zero", nameof(id));
            }

            var exists = await _appointmentRepository.ExistsAsync(id, cancellationToken);

            if (!exists)
            {
                _logger.LogWarning("Appointment not found with ID: {AppointmentId}", id);
                return false;
            }

            await _appointmentRepository.DeleteAsync(id, cancellationToken);
            _logger.LogInformation("Successfully deleted appointment with ID: {AppointmentId}", id);
            return true;
        }
        catch (ArgumentException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while deleting appointment with ID: {AppointmentId}", id);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<bool> CancelAppointmentAsync(int id, string modifiedBy, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Cancelling appointment with ID: {AppointmentId}", id);

            if (id <= 0)
            {
                _logger.LogWarning("Invalid appointment ID provided: {AppointmentId}", id);
                throw new ArgumentException("Appointment ID must be greater than zero", nameof(id));
            }

            if (string.IsNullOrWhiteSpace(modifiedBy))
            {
                throw new ArgumentException("ModifiedBy cannot be null or empty", nameof(modifiedBy));
            }

            var appointment = await _appointmentRepository.GetByIdAsync(id, cancellationToken);

            if (appointment == null)
            {
                _logger.LogWarning("Appointment not found with ID: {AppointmentId}", id);
                return false;
            }

            appointment.Status = "Cancelled";
            appointment.IsActive = false;
            appointment.ModifiedDate = DateTime.UtcNow;
            appointment.ModifiedBy = modifiedBy;

            await _appointmentRepository.UpdateAsync(appointment, cancellationToken);
            _logger.LogInformation("Successfully cancelled appointment with ID: {AppointmentId}", id);
            return true;
        }
        catch (ArgumentException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while cancelling appointment with ID: {AppointmentId}", id);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<bool> MarkAppointmentAsPaidAsync(int id, string modifiedBy, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Marking appointment as paid with ID: {AppointmentId}", id);

            if (id <= 0)
            {
                _logger.LogWarning("Invalid appointment ID provided: {AppointmentId}", id);
                throw new ArgumentException("Appointment ID must be greater than zero", nameof(id));
            }

            if (string.IsNullOrWhiteSpace(modifiedBy))
            {
                throw new ArgumentException("ModifiedBy cannot be null or empty", nameof(modifiedBy));
            }

            var appointment = await _appointmentRepository.GetByIdAsync(id, cancellationToken);

            if (appointment == null)
            {
                _logger.LogWarning("Appointment not found with ID: {AppointmentId}", id);
                return false;
            }

            appointment.IsPaid = true;
            appointment.ModifiedDate = DateTime.UtcNow;
            appointment.ModifiedBy = modifiedBy;

            await _appointmentRepository.UpdateAsync(appointment, cancellationToken);
            _logger.LogInformation("Successfully marked appointment as paid with ID: {AppointmentId}", id);
            return true;
        }
        catch (ArgumentException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while marking appointment as paid with ID: {AppointmentId}", id);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<AppointmentDto>> GetUpcomingAppointmentsByPatientAsync(int patientId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving upcoming appointments for patient ID: {PatientId}", patientId);

            if (patientId <= 0)
            {
                _logger.LogWarning("Invalid patient ID provided: {PatientId}", patientId);
                throw new ArgumentException("Patient ID must be greater than zero", nameof(patientId));
            }

            var appointments = await _appointmentRepository.GetByPatientIdAsync(patientId, cancellationToken);
            var upcomingAppointments = appointments.Where(a => a.AppointmentDate >= DateTime.UtcNow && a.IsActive)
                                                  .OrderBy(a => a.AppointmentDate);
            var appointmentDtos = _mapper.Map<IEnumerable<AppointmentDto>>(upcomingAppointments);
            _logger.LogInformation("Successfully retrieved {Count} upcoming appointments for patient ID: {PatientId}", appointmentDtos.Count(), patientId);
            return appointmentDtos;
        }
        catch (ArgumentException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving upcoming appointments for patient ID: {PatientId}", patientId);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<AppointmentDto>> GetUpcomingAppointmentsByDoctorAsync(int doctorId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving upcoming appointments for doctor ID: {DoctorId}", doctorId);

            if (doctorId <= 0)
            {
                _logger.LogWarning("Invalid doctor ID provided: {DoctorId}", doctorId);
                throw new ArgumentException("Doctor ID must be greater than zero", nameof(doctorId));
            }

            var appointments = await _appointmentRepository.GetByDoctorIdAsync(doctorId, cancellationToken);
            var upcomingAppointments = appointments.Where(a => a.AppointmentDate >= DateTime.UtcNow && a.IsActive)
                                                  .OrderBy(a => a.AppointmentDate);
            var appointmentDtos = _mapper.Map<IEnumerable<AppointmentDto>>(upcomingAppointments);
            _logger.LogInformation("Successfully retrieved {Count} upcoming appointments for doctor ID: {DoctorId}", appointmentDtos.Count(), doctorId);
            return appointmentDtos;
        }
        catch (ArgumentException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving upcoming appointments for doctor ID: {DoctorId}", doctorId);
            throw;
        }
    }
}
