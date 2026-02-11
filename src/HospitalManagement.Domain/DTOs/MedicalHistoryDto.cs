namespace HospitalManagement.Domain.DTOs;

public class MedicalHistoryDto
{
    public int Id { get; set; }
    public int PatientId { get; set; }
    public string PatientName { get; set; } = string.Empty;
    public string Diagnosis { get; set; } = string.Empty;
    public string Treatment { get; set; } = string.Empty;
    public DateTime VisitDate { get; set; }
    public string? Notes { get; set; }
}

public class MedicalHistoryCreateDto
{
    public int PatientId { get; set; }
    public string Diagnosis { get; set; } = string.Empty;
    public string Treatment { get; set; } = string.Empty;
    public DateTime VisitDate { get; set; }
    public string? Notes { get; set; }
}

public class MedicalHistoryUpdateDto
{
    public string Diagnosis { get; set; } = string.Empty;
    public string Treatment { get; set; } = string.Empty;
    public DateTime VisitDate { get; set; }
    public string? Notes { get; set; }
}
