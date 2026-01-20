namespace HospitalManagement.Domain.Entities;

/// <summary>
/// Represents feedback from patients about doctors
/// </summary>
public class Feedback
{
    /// <summary>
    /// Unique identifier for the feedback
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Patient ID who provided the feedback
    /// </summary>
    public int PatientId { get; set; }

    /// <summary>
    /// Doctor ID who received the feedback
    /// </summary>
    public int DoctorId { get; set; }

    /// <summary>
    /// Feedback text
    /// </summary>
    public string FeedbackText { get; set; } = string.Empty;

    /// <summary>
    /// Rating (1-5 stars)
    /// </summary>
    public int Rating { get; set; }

    /// <summary>
    /// Feedback date
    /// </summary>
    public DateTime FeedbackDate { get; set; }

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
