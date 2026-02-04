namespace ClinicManagement.Domain.Entities;

/// <summary>
/// Represents a billing record
/// </summary>
public class Bill
{
    public int Id { get; set; }
    public int PatientId { get; set; }
    public int AppointmentId { get; set; }
    public decimal Amount { get; set; }
    public DateTime BillDate { get; set; }
    public bool IsPaid { get; set; }
    public string? PaymentMethod { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public bool IsActive { get; set; }
    public string CreatedBy { get; set; } = "System";
    public string? ModifiedBy { get; set; }

    public virtual Patient Patient { get; set; } = null!;
}
