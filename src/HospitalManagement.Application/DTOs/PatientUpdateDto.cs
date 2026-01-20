using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.Application.DTOs
{
    public class PatientUpdateDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string Phone { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Address { get; set; }

        [Required]
        [Range(1, 120)]
        public int Age { get; set; }

        [Required]
        [StringLength(10)]
        public string Gender { get; set; } = string.Empty;

        [StringLength(50)]
        public string? BloodGroup { get; set; }

        [StringLength(1000)]
        public string? MedicalHistory { get; set; }
    }
}
