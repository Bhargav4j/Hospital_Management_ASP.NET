using System;
using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.Application.DTOs
{
    public class BillDto
    {
        public int Id { get; set; }

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

        [Required]
        [Range(0, double.MaxValue)]
        public decimal TotalAmount { get; set; }

        [Required]
        [StringLength(50)]
        public string PaymentStatus { get; set; } = string.Empty;

        [StringLength(50)]
        public string? PaymentMethod { get; set; }

        public DateTime? PaymentDate { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        // Navigation properties
        public string? PatientName { get; set; }
    }
}
