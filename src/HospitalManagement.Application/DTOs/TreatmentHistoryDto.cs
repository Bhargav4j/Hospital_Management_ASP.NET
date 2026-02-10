namespace HospitalManagement.Application.DTOs;

/// <summary>
/// Data transfer object for TreatmentHistory entity
/// </summary>
public class TreatmentHistoryDto
{
    public int Id { get; set; }
    public int AppointmentId { get; set; }
    public string? Prescription { get; set; }
    public string? Notes { get; set; }
    public DateTime TreatmentDate { get; set; }
    public bool IsActive { get; set; }
}

public class TreatmentHistoryCreateDto
{
    public int AppointmentId { get; set; }
    public string? Prescription { get; set; }
    public string? Notes { get; set; }
    public DateTime TreatmentDate { get; set; }
}

public class TreatmentHistoryUpdateDto
{
    public string? Prescription { get; set; }
    public string? Notes { get; set; }
    public DateTime TreatmentDate { get; set; }
}
