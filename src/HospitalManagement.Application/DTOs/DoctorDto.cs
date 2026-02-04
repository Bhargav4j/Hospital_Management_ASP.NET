namespace HospitalManagement.Application.DTOs;

/// <summary>
/// Data Transfer Object for Doctor entity
/// </summary>
public class DoctorDto
{
    /// <summary>
    /// Gets or sets the doctor identifier
    /// </summary>
    public int DoctorID { get; set; }

    /// <summary>
    /// Gets or sets the doctor name
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the doctor email address
    /// </summary>
    public string Email { get; set; } = string.Empty;

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

    /// <summary>
    /// Gets or sets the reputation index
    /// </summary>
    public float ReputationIndex { get; set; }

    /// <summary>
    /// Gets or sets the number of patients treated
    /// </summary>
    public int PatientsTreated { get; set; }

    /// <summary>
    /// Gets or sets the doctor status
    /// </summary>
    public bool Status { get; set; }

    /// <summary>
    /// Gets or sets the creation date
    /// </summary>
    public DateTime CreatedDate { get; set; }

    /// <summary>
    /// Gets or sets the modification date
    /// </summary>
    public DateTime? ModifiedDate { get; set; }

    /// <summary>
    /// Gets or sets whether the doctor is active
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Gets or sets who created the record
    /// </summary>
    public string CreatedBy { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets who modified the record
    /// </summary>
    public string? ModifiedBy { get; set; }

    /// <summary>
    /// Gets or sets the department name
    /// </summary>
    public string? DepartmentName { get; set; }
}
