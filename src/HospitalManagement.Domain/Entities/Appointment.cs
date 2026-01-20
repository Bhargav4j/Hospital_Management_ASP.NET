using HospitalManagement.Domain.Enums;

namespace HospitalManagement.Domain.Entities;

/// <summary>
/// Represents an appointment in the hospital management system
/// </summary>
public class Appointment
{
    /// <summary>
    /// Unique identifier for the appointment
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Patient ID associated with the appointment
    /// </summary>
    public int PatientId { get; set; }

    /// <summary>
    /// Doctor ID associated with the appointment
    /// </summary>
    public int DoctorId { get; set; }

    /// <summary>
    /// Date of the appointment
    /// </summary>
    public DateTime AppointmentDate { get; set; }

    /// <summary>
    /// Time slot of the appointment
    /// </summary>
    public string TimeSlot { get; set; } = string.Empty;

    /// <summary>
    /// Status of the appointment
    /// </summary>
    public AppointmentStatus Status { get; set; } = AppointmentStatus.Pending;

    /// <summary>
    /// Reason for the appointment
    /// </summary>
    public string Reason { get; set; } = string.Empty;

    /// <summary>
    /// Notes or remarks about the appointment
    /// </summary>
    public string? Notes { get; set; }

    /// <summary>
    /// Indicates if the appointment is active
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Date when the appointment was created
    /// </summary>
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Date when the appointment was last modified
    /// </summary>
    public DateTime? ModifiedDate { get; set; }

    /// <summary>
    /// User who created the appointment
    /// </summary>
    public string CreatedBy { get; set; } = "System";

    /// <summary>
    /// User who last modified the appointment
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
