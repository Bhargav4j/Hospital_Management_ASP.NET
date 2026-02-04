using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace HospitalManagement.Web.Pages;

/// <summary>
/// Page model for the home/index page
/// </summary>
public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="IndexModel"/> class
    /// </summary>
    /// <param name="logger">Logger instance</param>
    public IndexModel(ILogger<IndexModel> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Handles GET requests to the index page
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    public async Task OnGetAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Index page loaded");
        await Task.CompletedTask;
    }
}
