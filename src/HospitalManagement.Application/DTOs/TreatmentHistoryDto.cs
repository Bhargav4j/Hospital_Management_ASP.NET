using System;
using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.Application.DTOs
{
    public class TreatmentHistoryDto
    {
        public int Id { get; set; }

        [Required]
        public int PatientId { get; set; }

        [Required]
        public int DoctorId { get; set; }

        public int? AppointmentId { get; set; }

        [Required]
        public DateTime TreatmentDate { get; set; }

        [Required]
        [StringLength(200)]
        public string Diagnosis { get; set; } = string.Empty;

        [Required]
        [StringLength(1000)]
        public string Prescription { get; set; } = string.Empty;

        [StringLength(1000)]
        public string? Treatment { get; set; }

        [StringLength(1000)]
        public string? Notes { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        // Navigation properties
        public string? PatientName { get; set; }
        public string? DoctorName { get; set; }
    }
}
