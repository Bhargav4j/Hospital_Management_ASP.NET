using System;
using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.Application.DTOs
{
    public class TreatmentHistoryUpdateDto
    {
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
    }
}
