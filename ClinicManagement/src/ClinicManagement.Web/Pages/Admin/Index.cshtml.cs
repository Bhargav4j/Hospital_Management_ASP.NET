using ClinicManagement.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClinicManagement.Web.Pages.Admin;

public class AdminIndexModel : PageModel
{
    private readonly IPatientService _patientService;
    private readonly IDoctorService _doctorService;
    private readonly IDepartmentService _departmentService;
    private readonly IAppointmentService _appointmentService;

    public AdminIndexModel(
        IPatientService patientService,
        IDoctorService doctorService,
        IDepartmentService departmentService,
        IAppointmentService appointmentService)
    {
        _patientService = patientService;
        _doctorService = doctorService;
        _departmentService = departmentService;
        _appointmentService = appointmentService;
    }

    public int PatientCount { get; set; }
    public int DoctorCount { get; set; }
    public int DepartmentCount { get; set; }
    public int AppointmentCount { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        var userId = HttpContext.Session.GetString("UserId");
        var userRole = HttpContext.Session.GetString("UserRole");
        
        if (string.IsNullOrEmpty(userId) || userRole != "Admin")
        {
            return RedirectToPage("/Login");
        }

        var patients = await _patientService.GetAllPatientsAsync();
        PatientCount = patients.Count();
        
        var doctors = await _doctorService.GetAllDoctorsAsync();
        DoctorCount = doctors.Count();
        
        var departments = await _departmentService.GetAllDepartmentsAsync();
        DepartmentCount = departments.Count();
        
        var appointments = await _appointmentService.GetAllAppointmentsAsync();
        AppointmentCount = appointments.Count();

        return Page();
    }
}
