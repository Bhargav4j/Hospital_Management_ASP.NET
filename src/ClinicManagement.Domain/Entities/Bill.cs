namespace ClinicManagement.Domain.Entities;

/// <summary>
/// Bill entity representing billing information
/// </summary>
public class Bill
{
    public int Id { get; set; }
    public int PatientId { get; set; }
    public int AppointmentId { get; set; }
    public decimal ConsultationFee { get; set; }
    public decimal? TestCharges { get; set; }
    public decimal? MedicineCharges { get; set; }
    public decimal? OtherCharges { get; set; }
    public decimal TotalAmount { get; set; }
    public string PaymentStatus { get; set; } = "Unpaid"; // Unpaid, Paid, PartiallyPaid
    public DateTime? PaymentDate { get; set; }
    public string? PaymentMethod { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public bool IsActive { get; set; }
    public string CreatedBy { get; set; } = "System";
    public string? ModifiedBy { get; set; }

    // Navigation properties
    public virtual Patient Patient { get; set; } = null!;
}
