using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.Domain.Entities;

public class Appointment
{
    public int Id { get; set; }

    public int PatientId { get; set; }

    public int DoctorId { get; set; }

    public DateTime AppointmentDate { get; set; }

    [MaxLength(50)]
    public string Status { get; set; } = "Pending";

    [MaxLength(500)]
    public string? Reason { get; set; }

    [MaxLength(1000)]
    public string? Notes { get; set; }

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    public DateTime? ModifiedDate { get; set; }

    public bool IsActive { get; set; } = true;

    [MaxLength(100)]
    public string CreatedBy { get; set; } = "System";

    [MaxLength(100)]
    public string? ModifiedBy { get; set; }

    public Patient? Patient { get; set; }
    public Doctor? Doctor { get; set; }
}
