using System;
using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.Application.DTOs
{
    public class FeedbackCreateDto
    {
        [Required]
        public int PatientId { get; set; }

        [Required]
        public int DoctorId { get; set; }

        public int? AppointmentId { get; set; }

        [Required]
        [Range(1, 5)]
        public int Rating { get; set; }

        [StringLength(1000)]
        public string? Comments { get; set; }

        [Required]
        public DateTime FeedbackDate { get; set; }
    }
}
