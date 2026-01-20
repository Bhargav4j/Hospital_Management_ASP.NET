using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.Application.DTOs
{
    public class AdminCreateDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string Password { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string Phone { get; set; } = string.Empty;

        [StringLength(50)]
        public string? Role { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
