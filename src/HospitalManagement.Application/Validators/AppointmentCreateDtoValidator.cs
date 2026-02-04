using FluentValidation;
using HospitalManagement.Application.DTOs;

namespace HospitalManagement.Application.Validators;

/// <summary>
/// Validator for AppointmentCreateDto
/// </summary>
public class AppointmentCreateDtoValidator : AbstractValidator<AppointmentCreateDto>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AppointmentCreateDtoValidator"/> class
    /// </summary>
    public AppointmentCreateDtoValidator()
    {
        RuleFor(x => x.PatientID)
            .GreaterThan(0).WithMessage("Patient ID must be greater than zero");

        RuleFor(x => x.DoctorID)
            .GreaterThan(0).WithMessage("Doctor ID must be greater than zero");

        RuleFor(x => x.FreeSlotID)
            .GreaterThan(0).WithMessage("Free slot ID must be greater than zero");

        RuleFor(x => x.AppointmentDate)
            .NotEmpty().WithMessage("Appointment date is required")
            .GreaterThanOrEqualTo(DateTime.UtcNow.Date).WithMessage("Appointment date cannot be in the past");

        RuleFor(x => x.Status)
            .NotEmpty().WithMessage("Status is required")
            .MaximumLength(50).WithMessage("Status cannot exceed 50 characters")
            .Must(s => s == "Pending" || s == "Confirmed" || s == "Completed" || s == "Cancelled")
            .WithMessage("Status must be Pending, Confirmed, Completed, or Cancelled");

        RuleFor(x => x.Disease)
            .MaximumLength(500).WithMessage("Disease description cannot exceed 500 characters")
            .When(x => !string.IsNullOrEmpty(x.Disease));
    }
}
