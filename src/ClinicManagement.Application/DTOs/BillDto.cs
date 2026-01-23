namespace ClinicManagement.Application.DTOs;

public class BillDto
{
    public int Id { get; set; }
    public int PatientId { get; set; }
    public int? DoctorId { get; set; }
    public string PatientName { get; set; } = string.Empty;
    public string? DoctorName { get; set; }
    public decimal Amount { get; set; }
    public DateTime BillDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? Description { get; set; }
}

public class BillCreateDto
{
    public int PatientId { get; set; }
    public int? DoctorId { get; set; }
    public decimal Amount { get; set; }
    public string? Description { get; set; }
}

public class BillUpdateDto
{
    public decimal Amount { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? Description { get; set; }
}
