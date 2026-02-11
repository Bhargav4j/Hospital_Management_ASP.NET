using HospitalManagement.Domain.Enums;

namespace HospitalManagement.Domain.Entities;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime BirthDate { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string PhoneNo { get; set; } = string.Empty;
    public Gender Gender { get; set; }
    public string Address { get; set; } = string.Empty;
    public UserType UserType { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public bool IsActive { get; set; }
    public string CreatedBy { get; set; } = "System";
    public string? ModifiedBy { get; set; }

    public ICollection<Appointment> AppointmentsAsPatient { get; set; } = new List<Appointment>();
    public ICollection<Appointment> AppointmentsAsDoctor { get; set; } = new List<Appointment>();
    public ICollection<Bill> Bills { get; set; } = new List<Bill>();
    public ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();
    public ICollection<MedicalHistory> MedicalHistories { get; set; } = new List<MedicalHistory>();
}
