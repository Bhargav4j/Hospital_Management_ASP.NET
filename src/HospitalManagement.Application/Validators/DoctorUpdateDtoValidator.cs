using FluentValidation;
using HospitalManagement.Application.DTOs;

namespace HospitalManagement.Application.Validators;

/// <summary>
/// Validator for DoctorUpdateDto
/// </summary>
public class DoctorUpdateDtoValidator : AbstractValidator<DoctorUpdateDto>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DoctorUpdateDtoValidator"/> class
    /// </summary>
    public DoctorUpdateDtoValidator()
    {
        RuleFor(x => x.DoctorID)
            .GreaterThan(0).WithMessage("Doctor ID must be greater than zero");

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
            .GreaterThan(DateTime.UtcNow.AddYears(-100)).WithMessage("Birth date is not valid");

        RuleFor(x => x.Gender)
            .NotEmpty().WithMessage("Gender is required")
            .Must(g => g == "Male" || g == "Female" || g == "Other")
            .WithMessage("Gender must be Male, Female, or Other");

        RuleFor(x => x.DeptNo)
            .GreaterThan(0).WithMessage("Department number must be greater than zero");

        RuleFor(x => x.Experience)
            .GreaterThanOrEqualTo(0).WithMessage("Experience must be zero or positive")
            .LessThan(100).WithMessage("Experience cannot exceed 100 years");

        RuleFor(x => x.Salary)
            .GreaterThan(0).WithMessage("Salary must be greater than zero")
            .LessThan(10000000).WithMessage("Salary value is not valid");

        RuleFor(x => x.ChargesPerVisit)
            .GreaterThan(0).WithMessage("Charges per visit must be greater than zero")
            .LessThan(100000).WithMessage("Charges per visit value is not valid");

        RuleFor(x => x.Specialization)
            .NotEmpty().WithMessage("Specialization is required")
            .MaximumLength(100).WithMessage("Specialization cannot exceed 100 characters");

        RuleFor(x => x.Qualification)
            .NotEmpty().WithMessage("Qualification is required")
            .MaximumLength(200).WithMessage("Qualification cannot exceed 200 characters");
    }
}
