namespace HospitalManagement.Application.DTOs;

/// <summary>
/// Data Transfer Object for creating a new Appointment
/// </summary>
public class AppointmentCreateDto
{
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
}
