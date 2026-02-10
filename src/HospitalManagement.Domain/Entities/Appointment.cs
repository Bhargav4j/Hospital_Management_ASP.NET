using HospitalManagement.Domain.Enums;

namespace HospitalManagement.Domain.Entities;

/// <summary>
/// Represents an appointment between a patient and a doctor
/// </summary>
public class Appointment
{
    public int Id { get; set; }
    public int PatientId { get; set; }
    public int DoctorId { get; set; }
    public DateTime AppointmentDate { get; set; }
    public string? Symptoms { get; set; }
    public string? Diagnosis { get; set; }
    public AppointmentStatus Status { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public bool IsActive { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public string? ModifiedBy { get; set; }

    // Navigation properties
    public Patient Patient { get; set; } = null!;
    public Doctor Doctor { get; set; } = null!;
    public ICollection<TreatmentHistory> TreatmentHistories { get; set; } = new List<TreatmentHistory>();
}
