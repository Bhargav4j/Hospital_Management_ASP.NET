namespace ClinicManagement.Domain.Entities;

/// <summary>
/// Doctor entity representing a doctor in the clinic management system
/// </summary>
public class Doctor
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string Specialization { get; set; } = string.Empty;
    public string PhoneNo { get; set; } = string.Empty;
    public string? LicenseNumber { get; set; }
    public decimal ConsultationFee { get; set; }
    public string? Experience { get; set; }
    public string? Qualification { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public bool IsActive { get; set; }
    public string CreatedBy { get; set; } = "System";
    public string? ModifiedBy { get; set; }

    // Navigation properties
    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    public virtual ICollection<TreatmentHistory> TreatmentHistories { get; set; } = new List<TreatmentHistory>();
}
