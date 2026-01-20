using HospitalManagement.Domain.Enums;

namespace HospitalManagement.Domain.Entities;

/// <summary>
/// Represents a doctor in the hospital management system
/// </summary>
public class Doctor
{
    /// <summary>
    /// Unique identifier for the doctor
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Doctor's full name
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Doctor's email address
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Doctor's password (should be hashed)
    /// </summary>
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// Doctor's phone number
    /// </summary>
    public string PhoneNumber { get; set; } = string.Empty;

    /// <summary>
    /// Doctor's date of birth
    /// </summary>
    public DateTime BirthDate { get; set; }

    /// <summary>
    /// Doctor's gender
    /// </summary>
    public Gender Gender { get; set; }

    /// <summary>
    /// Doctor's residential address
    /// </summary>
    public string Address { get; set; } = string.Empty;

    /// <summary>
    /// Doctor's specialization
    /// </summary>
    public string Specialization { get; set; } = string.Empty;

    /// <summary>
    /// Doctor's qualification
    /// </summary>
    public string Qualification { get; set; } = string.Empty;

    /// <summary>
    /// Doctor's experience in years
    /// </summary>
    public int ExperienceYears { get; set; }

    /// <summary>
    /// Doctor's consultation fee
    /// </summary>
    public decimal ConsultationFee { get; set; }

    /// <summary>
    /// Indicates if the doctor is active
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Date when the doctor was created
    /// </summary>
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Date when the doctor was last modified
    /// </summary>
    public DateTime? ModifiedDate { get; set; }

    /// <summary>
    /// User who created the doctor record
    /// </summary>
    public string CreatedBy { get; set; } = "System";

    /// <summary>
    /// User who last modified the doctor record
    /// </summary>
    public string? ModifiedBy { get; set; }

    /// <summary>
    /// Navigation property for appointments
    /// </summary>
    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    /// <summary>
    /// Navigation property for treatment history
    /// </summary>
    public virtual ICollection<TreatmentHistory> TreatmentHistories { get; set; } = new List<TreatmentHistory>();

    /// <summary>
    /// Navigation property for bills
    /// </summary>
    public virtual ICollection<Bill> Bills { get; set; } = new List<Bill>();

    /// <summary>
    /// Navigation property for feedback received
    /// </summary>
    public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();
}
