namespace HospitalManagement.Domain.Entities;

public class MedicalHistory
{
    public int Id { get; set; }
    public int PatientId { get; set; }
    public User Patient { get; set; } = null!;
    public string Diagnosis { get; set; } = string.Empty;
    public string Treatment { get; set; } = string.Empty;
    public DateTime VisitDate { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public bool IsActive { get; set; }
    public string CreatedBy { get; set; } = "System";
    public string? ModifiedBy { get; set; }
}
