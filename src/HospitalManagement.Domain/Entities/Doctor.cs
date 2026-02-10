namespace HospitalManagement.Domain.Entities;

/// <summary>
/// Represents a doctor in the hospital management system
/// </summary>
public class Doctor
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Specialization { get; set; } = string.Empty;
    public string? Qualification { get; set; }
    public string? PhoneNumber { get; set; }
    public decimal? ConsultationFee { get; set; }
    public string? Address { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public bool IsActive { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public string? ModifiedBy { get; set; }

    // Navigation properties
    public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    public ICollection<Bill> Bills { get; set; } = new List<Bill>();
}
