namespace ClinicManagement.Domain.Entities;

public class Department
{
    public int DeptNo { get; set; }
    public string DeptName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public DateTime? ModifiedDate { get; set; }
    
    // Navigation properties
    public virtual ICollection<Doctor> Doctors { get; set; } = new List<Doctor>();
}
