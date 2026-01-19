namespace ClinicManagement.Domain.Entities;

public class Doctor
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    public string Specialization { get; set; } = string.Empty;
    public string Qualifications { get; set; } = string.Empty;
    public int ClinicId { get; set; }
    public Clinic Clinic { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
}
