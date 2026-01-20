namespace HospitalManagement.Domain.Entities;

/// <summary>
/// Represents an administrator in the hospital management system
/// </summary>
public class Admin
{
    /// <summary>
    /// Unique identifier for the admin
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Admin's full name
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Admin's email address
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Admin's password (should be hashed)
    /// </summary>
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// Indicates if the admin is active
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Date when the admin was created
    /// </summary>
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Date when the admin was last modified
    /// </summary>
    public DateTime? ModifiedDate { get; set; }

    /// <summary>
    /// User who created the admin record
    /// </summary>
    public string CreatedBy { get; set; } = "System";

    /// <summary>
    /// User who last modified the admin record
    /// </summary>
    public string? ModifiedBy { get; set; }
}
