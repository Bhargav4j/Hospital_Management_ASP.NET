using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.Domain.Entities;

public class TreatmentHistory
{
    public int Id { get; set; }

    public int PatientId { get; set; }

    public int DoctorId { get; set; }

    public int? AppointmentId { get; set; }

    public DateTime TreatmentDate { get; set; } = DateTime.UtcNow;

    [MaxLength(200)]
    public string? Diagnosis { get; set; }

    [MaxLength(1000)]
    public string? Treatment { get; set; }

    [MaxLength(500)]
    public string? Prescription { get; set; }

    [MaxLength(1000)]
    public string? Notes { get; set; }

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    public DateTime? ModifiedDate { get; set; }

    public bool IsActive { get; set; } = true;

    [MaxLength(100)]
    public string CreatedBy { get; set; } = "System";

    [MaxLength(100)]
    public string? ModifiedBy { get; set; }
}
