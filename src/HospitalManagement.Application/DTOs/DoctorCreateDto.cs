using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.Application.DTOs
{
    public class DoctorCreateDto
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

        [Required]
        [StringLength(100)]
        public string Specialization { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Qualification { get; set; } = string.Empty;

        [Required]
        [Range(0, 50)]
        public int Experience { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal ConsultationFee { get; set; }

        [StringLength(500)]
        public string? Address { get; set; }

        public bool IsAvailable { get; set; } = true;
    }
}
