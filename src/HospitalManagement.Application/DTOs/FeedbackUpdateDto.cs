using System;
using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.Application.DTOs
{
    public class FeedbackUpdateDto
    {
        [Required]
        [Range(1, 5)]
        public int Rating { get; set; }

        [StringLength(1000)]
        public string? Comments { get; set; }

        [Required]
        public DateTime FeedbackDate { get; set; }
    }
}
