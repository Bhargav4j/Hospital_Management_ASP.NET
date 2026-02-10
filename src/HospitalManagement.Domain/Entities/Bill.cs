namespace HospitalManagement.Domain.Entities;

/// <summary>
/// Represents a bill for medical services
/// </summary>
public class Bill
{
    public int Id { get; set; }
    public int PatientId { get; set; }
    public int DoctorId { get; set; }
    public int? AppointmentId { get; set; }
    public decimal Amount { get; set; }
    public DateTime BillDate { get; set; }
    public string? Description { get; set; }
    public bool IsPaid { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public bool IsActive { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public string? ModifiedBy { get; set; }

    // Navigation properties
    public Patient Patient { get; set; } = null!;
    public Doctor Doctor { get; set; } = null!;
}
