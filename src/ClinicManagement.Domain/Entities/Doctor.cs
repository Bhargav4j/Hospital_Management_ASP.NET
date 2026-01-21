namespace ClinicManagement.Domain.Entities;

public class Doctor
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Specialization { get; set; } = string.Empty;
    public string Qualification { get; set; } = string.Empty;
    public decimal ConsultationFee { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public bool IsActive { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public string? ModifiedBy { get; set; }

    public User? User { get; set; }
}
