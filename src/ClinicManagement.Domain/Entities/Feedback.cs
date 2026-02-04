namespace ClinicManagement.Domain.Entities;

/// <summary>
/// Represents patient feedback
/// </summary>
public class Feedback
{
    public int Id { get; set; }
    public int PatientId { get; set; }
    public int? DoctorId { get; set; }
    public string Message { get; set; } = string.Empty;
    public int? Rating { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public bool IsActive { get; set; }
    public string CreatedBy { get; set; } = "System";
    public string? ModifiedBy { get; set; }

    public virtual Patient Patient { get; set; } = null!;
}
