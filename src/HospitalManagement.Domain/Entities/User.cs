using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.Domain.Entities;

public class User
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string Password { get; set; } = string.Empty;

    public int UserType { get; set; }

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    public DateTime? ModifiedDate { get; set; }

    public bool IsActive { get; set; } = true;

    [MaxLength(100)]
    public string CreatedBy { get; set; } = "System";

    [MaxLength(100)]
    public string? ModifiedBy { get; set; }
}
