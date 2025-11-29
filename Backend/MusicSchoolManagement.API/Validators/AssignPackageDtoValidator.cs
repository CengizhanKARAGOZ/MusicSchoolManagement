using FluentValidation;
using MusicSchoolManagement.Core.DTOs.Packages;

namespace MusicSchoolManagement.API.Validators;

public class AssignPackageDtoValidator : AbstractValidator<AssignPackageDto>
{
    public AssignPackageDtoValidator()
    {
        RuleFor(x => x.StudentId)
            .GreaterThan(0).WithMessage("Student ID is required");

        RuleFor(x => x.PackageId)
            .GreaterThan(0).WithMessage("Package ID is required");

        RuleFor(x => x.CourseId)
            .GreaterThan(0).WithMessage("Course ID is required");

        RuleFor(x => x.StartDate)
            .NotEmpty().WithMessage("Start date is required")
            .GreaterThanOrEqualTo(DateTime.Today.AddDays(-7))
            .WithMessage("Start date cannot be more than 7 days in the past");
    }
}