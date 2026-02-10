namespace HospitalManagement.Domain.Entities;

/// <summary>
/// Represents the treatment history for an appointment
/// </summary>
public class TreatmentHistory
{
    public int Id { get; set; }
    public int AppointmentId { get; set; }
    public string? Prescription { get; set; }
    public string? Notes { get; set; }
    public DateTime TreatmentDate { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public bool IsActive { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public string? ModifiedBy { get; set; }

    // Navigation properties
    public Appointment Appointment { get; set; } = null!;
}
