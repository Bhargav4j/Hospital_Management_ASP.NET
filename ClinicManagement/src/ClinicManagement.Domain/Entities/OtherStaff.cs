namespace ClinicManagement.Domain.Entities;

public class OtherStaff
{
    public int StaffID { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public DateTime BirthDate { get; set; }
    public string Gender { get; set; } = string.Empty;
    public string Designation { get; set; } = string.Empty;
    public decimal Salary { get; set; }
    public string Qualification { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public DateTime? ModifiedDate { get; set; }
}
