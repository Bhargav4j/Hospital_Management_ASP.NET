namespace HospitalManagement.Domain.Entities;

public class Appointment
{
    public int Id { get; set; }
    public int PatientId { get; set; }
    public User Patient { get; set; } = null!;
    public int DoctorId { get; set; }
    public User Doctor { get; set; } = null!;
    public DateTime AppointmentDate { get; set; }
    public string? Symptoms { get; set; }
    public string Status { get; set; } = "Pending";
    public DateTime CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public bool IsActive { get; set; }
    public string CreatedBy { get; set; } = "System";
    public string? ModifiedBy { get; set; }
}
