namespace ClinicManagement.Domain.Entities;

public class Doctor
{
    public int DoctorID { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public DateTime BirthDate { get; set; }
    public string Gender { get; set; } = string.Empty;
    public int DeptNo { get; set; }
    public int Experience { get; set; }
    public decimal Salary { get; set; }
    public decimal ChargesPerVisit { get; set; }
    public string Specialization { get; set; } = string.Empty;
    public string Qualification { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public DateTime? ModifiedDate { get; set; }
    
    // Navigation properties
    public virtual Department Department { get; set; } = null!;
    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
}
