using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.Domain.Entities;

public class Bill
{
    public int Id { get; set; }

    public int PatientId { get; set; }

    public int? AppointmentId { get; set; }

    public decimal Amount { get; set; }

    public DateTime BillDate { get; set; } = DateTime.UtcNow;

    [MaxLength(50)]
    public string Status { get; set; } = "Pending";

    [MaxLength(500)]
    public string? Description { get; set; }

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    public DateTime? ModifiedDate { get; set; }

    public bool IsActive { get; set; } = true;

    [MaxLength(100)]
    public string CreatedBy { get; set; } = "System";

    [MaxLength(100)]
    public string? ModifiedBy { get; set; }

    public Patient? Patient { get; set; }
}
