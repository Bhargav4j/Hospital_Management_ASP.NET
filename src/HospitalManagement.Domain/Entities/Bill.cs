namespace HospitalManagement.Domain.Entities;

public class Bill
{
    public int Id { get; set; }
    public int PatientId { get; set; }
    public User Patient { get; set; } = null!;
    public decimal Amount { get; set; }
    public DateTime BillDate { get; set; }
    public string Description { get; set; } = string.Empty;
    public bool IsPaid { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public bool IsActive { get; set; }
    public string CreatedBy { get; set; } = "System";
    public string? ModifiedBy { get; set; }
}
