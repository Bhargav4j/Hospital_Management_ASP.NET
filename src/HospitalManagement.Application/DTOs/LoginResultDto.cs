using System.ComponentModel.DataAnnotations;

namespace HospitalManagement.Application.DTOs
{
    public class LoginResultDto
    {
        public bool IsSuccess { get; set; }

        public int UserId { get; set; }

        [StringLength(50)]
        public string UserType { get; set; } = string.Empty;

        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [StringLength(500)]
        public string Message { get; set; } = string.Empty;

        public string? Token { get; set; }
    }
}
