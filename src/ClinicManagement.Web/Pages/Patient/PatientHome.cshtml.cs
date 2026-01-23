using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ClinicManagement.Domain.Interfaces.Repositories;

namespace ClinicManagement.Web.Pages.Patient;

public class PatientHomeModel : PageModel
{
    private readonly IPatientRepository _patientRepository;
    private readonly ILogger<PatientHomeModel> _logger;

    public PatientHomeModel(IPatientRepository patientRepository, ILogger<PatientHomeModel> logger)
    {
        _patientRepository = patientRepository ?? throw new ArgumentNullException(nameof(patientRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public string PatientName { get; set; } = "Patient";

    public async Task<IActionResult> OnGetAsync()
    {
        var userId = HttpContext.Session.GetInt32("UserId");
        if (!userId.HasValue)
        {
            return RedirectToPage("/Index");
        }

        try
        {
            var patient = await _patientRepository.GetByIdAsync(userId.Value);
            if (patient != null)
            {
                PatientName = patient.Name;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading patient home page");
        }

        return Page();
    }
}
