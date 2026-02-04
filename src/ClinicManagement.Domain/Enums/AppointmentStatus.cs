namespace ClinicManagement.Domain.Enums;

/// <summary>
/// Represents the status of an appointment
/// </summary>
public enum AppointmentStatus
{
    Pending = 1,
    Approved = 2,
    Rejected = 3,
    Completed = 4,
    Cancelled = 5
}
