using System;
using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.Application.DTOs
{
    public class AppointmentDto
    {
        public int Id { get; set; }

        [Required]
        public int PatientId { get; set; }

        [Required]
        public int DoctorId { get; set; }

        [Required]
        public DateTime AppointmentDate { get; set; }

        [Required]
        [StringLength(50)]
        public string TimeSlot { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Status { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Reason { get; set; }

        [StringLength(1000)]
        public string? Notes { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        // Navigation properties
        public string? PatientName { get; set; }
        public string? DoctorName { get; set; }
        public string? DoctorSpecialization { get; set; }
    }
}
