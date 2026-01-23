namespace ClinicManagement.Domain.Entities;

/// <summary>
/// Notification entity representing system notifications
/// </summary>
public class Notification
{
    public int Id { get; set; }
    public int? PatientId { get; set; }
    public int? DoctorId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string Type { get; set; } = "Info"; // Info, Warning, Success, Error
    public bool IsRead { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public bool IsActive { get; set; }
    public string CreatedBy { get; set; } = "System";
    public string? ModifiedBy { get; set; }

    // Navigation properties
    public virtual Patient? Patient { get; set; }
}
