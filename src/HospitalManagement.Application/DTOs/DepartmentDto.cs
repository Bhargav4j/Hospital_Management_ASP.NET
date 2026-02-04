namespace HospitalManagement.Application.DTOs;

/// <summary>
/// Data Transfer Object for Department entity
/// </summary>
public class DepartmentDto
{
    /// <summary>
    /// Gets or sets the department number
    /// </summary>
    public int DeptNo { get; set; }

    /// <summary>
    /// Gets or sets the department name
    /// </summary>
    public string DeptName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the department description
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the creation date
    /// </summary>
    public DateTime CreatedDate { get; set; }

    /// <summary>
    /// Gets or sets the modification date
    /// </summary>
    public DateTime? ModifiedDate { get; set; }

    /// <summary>
    /// Gets or sets whether the department is active
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
