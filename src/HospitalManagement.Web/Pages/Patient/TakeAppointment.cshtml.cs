using HospitalManagement.Application.DTOs;
using HospitalManagement.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.Web.Pages.Patient;

/// <summary>
/// Page model for booking appointments
/// </summary>
public class TakeAppointmentModel : PageModel
{
    private readonly IPatientService _patientService;
    private readonly IDoctorService _doctorService;
    private readonly IAppointmentService _appointmentService;
    private readonly ILogger<TakeAppointmentModel> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="TakeAppointmentModel"/> class
    /// </summary>
    /// <param name="patientService">Patient service</param>
    /// <param name="doctorService">Doctor service</param>
    /// <param name="appointmentService">Appointment service</param>
    /// <param name="logger">Logger instance</param>
    public TakeAppointmentModel(
        IPatientService patientService,
        IDoctorService doctorService,
        IAppointmentService appointmentService,
        ILogger<TakeAppointmentModel> logger)
    {
        _patientService = patientService ?? throw new ArgumentNullException(nameof(patientService));
        _doctorService = doctorService ?? throw new ArgumentNullException(nameof(doctorService));
        _appointmentService = appointmentService ?? throw new ArgumentNullException(nameof(appointmentService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Gets or sets the doctor information
    /// </summary>
    public DoctorDto? Doctor { get; set; }

    /// <summary>
    /// Gets or sets available time slots
    /// </summary>
    public SelectList AvailableSlots { get; set; } = new SelectList(Enumerable.Empty<SelectListItem>());

    /// <summary>
    /// Gets or sets the input model
    /// </summary>
    [BindProperty]
    public InputModel Input { get; set; } = new InputModel();

    /// <summary>
    /// Input model for appointment booking
    /// </summary>
    public class InputModel
    {
        /// <summary>
        /// Gets or sets the doctor ID
        /// </summary>
        [Required]
        public int DoctorId { get; set; }

        /// <summary>
        /// Gets or sets the appointment date
        /// </summary>
        [Required(ErrorMessage = "Please select an appointment date")]
        [DataType(DataType.Date)]
        public DateTime AppointmentDate { get; set; }

        /// <summary>
        /// Gets or sets the free slot ID
        /// </summary>
        [Required(ErrorMessage = "Please select a time slot")]
        public int FreeSlotId { get; set; }

        /// <summary>
        /// Gets or sets the disease/reason for visit
        /// </summary>
        [Required(ErrorMessage = "Please describe your symptoms or reason for visit")]
        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string Disease { get; set; } = string.Empty;
    }

    /// <summary>
    /// Handles GET requests
    /// </summary>
    /// <param name="doctorId">Doctor ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Action result</returns>
    public async Task<IActionResult> OnGetAsync(int doctorId, CancellationToken cancellationToken = default)
    {
        try
        {
            // Check if user is logged in as patient
            var userRole = HttpContext.Session.GetString("UserRole");
            if (userRole != "Patient")
            {
                _logger.LogWarning("Unauthorized access attempt to book appointment");
                TempData["ErrorMessage"] = "Please login as a patient to book appointments.";
                return RedirectToPage("/Login");
            }

            if (doctorId <= 0)
            {
                _logger.LogWarning("Invalid doctor ID provided: {DoctorId}", doctorId);
                TempData["ErrorMessage"] = "Invalid doctor ID.";
                return RedirectToPage("/Patient/ViewDoctors");
            }

            _logger.LogInformation("Loading appointment booking for doctor ID: {DoctorId}", doctorId);

            // Get doctor information
            Doctor = await _doctorService.GetDoctorByIdAsync(doctorId, cancellationToken);

            if (Doctor == null)
            {
                _logger.LogWarning("Doctor not found: {DoctorId}", doctorId);
                TempData["ErrorMessage"] = "Doctor not found.";
                return RedirectToPage("/Patient/ViewDoctors");
            }

            if (!Doctor.Status || !Doctor.IsActive)
            {
                _logger.LogWarning("Doctor is not available: {DoctorId}", doctorId);
                TempData["ErrorMessage"] = "This doctor is currently not available for appointments.";
                return RedirectToPage("/Patient/ViewDoctors");
            }

            // Initialize input model
            Input.DoctorId = doctorId;
            Input.AppointmentDate = DateTime.Today.AddDays(1);

            // Load available slots (simplified - in real app would be dynamic)
            var slots = new List<SelectListItem>
            {
                new SelectListItem { Value = "1", Text = "09:00 AM - 10:00 AM" },
                new SelectListItem { Value = "2", Text = "10:00 AM - 11:00 AM" },
                new SelectListItem { Value = "3", Text = "11:00 AM - 12:00 PM" },
                new SelectListItem { Value = "4", Text = "02:00 PM - 03:00 PM" },
                new SelectListItem { Value = "5", Text = "03:00 PM - 04:00 PM" },
                new SelectListItem { Value = "6", Text = "04:00 PM - 05:00 PM" }
            };
            AvailableSlots = new SelectList(slots, "Value", "Text");

            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while loading appointment booking for doctor ID: {DoctorId}", doctorId);
            TempData["ErrorMessage"] = "An error occurred while loading appointment form.";
            return RedirectToPage("/Patient/ViewDoctors");
        }
    }

    /// <summary>
    /// Handles POST requests for booking appointment
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Action result</returns>
    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
        {
            // Reload doctor info and slots
            Doctor = await _doctorService.GetDoctorByIdAsync(Input.DoctorId, cancellationToken);
            var slots = new List<SelectListItem>
            {
                new SelectListItem { Value = "1", Text = "09:00 AM - 10:00 AM" },
                new SelectListItem { Value = "2", Text = "10:00 AM - 11:00 AM" },
                new SelectListItem { Value = "3", Text = "11:00 AM - 12:00 PM" },
                new SelectListItem { Value = "4", Text = "02:00 PM - 03:00 PM" },
                new SelectListItem { Value = "5", Text = "03:00 PM - 04:00 PM" },
                new SelectListItem { Value = "6", Text = "04:00 PM - 05:00 PM" }
            };
            AvailableSlots = new SelectList(slots, "Value", "Text");
            return Page();
        }

        try
        {
            var userIdStr = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdStr) || !int.TryParse(userIdStr, out int patientId))
            {
                _logger.LogError("Invalid patient ID in session");
                TempData["ErrorMessage"] = "Invalid session data.";
                return RedirectToPage("/Login");
            }

            _logger.LogInformation("Creating appointment for patient ID: {PatientId}, doctor ID: {DoctorId}",
                patientId, Input.DoctorId);

            // Create appointment DTO
            var appointmentCreateDto = new AppointmentCreateDto
            {
                PatientID = patientId,
                DoctorID = Input.DoctorId,
                FreeSlotID = Input.FreeSlotId,
                AppointmentDate = Input.AppointmentDate,
                Disease = Input.Disease,
                Status = "Pending"
            };

            // Create appointment
            var createdBy = HttpContext.Session.GetString("UserEmail") ?? "Patient";
            var createdAppointment = await _appointmentService.CreateAppointmentAsync(appointmentCreateDto, createdBy, cancellationToken);

            if (createdAppointment != null)
            {
                _logger.LogInformation("Appointment created successfully: {AppointmentId}", createdAppointment.AppointmentID);
                TempData["SuccessMessage"] = "Appointment booked successfully!";
                return RedirectToPage("/Patient/Appointments");
            }
            else
            {
                _logger.LogError("Failed to create appointment");
                ModelState.AddModelError(string.Empty, "Failed to book appointment. Please try again.");

                // Reload doctor info and slots
                Doctor = await _doctorService.GetDoctorByIdAsync(Input.DoctorId, cancellationToken);
                var slots = new List<SelectListItem>
                {
                    new SelectListItem { Value = "1", Text = "09:00 AM - 10:00 AM" },
                    new SelectListItem { Value = "2", Text = "10:00 AM - 11:00 AM" },
                    new SelectListItem { Value = "3", Text = "11:00 AM - 12:00 PM" },
                    new SelectListItem { Value = "4", Text = "02:00 PM - 03:00 PM" },
                    new SelectListItem { Value = "5", Text = "03:00 PM - 04:00 PM" },
                    new SelectListItem { Value = "6", Text = "04:00 PM - 05:00 PM" }
                };
                AvailableSlots = new SelectList(slots, "Value", "Text");
                return Page();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while booking appointment");
            ModelState.AddModelError(string.Empty, "An error occurred while booking appointment.");

            // Reload doctor info and slots
            Doctor = await _doctorService.GetDoctorByIdAsync(Input.DoctorId, cancellationToken);
            var slots = new List<SelectListItem>
            {
                new SelectListItem { Value = "1", Text = "09:00 AM - 10:00 AM" },
                new SelectListItem { Value = "2", Text = "10:00 AM - 11:00 AM" },
                new SelectListItem { Value = "3", Text = "11:00 AM - 12:00 PM" },
                new SelectListItem { Value = "4", Text = "02:00 PM - 03:00 PM" },
                new SelectListItem { Value = "5", Text = "03:00 PM - 04:00 PM" },
                new SelectListItem { Value = "6", Text = "04:00 PM - 05:00 PM" }
            };
            AvailableSlots = new SelectList(slots, "Value", "Text");
            return Page();
        }
    }
}
