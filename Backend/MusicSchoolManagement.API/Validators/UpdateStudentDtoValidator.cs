using FluentValidation;
using MusicSchoolManagement.Core.DTOs.Students;
using System.Text.RegularExpressions;

namespace MusicSchoolManagement.API.Validators;

public class UpdateStudentDtoValidator : AbstractValidator<UpdateStudentDto>
{
    public UpdateStudentDtoValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required")
            .MaximumLength(100).WithMessage("First name cannot exceed 100 characters");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required")
            .MaximumLength(100).WithMessage("Last name cannot exceed 100 characters");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email format");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Phone number is required")
            .Must(phone => BeValidPhoneNumber(phone))
            .WithMessage("Invalid phone number format. Use: +905381234567, 05381234567, or (538) 123 45 67");

        RuleFor(x => x.ParentPhone)
            .Must(phone => BeValidPhoneNumberOrEmpty(phone))
            .WithMessage("Invalid parent phone number format")
            .When(x => !string.IsNullOrEmpty(x.ParentPhone));

        RuleFor(x => x.DateOfBirth)
            .NotEmpty().WithMessage("Date of birth is required")
            .Must(date => date.HasValue && BeValidDate(date.Value))
            .WithMessage("Invalid date of birth")
            .Must(date => date.HasValue && BeInThePast(date.Value))
            .WithMessage("Date of birth must be in the past")
            .Must(date => date.HasValue && NotBeTooOld(date.Value))
            .WithMessage("Date of birth cannot be more than 100 years ago");

        RuleFor(x => x.ParentName)
            .MaximumLength(200).WithMessage("Parent name cannot exceed 200 characters");

        RuleFor(x => x.Address)
            .MaximumLength(500).WithMessage("Address cannot exceed 500 characters");
    }

    private static bool BeValidPhoneNumber(string phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(phoneNumber))
            return false;

        var digitsOnly = Regex.Replace(phoneNumber, @"[^\d]", "");

        if (digitsOnly.Length == 13 && digitsOnly.StartsWith("90"))
            return true;
        
        if (digitsOnly.Length == 12 && digitsOnly.StartsWith("90"))
            return true;

        if (digitsOnly.Length == 11 && digitsOnly.StartsWith("0"))
            return true;

        if (digitsOnly.Length == 10)
            return true;

        return false;
    }

    private static bool BeValidPhoneNumberOrEmpty(string? phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(phoneNumber))
            return true;

        return BeValidPhoneNumber(phoneNumber);
    }

    private static bool BeValidDate(DateTime date)
    {
        return date != default(DateTime);
    }

    private static bool BeInThePast(DateTime date)
    {
        return date < DateTime.Now;
    }

    private static bool NotBeTooOld(DateTime date)
    {
        return date > DateTime.Now.AddYears(-100);
    }
}