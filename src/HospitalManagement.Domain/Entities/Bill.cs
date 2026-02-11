namespace HospitalManagement.Domain.Entities;

/// <summary>
/// Represents a billing record for a patient
/// </summary>
public class Bill
{
    public int Id { get; set; }
    public int PatientId { get; set; }
    public DateTime BillDate { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal PaidAmount { get; set; }
    public decimal Balance { get; set; }
    public string Status { get; set; } = string.Empty; // Paid, Pending, Partial
    public string? Description { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public bool IsActive { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public string? ModifiedBy { get; set; }

    // Navigation properties
    public virtual Patient? Patient { get; set; }
}
