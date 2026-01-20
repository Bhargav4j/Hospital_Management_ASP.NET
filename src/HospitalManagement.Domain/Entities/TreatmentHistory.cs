namespace HospitalManagement.Domain.Entities;

/// <summary>
/// Represents treatment history for a patient
/// </summary>
public class TreatmentHistory
{
    /// <summary>
    /// Unique identifier for the treatment history
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Patient ID associated with the treatment
    /// </summary>
    public int PatientId { get; set; }

    /// <summary>
    /// Doctor ID who provided the treatment
    /// </summary>
    public int DoctorId { get; set; }

    /// <summary>
    /// Date of the treatment
    /// </summary>
    public DateTime TreatmentDate { get; set; }

    /// <summary>
    /// Diagnosis provided by the doctor
    /// </summary>
    public string Diagnosis { get; set; } = string.Empty;

    /// <summary>
    /// Prescription details
    /// </summary>
    public string Prescription { get; set; } = string.Empty;

    /// <summary>
    /// Medical tests recommended
    /// </summary>
    public string? MedicalTests { get; set; }

    /// <summary>
    /// Next visit date
    /// </summary>
    public DateTime? NextVisitDate { get; set; }

    /// <summary>
    /// Additional notes about the treatment
    /// </summary>
    public string? Notes { get; set; }

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
