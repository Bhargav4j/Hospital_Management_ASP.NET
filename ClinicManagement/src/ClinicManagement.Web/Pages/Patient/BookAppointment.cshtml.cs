using AutoMapper;
using ClinicManagement.Application.DTOs;
using ClinicManagement.Domain.Entities;
using ClinicManagement.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace ClinicManagement.Web.Pages.Patient;

public class BookAppointmentModel : PageModel
{
    private readonly IAppointmentService _appointmentService;
    private readonly IDoctorService _doctorService;
    private readonly IMapper _mapper;

    public BookAppointmentModel(
        IAppointmentService appointmentService,
        IDoctorService doctorService,
        IMapper mapper)
    {
        _appointmentService = appointmentService;
        _doctorService = doctorService;
        _mapper = mapper;
    }

    [BindProperty(SupportsGet = true)]
    public int DoctorId { get; set; }

    [BindProperty]
    [Required(ErrorMessage = "Appointment date is required")]
    public DateTime AppointmentDate { get; set; } = DateTime.Today.AddDays(1);

    [BindProperty]
    [Required(ErrorMessage = "Time slot is required")]
    public string Timings { get; set; } = string.Empty;

    [BindProperty]
    [Required(ErrorMessage = "Please describe your symptoms")]
    public string Disease { get; set; } = string.Empty;

    public DoctorDto? Doctor { get; set; }
    public string? SuccessMessage { get; set; }
    public string? ErrorMessage { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        var userId = HttpContext.Session.GetString("UserId");
        if (string.IsNullOrEmpty(userId))
        {
            return RedirectToPage("/Login");
        }

        var doctor = await _doctorService.GetDoctorByIdAsync(DoctorId);
        if (doctor == null)
        {
            return RedirectToPage("/Patient/ViewDoctors");
        }

        Doctor = _mapper.Map<DoctorDto>(doctor);
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var userId = HttpContext.Session.GetString("UserId");
        if (string.IsNullOrEmpty(userId))
        {
            return RedirectToPage("/Login");
        }

        var doctor = await _doctorService.GetDoctorByIdAsync(DoctorId);
        if (doctor == null)
        {
            return RedirectToPage("/Patient/ViewDoctors");
        }
        Doctor = _mapper.Map<DoctorDto>(doctor);

        if (!ModelState.IsValid)
        {
            return Page();
        }

        try
        {
            var appointment = new Appointment
            {
                PatientID = int.Parse(userId),
                DoctorID = DoctorId,
                AppointmentDate = AppointmentDate,
                Timings = Timings,
                Disease = Disease,
                Status = "Pending"
            };

            await _appointmentService.CreateAppointmentAsync(appointment);
            SuccessMessage = "Appointment booked successfully!";
            return RedirectToPage("/Patient/Appointments");
        }
        catch (Exception ex)
        {
            ErrorMessage = "An error occurred while booking the appointment. Please try again.";
            return Page();
        }
    }
}
