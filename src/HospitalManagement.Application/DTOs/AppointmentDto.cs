namespace HospitalManagement.Application.DTOs;

/// <summary>
/// Data Transfer Object for Appointment entity
/// </summary>
public class AppointmentDto
{
    /// <summary>
    /// Gets or sets the appointment identifier
    /// </summary>
    public int AppointmentID { get; set; }

    /// <summary>
    /// Gets or sets the patient identifier
    /// </summary>
    public int PatientID { get; set; }

    /// <summary>
    /// Gets or sets the doctor identifier
    /// </summary>
    public int DoctorID { get; set; }

    /// <summary>
    /// Gets or sets the free slot identifier
    /// </summary>
    public int FreeSlotID { get; set; }

    /// <summary>
    /// Gets or sets the appointment date
    /// </summary>
    public DateTime AppointmentDate { get; set; }

    /// <summary>
    /// Gets or sets the appointment status
    /// </summary>
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the disease information
    /// </summary>
    public string? Disease { get; set; }

    /// <summary>
    /// Gets or sets the progress information
    /// </summary>
    public string? Progress { get; set; }

    /// <summary>
    /// Gets or sets the prescription
    /// </summary>
    public string? Prescription { get; set; }

    /// <summary>
    /// Gets or sets whether the appointment is paid
    /// </summary>
    public bool IsPaid { get; set; }

    /// <summary>
    /// Gets or sets whether feedback has been given
    /// </summary>
    public bool FeedbackGiven { get; set; }

    /// <summary>
    /// Gets or sets the creation date
    /// </summary>
    public DateTime CreatedDate { get; set; }

    /// <summary>
    /// Gets or sets the modification date
    /// </summary>
    public DateTime? ModifiedDate { get; set; }

    /// <summary>
    /// Gets or sets whether the appointment is active
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Gets or sets who created the record
    /// </summary>
    public string CreatedBy { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets who modified the record
    /// </summary>
    public string? ModifiedBy { get; set; }

    /// <summary>
    /// Gets or sets the patient name
    /// </summary>
    public string? PatientName { get; set; }

    /// <summary>
    /// Gets or sets the doctor name
    /// </summary>
    public string? DoctorName { get; set; }
}
