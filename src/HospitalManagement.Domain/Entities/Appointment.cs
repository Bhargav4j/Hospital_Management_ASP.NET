namespace HospitalManagement.Domain.Entities;

/// <summary>
/// Represents an appointment between a patient and a doctor
/// </summary>
public class Appointment
{
    public int Id { get; set; }
    public int PatientId { get; set; }
    public int DoctorId { get; set; }
    public DateTime AppointmentDate { get; set; }
    public string AppointmentTime { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty; // Pending, Approved, Completed, Cancelled
    public string? Symptoms { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public bool IsActive { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public string? ModifiedBy { get; set; }

    // Navigation properties
    public virtual Patient? Patient { get; set; }
    public virtual Doctor? Doctor { get; set; }
    public virtual ICollection<Treatment> Treatments { get; set; } = new List<Treatment>();
}
