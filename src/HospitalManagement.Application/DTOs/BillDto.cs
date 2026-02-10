namespace HospitalManagement.Application.DTOs;

/// <summary>
/// Data transfer object for Bill entity
/// </summary>
public class BillDto
{
    public int Id { get; set; }
    public int PatientId { get; set; }
    public string PatientName { get; set; } = string.Empty;
    public int DoctorId { get; set; }
    public string DoctorName { get; set; } = string.Empty;
    public int? AppointmentId { get; set; }
    public decimal Amount { get; set; }
    public DateTime BillDate { get; set; }
    public string? Description { get; set; }
    public bool IsPaid { get; set; }
    public bool IsActive { get; set; }
}

public class BillCreateDto
{
    public int PatientId { get; set; }
    public int DoctorId { get; set; }
    public int? AppointmentId { get; set; }
    public decimal Amount { get; set; }
    public DateTime BillDate { get; set; }
    public string? Description { get; set; }
    public bool IsPaid { get; set; }
}

public class BillUpdateDto
{
    public decimal Amount { get; set; }
    public string? Description { get; set; }
    public bool IsPaid { get; set; }
}
