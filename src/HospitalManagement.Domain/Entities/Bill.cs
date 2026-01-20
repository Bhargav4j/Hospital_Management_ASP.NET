namespace HospitalManagement.Domain.Entities;

/// <summary>
/// Represents a bill in the hospital management system
/// </summary>
public class Bill
{
    /// <summary>
    /// Unique identifier for the bill
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Patient ID associated with the bill
    /// </summary>
    public int PatientId { get; set; }

    /// <summary>
    /// Doctor ID who generated the bill
    /// </summary>
    public int DoctorId { get; set; }

    /// <summary>
    /// Bill date
    /// </summary>
    public DateTime BillDate { get; set; }

    /// <summary>
    /// Consultation fee
    /// </summary>
    public decimal ConsultationFee { get; set; }

    /// <summary>
    /// Medicine cost
    /// </summary>
    public decimal MedicineCost { get; set; }

    /// <summary>
    /// Test charges
    /// </summary>
    public decimal TestCharges { get; set; }

    /// <summary>
    /// Other charges
    /// </summary>
    public decimal OtherCharges { get; set; }

    /// <summary>
    /// Total amount
    /// </summary>
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// Indicates if the bill is paid
    /// </summary>
    public bool IsPaid { get; set; }

    /// <summary>
    /// Payment date
    /// </summary>
    public DateTime? PaymentDate { get; set; }

    /// <summary>
    /// Bill description
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Indicates if the record is active
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Date when the record was created
    /// </summary>
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Date when the record was last modified
    /// </summary>
    public DateTime? ModifiedDate { get; set; }

    /// <summary>
    /// User who created the record
    /// </summary>
    public string CreatedBy { get; set; } = "System";

    /// <summary>
    /// User who last modified the record
    /// </summary>
    public string? ModifiedBy { get; set; }

    /// <summary>
    /// Navigation property to Patient
    /// </summary>
    public virtual Patient? Patient { get; set; }

    /// <summary>
    /// Navigation property to Doctor
    /// </summary>
    public virtual Doctor? Doctor { get; set; }
}
