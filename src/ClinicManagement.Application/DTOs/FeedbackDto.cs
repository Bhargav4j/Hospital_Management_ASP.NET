namespace ClinicManagement.Application.DTOs;

public class FeedbackDto
{
    public int Id { get; set; }
    public int PatientId { get; set; }
    public string PatientName { get; set; } = string.Empty;
    public int? DoctorId { get; set; }
    public string? DoctorName { get; set; }
    public string Subject { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public int Rating { get; set; }
    public DateTime CreatedDate { get; set; }
}

public class FeedbackCreateDto
{
    public int PatientId { get; set; }
    public int? DoctorId { get; set; }
    public string Subject { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public int Rating { get; set; }
}
