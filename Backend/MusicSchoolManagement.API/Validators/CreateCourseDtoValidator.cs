using FluentValidation;
using MusicSchoolManagement.Core.DTOs.Courses;

namespace MusicSchoolManagement.API.Validators;

public class CreateCourseDtoValidator : AbstractValidator<CreateCourseDto>
{
    public CreateCourseDtoValidator()
    {
        RuleFor(x => x.InstrumentId)
            .GreaterThan(0).WithMessage("Instrument ID is required");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Course name is required")
            .MaximumLength(200).WithMessage("Course name cannot exceed 200 characters");

        RuleFor(x => x.Level)
            .IsInEnum().WithMessage("Invalid course level");

        RuleFor(x => x.Type)
            .IsInEnum().WithMessage("Invalid course type");

        RuleFor(x => x.Duration)
            .GreaterThan(0).WithMessage("Duration must be greater than zero")
            .LessThanOrEqualTo(480).WithMessage("Duration cannot exceed 480 minutes (8 hours)");

        RuleFor(x => x.BasePrice)
            .GreaterThan(0).WithMessage("Base price must be greater than zero")
            .LessThanOrEqualTo(10000).WithMessage("Base price cannot exceed 10,000");

        RuleFor(x => x.MaxStudents)
            .GreaterThan(0).WithMessage("Max students must be at least 1")
            .LessThanOrEqualTo(50).WithMessage("Max students cannot exceed 50");

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Description cannot exceed 1000 characters")
            .When(x => !string.IsNullOrEmpty(x.Description));
    }
}