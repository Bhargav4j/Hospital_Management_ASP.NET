using ClinicManagement.Domain.Entities;
using ClinicManagement.Domain.Interfaces.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClinicManagement.Web.Pages.Patient;

public class BillsHistoryModel : PageModel
{
    private readonly IBillRepository _billRepository;
    private readonly ILogger<BillsHistoryModel> _logger;

    public List<Bill> Bills { get; set; } = new();

    public BillsHistoryModel(IBillRepository billRepository, ILogger<BillsHistoryModel> logger)
    {
        _billRepository = billRepository;
        _logger = logger;
    }

    public async Task<IActionResult> OnGetAsync()
    {
        var userId = HttpContext.Session.GetInt32("UserId");
        if (userId == null)
        {
            return RedirectToPage("/Login");
        }

        try
        {
            var bills = await _billRepository.GetByPatientIdAsync(userId.Value);
            Bills = bills.ToList();
            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading bills for patient {PatientId}", userId);
            return Page();
        }
    }
}
