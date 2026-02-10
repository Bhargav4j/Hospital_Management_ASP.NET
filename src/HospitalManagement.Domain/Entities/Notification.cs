namespace HospitalManagement.Domain.Entities;

/// <summary>
/// Represents a notification for patients
/// </summary>
public class Notification
{
    public int Id { get; set; }
    public int PatientId { get; set; }
    public string Message { get; set; } = string.Empty;
    public bool IsRead { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public bool IsActive { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public string? ModifiedBy { get; set; }

    // Navigation properties
    public Patient Patient { get; set; } = null!;
}
