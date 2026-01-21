namespace ClinicManagement.Application.DTOs;

public class AppointmentDto
{
    public int Id { get; set; }
    public int PatientId { get; set; }
    public int DoctorId { get; set; }
    public DateTime AppointmentDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? Symptoms { get; set; }
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
    public string Status { get; set; } = string.Empty;
    public string? Symptoms { get; set; }
}
