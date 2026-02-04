using HospitalManagement.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HospitalManagement.Web.Pages.Admin;

public class IndexModel : PageModel
{
    private readonly IPatientService _patientService;
    private readonly IDoctorService _doctorService;
    private readonly IAppointmentService _appointmentService;
    private readonly IDepartmentService _departmentService;

    public IndexModel(
        IPatientService patientService,
        IDoctorService doctorService,
        IAppointmentService appointmentService,
        IDepartmentService departmentService)
    {
        _patientService = patientService;
        _doctorService = doctorService;
        _appointmentService = appointmentService;
        _departmentService = departmentService;
    }

    public int TotalPatients { get; set; }
    public int TotalDoctors { get; set; }
    public int TotalAppointments { get; set; }
    public int TotalDepartments { get; set; }

    public async Task<IActionResult> OnGetAsync(CancellationToken cancellationToken = default)
    {
        var userRole = HttpContext.Session.GetString("UserRole");
        if (userRole != "Admin")
        {
            TempData["ErrorMessage"] = "Unauthorized access.";
            return RedirectToPage("/Index");
        }

        var patients = await _patientService.GetAllPatientsAsync(cancellationToken);
        var doctors = await _doctorService.GetAllDoctorsAsync(cancellationToken);
        var appointments = await _appointmentService.GetAllAppointmentsAsync(cancellationToken);
        var departments = await _departmentService.GetAllDepartmentsAsync(cancellationToken);

        TotalPatients = patients.Count();
        TotalDoctors = doctors.Count();
        TotalAppointments = appointments.Count();
        TotalDepartments = departments.Count();

        return Page();
    }
}
