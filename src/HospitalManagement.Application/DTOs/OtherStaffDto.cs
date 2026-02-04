namespace HospitalManagement.Application.DTOs;

/// <summary>
/// Data Transfer Object for OtherStaff entity
/// </summary>
public class OtherStaffDto
{
    /// <summary>
    /// Gets or sets the staff identifier
    /// </summary>
    public int StaffID { get; set; }

    /// <summary>
    /// Gets or sets the staff name
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the staff phone number
    /// </summary>
    public string Phone { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the staff address
    /// </summary>
    public string Address { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the staff birth date
    /// </summary>
    public DateTime BirthDate { get; set; }

    /// <summary>
    /// Gets or sets the staff gender
    /// </summary>
    public string Gender { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the staff designation
    /// </summary>
    public string Designation { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the staff qualification
    /// </summary>
    public string Qualification { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the staff salary
    /// </summary>
    public decimal Salary { get; set; }

    /// <summary>
    /// Gets or sets the creation date
    /// </summary>
    public DateTime CreatedDate { get; set; }

    /// <summary>
    /// Gets or sets the modification date
    /// </summary>
    public DateTime? ModifiedDate { get; set; }

    /// <summary>
    /// Gets or sets whether the staff is active
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
}
