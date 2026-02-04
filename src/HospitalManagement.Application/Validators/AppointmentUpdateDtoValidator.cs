using FluentValidation;
using HospitalManagement.Application.DTOs;

namespace HospitalManagement.Application.Validators;

/// <summary>
/// Validator for AppointmentUpdateDto
/// </summary>
public class AppointmentUpdateDtoValidator : AbstractValidator<AppointmentUpdateDto>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AppointmentUpdateDtoValidator"/> class
    /// </summary>
    public AppointmentUpdateDtoValidator()
    {
        RuleFor(x => x.AppointmentID)
            .GreaterThan(0).WithMessage("Appointment ID must be greater than zero");

        RuleFor(x => x.AppointmentDate)
            .NotEmpty().WithMessage("Appointment date is required");

        RuleFor(x => x.Status)
            .NotEmpty().WithMessage("Status is required")
            .MaximumLength(50).WithMessage("Status cannot exceed 50 characters")
            .Must(s => s == "Pending" || s == "Confirmed" || s == "Completed" || s == "Cancelled")
            .WithMessage("Status must be Pending, Confirmed, Completed, or Cancelled");

        RuleFor(x => x.Disease)
            .MaximumLength(500).WithMessage("Disease description cannot exceed 500 characters")
            .When(x => !string.IsNullOrEmpty(x.Disease));

        RuleFor(x => x.Progress)
            .MaximumLength(1000).WithMessage("Progress description cannot exceed 1000 characters")
            .When(x => !string.IsNullOrEmpty(x.Progress));

        RuleFor(x => x.Prescription)
            .MaximumLength(2000).WithMessage("Prescription cannot exceed 2000 characters")
            .When(x => !string.IsNullOrEmpty(x.Prescription));
    }
}
