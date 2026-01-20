namespace HospitalManagement.Domain.Entities;

public class Bill
{
    public int BillID { get; set; }
    public int PatientID { get; set; }
    public int AppointmentID { get; set; }
    public decimal Amount { get; set; }
    public DateTime BillDate { get; set; }
    public bool IsPaid { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public bool IsActive { get; set; }
    public string CreatedBy { get; set; } = "System";
    public string? ModifiedBy { get; set; }

    public virtual Patient? Patient { get; set; }
    public virtual Appointment? Appointment { get; set; }
}
