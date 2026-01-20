using System;
using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.Application.DTOs
{
    public class BillCreateDto
    {
        [Required]
        public int PatientId { get; set; }

        public int? AppointmentId { get; set; }

        [Required]
        public DateTime BillDate { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal ConsultationCharges { get; set; }

        [Range(0, double.MaxValue)]
        public decimal MedicineCharges { get; set; }

        [Range(0, double.MaxValue)]
        public decimal TestCharges { get; set; }

        [Range(0, double.MaxValue)]
        public decimal OtherCharges { get; set; }

        [StringLength(50)]
        public string? PaymentMethod { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }
    }
}
