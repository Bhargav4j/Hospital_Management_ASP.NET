namespace HospitalManagement.Application.DTOs;

/// <summary>
/// Data Transfer Object for updating an existing Patient
/// </summary>
public class PatientUpdateDto
{
    /// <summary>
    /// Gets or sets the patient identifier
    /// </summary>
    public int PatientID { get; set; }

    /// <summary>
    /// Gets or sets the patient name
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the patient email address
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the patient phone number
    /// </summary>
    public string Phone { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the patient address
    /// </summary>
    public string Address { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the patient birth date
    /// </summary>
    public DateTime BirthDate { get; set; }

    /// <summary>
    /// Gets or sets the patient gender
    /// </summary>
    public string Gender { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets whether the patient is active
    /// </summary>
    public bool IsActive { get; set; }
}
