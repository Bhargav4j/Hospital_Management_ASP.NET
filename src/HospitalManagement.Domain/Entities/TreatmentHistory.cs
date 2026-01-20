namespace HospitalManagement.Domain.Entities;

public class TreatmentHistory
{
    public int TreatmentID { get; set; }
    public int PatientID { get; set; }
    public int DoctorID { get; set; }
    public DateTime TreatmentDate { get; set; }
    public string Disease { get; set; } = string.Empty;
    public string Prescription { get; set; } = string.Empty;
    public string Progress { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public bool IsActive { get; set; }
    public string CreatedBy { get; set; } = "System";
    public string? ModifiedBy { get; set; }

    public virtual Patient? Patient { get; set; }
}
