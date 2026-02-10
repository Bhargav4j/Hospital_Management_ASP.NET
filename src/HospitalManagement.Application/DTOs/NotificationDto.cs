namespace HospitalManagement.Application.DTOs;

/// <summary>
/// Data transfer object for Notification entity
/// </summary>
public class NotificationDto
{
    public int Id { get; set; }
    public int PatientId { get; set; }
    public string Message { get; set; } = string.Empty;
    public bool IsRead { get; set; }
    public DateTime CreatedDate { get; set; }
    public bool IsActive { get; set; }
}

public class NotificationCreateDto
{
    public int PatientId { get; set; }
    public string Message { get; set; } = string.Empty;
}

public class NotificationUpdateDto
{
    public bool IsRead { get; set; }
}
