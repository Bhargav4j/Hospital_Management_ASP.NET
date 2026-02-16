namespace ClinicManagement.Domain.Entities;

public class Appointment
{
    public int AppointmentID { get; set; }
    public int PatientID { get; set; }
    public int DoctorID { get; set; }
    public DateTime AppointmentDate { get; set; }
    public string Timings { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string Disease { get; set; } = string.Empty;
    public string Progress { get; set; } = string.Empty;
    public string Prescription { get; set; } = string.Empty;
    public decimal BillAmount { get; set; }
    public bool IsPaid { get; set; }
    public bool FeedbackGiven { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public DateTime? ModifiedDate { get; set; }
    
    // Navigation properties
    public virtual Patient Patient { get; set; } = null!;
    public virtual Doctor Doctor { get; set; } = null!;
}
