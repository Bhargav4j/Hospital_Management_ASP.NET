namespace HospitalManagement.Domain.DTOs;

public class BillDto
{
    public int Id { get; set; }
    public int PatientId { get; set; }
    public string PatientName { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateTime BillDate { get; set; }
    public string Description { get; set; } = string.Empty;
    public bool IsPaid { get; set; }
}

public class BillCreateDto
{
    public int PatientId { get; set; }
    public decimal Amount { get; set; }
    public string Description { get; set; } = string.Empty;
}

public class BillUpdateDto
{
    public decimal Amount { get; set; }
    public string Description { get; set; } = string.Empty;
    public bool IsPaid { get; set; }
}
