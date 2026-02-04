using FluentValidation;
using HospitalManagement.Application.DTOs;

namespace HospitalManagement.Application.Validators;

/// <summary>
/// Validator for PatientUpdateDto
/// </summary>
public class PatientUpdateDtoValidator : AbstractValidator<PatientUpdateDto>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PatientUpdateDtoValidator"/> class
    /// </summary>
    public PatientUpdateDtoValidator()
    {
        RuleFor(x => x.PatientID)
            .GreaterThan(0).WithMessage("Patient ID must be greater than zero");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(100).WithMessage("Name cannot exceed 100 characters");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email format")
            .MaximumLength(100).WithMessage("Email cannot exceed 100 characters");

        RuleFor(x => x.Phone)
            .NotEmpty().WithMessage("Phone is required")
            .Matches(@"^\+?[1-9]\d{1,14}$").WithMessage("Invalid phone number format")
            .MaximumLength(20).WithMessage("Phone cannot exceed 20 characters");

        RuleFor(x => x.Address)
            .NotEmpty().WithMessage("Address is required")
            .MaximumLength(500).WithMessage("Address cannot exceed 500 characters");

        RuleFor(x => x.BirthDate)
            .NotEmpty().WithMessage("Birth date is required")
            .LessThan(DateTime.UtcNow).WithMessage("Birth date must be in the past")
            .GreaterThan(DateTime.UtcNow.AddYears(-150)).WithMessage("Birth date is not valid");

        RuleFor(x => x.Gender)
            .NotEmpty().WithMessage("Gender is required")
            .Must(g => g == "Male" || g == "Female" || g == "Other")
            .WithMessage("Gender must be Male, Female, or Other");
    }
}
