namespace HospitalManagement.Domain.DTOs;

public class FeedbackDto
{
    public int Id { get; set; }
    public int PatientId { get; set; }
    public string PatientName { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public int Rating { get; set; }
    public DateTime CreatedDate { get; set; }
}

public class FeedbackCreateDto
{
    public int PatientId { get; set; }
    public string Message { get; set; } = string.Empty;
    public int Rating { get; set; }
}

public class FeedbackUpdateDto
{
    public string Message { get; set; } = string.Empty;
    public int Rating { get; set; }
}
