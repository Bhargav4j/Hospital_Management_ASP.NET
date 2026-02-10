using HospitalManagement.Domain.Enums;

namespace HospitalManagement.Application.DTOs;

/// <summary>
/// Data transfer object for Appointment entity
/// </summary>
public class AppointmentDto
{
    public int Id { get; set; }
    public int PatientId { get; set; }
    public string PatientName { get; set; } = string.Empty;
    public int DoctorId { get; set; }
    public string DoctorName { get; set; } = string.Empty;
    public DateTime AppointmentDate { get; set; }
    public string? Symptoms { get; set; }
    public string? Diagnosis { get; set; }
    public AppointmentStatus Status { get; set; }
    public string StatusText { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}

public class AppointmentCreateDto
{
    public int PatientId { get; set; }
    public int DoctorId { get; set; }
    public DateTime AppointmentDate { get; set; }
    public string? Symptoms { get; set; }
}

public class AppointmentUpdateDto
{
    public DateTime AppointmentDate { get; set; }
    public string? Symptoms { get; set; }
    public string? Diagnosis { get; set; }
    public AppointmentStatus Status { get; set; }
}
