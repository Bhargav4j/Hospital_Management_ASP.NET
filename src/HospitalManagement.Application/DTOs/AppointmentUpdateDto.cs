namespace HospitalManagement.Application.DTOs;

/// <summary>
/// Data Transfer Object for updating an existing Appointment
/// </summary>
public class AppointmentUpdateDto
{
    /// <summary>
    /// Gets or sets the appointment identifier
    /// </summary>
    public int AppointmentID { get; set; }

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
    /// Gets or sets whether the appointment is active
    /// </summary>
    public bool IsActive { get; set; }
}
