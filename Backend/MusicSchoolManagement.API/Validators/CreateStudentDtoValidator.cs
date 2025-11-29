using FluentValidation;
using MusicSchoolManagement.Core.DTOs.Students;

namespace MusicSchoolManagement.API.Validators;

public class CreateStudentDtoValidator : AbstractValidator<CreateStudentDto>
{
    public CreateStudentDtoValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required")
            .MaximumLength(100).WithMessage("First name cannot exceed 100 characters");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required")
            .MaximumLength(100).WithMessage("Last name cannot exceed 100 characters");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email format")
            .MaximumLength(255).WithMessage("Email cannot exceed 255 characters");

        RuleFor(x => x.PhoneNumber)
            .Matches(@"^\+?[1-9]\d{1,14}$").WithMessage("Invalid phone number format")
            .When(x => !string.IsNullOrEmpty(x.PhoneNumber));

        RuleFor(x => x.DateOfBirth)
            .NotEmpty().WithMessage("Date of birth is required")
            .LessThan(DateTime.Today).WithMessage("Date of birth must be in the past")
            .GreaterThan(DateTime.Today.AddYears(-100)).WithMessage("Invalid date of birth");

        RuleFor(x => x.ParentName)
            .NotEmpty().WithMessage("Parent name is required")
            .MaximumLength(200).WithMessage("Parent name cannot exceed 200 characters");

        RuleFor(x => x.ParentPhone)
            .NotEmpty().WithMessage("Parent phone is required")
            .Matches(@"^\+?[1-9]\d{1,14}$").WithMessage("Invalid phone number format");

        RuleFor(x => x.ParentEmail)
            .EmailAddress().WithMessage("Invalid parent email format")
            .When(x => !string.IsNullOrEmpty(x.ParentEmail));

        RuleFor(x => x.Address)
            .MaximumLength(500).WithMessage("Address cannot exceed 500 characters")
            .When(x => !string.IsNullOrEmpty(x.Address));

        RuleFor(x => x.EmergencyContact)
            .Matches(@"^\+?[1-9]\d{1,14}$").WithMessage("Invalid emergency contact format")
            .When(x => !string.IsNullOrEmpty(x.EmergencyContact));

        RuleFor(x => x.RegistrationDate)
            .NotEmpty().WithMessage("Registration date is required")
            .LessThanOrEqualTo(DateTime.Today).WithMessage("Registration date cannot be in the future");
    }
}