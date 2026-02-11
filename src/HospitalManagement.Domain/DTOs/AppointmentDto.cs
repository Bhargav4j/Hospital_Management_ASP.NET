namespace HospitalManagement.Domain.DTOs;

public class AppointmentDto
{
    public int Id { get; set; }
    public int PatientId { get; set; }
    public string PatientName { get; set; } = string.Empty;
    public int DoctorId { get; set; }
    public string DoctorName { get; set; } = string.Empty;
    public DateTime AppointmentDate { get; set; }
    public string? Symptoms { get; set; }
    public string Status { get; set; } = "Pending";
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
    public string Status { get; set; } = "Pending";
}
