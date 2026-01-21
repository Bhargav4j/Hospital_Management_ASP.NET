namespace HospitalManagement.Domain.Entities;

/// <summary>
/// Represents feedback from a patient about a doctor
/// </summary>
public class Feedback
{
    public int FeedbackID { get; set; }
    public int PatientID { get; set; }
    public int DoctorID { get; set; }
    public int AppointmentID { get; set; }
    public int Rating { get; set; }
    public string? Comments { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public bool IsActive { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public string? ModifiedBy { get; set; }

    // Navigation properties
    public virtual Patient? Patient { get; set; }
    public virtual Doctor? Doctor { get; set; }
    public virtual Appointment? Appointment { get; set; }
}
