namespace ClinicManagement.Application.DTOs;

public class DoctorDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Specialization { get; set; } = string.Empty;
    public string Qualification { get; set; } = string.Empty;
    public int Experience { get; set; }
    public decimal ConsultationFee { get; set; }
    public string? ProfileImage { get; set; }
    public bool IsActive { get; set; }
}

public class DoctorCreateDto
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Specialization { get; set; } = string.Empty;
    public string Qualification { get; set; } = string.Empty;
    public int Experience { get; set; }
    public decimal ConsultationFee { get; set; }
}

public class DoctorUpdateDto
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Specialization { get; set; } = string.Empty;
    public string Qualification { get; set; } = string.Empty;
    public int Experience { get; set; }
    public decimal ConsultationFee { get; set; }
}
