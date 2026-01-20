using HospitalManagement.Domain.Enums;

namespace HospitalManagement.Domain.Entities;

/// <summary>
/// Represents a patient in the hospital management system
/// </summary>
public class Patient
{
    /// <summary>
    /// Unique identifier for the patient
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Patient's full name
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Patient's email address
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Patient's password (should be hashed)
    /// </summary>
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// Patient's phone number
    /// </summary>
    public string PhoneNumber { get; set; } = string.Empty;

    /// <summary>
    /// Patient's date of birth
    /// </summary>
    public DateTime BirthDate { get; set; }

    /// <summary>
    /// Patient's gender
    /// </summary>
    public Gender Gender { get; set; }

    /// <summary>
    /// Patient's residential address
    /// </summary>
    public string Address { get; set; } = string.Empty;

    /// <summary>
    /// Patient's blood group
    /// </summary>
    public string? BloodGroup { get; set; }

    /// <summary>
    /// Indicates if the patient is active
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Date when the patient was created
    /// </summary>
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Date when the patient was last modified
    /// </summary>
    public DateTime? ModifiedDate { get; set; }

    /// <summary>
    /// User who created the patient record
    /// </summary>
    public string CreatedBy { get; set; } = "System";

    /// <summary>
    /// User who last modified the patient record
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
    /// Navigation property for feedback
    /// </summary>
    public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();
}
