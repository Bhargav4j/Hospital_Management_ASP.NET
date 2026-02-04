namespace HospitalManagement.Application.DTOs;

/// <summary>
/// Data Transfer Object for creating a new Doctor
/// </summary>
public class DoctorCreateDto
{
    /// <summary>
    /// Gets or sets the doctor name
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the doctor email address
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the doctor password
    /// </summary>
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the doctor phone number
    /// </summary>
    public string Phone { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the doctor address
    /// </summary>
    public string Address { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the doctor birth date
    /// </summary>
    public DateTime BirthDate { get; set; }

    /// <summary>
    /// Gets or sets the doctor gender
    /// </summary>
    public string Gender { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the department number
    /// </summary>
    public int DeptNo { get; set; }

    /// <summary>
    /// Gets or sets the years of experience
    /// </summary>
    public int Experience { get; set; }

    /// <summary>
    /// Gets or sets the doctor salary
    /// </summary>
    public decimal Salary { get; set; }

    /// <summary>
    /// Gets or sets the charges per visit
    /// </summary>
    public decimal ChargesPerVisit { get; set; }

    /// <summary>
    /// Gets or sets the specialization
    /// </summary>
    public string Specialization { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the qualification
    /// </summary>
    public string Qualification { get; set; } = string.Empty;
}
