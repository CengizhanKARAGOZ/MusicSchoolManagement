using FluentValidation;
using MusicSchoolManagement.Core.DTOs.Packages;

namespace MusicSchoolManagement.API.Validators;

public class CreatePackageDtoValidator : AbstractValidator<CreatePackageDto>
{
    public CreatePackageDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Package name is required")
            .MaximumLength(100).WithMessage("Package name cannot exceed 100 characters");

        RuleFor(x => x.DurationMonths)
            .GreaterThan(0).WithMessage("Duration must be at least 1 month")
            .LessThanOrEqualTo(24).WithMessage("Duration cannot exceed 24 months");

        RuleFor(x => x.LessonsPerMonth)
            .GreaterThan(0).WithMessage("Lessons per month must be at least 1")
            .LessThanOrEqualTo(30).WithMessage("Lessons per month cannot exceed 30");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Price must be greater than zero")
            .LessThanOrEqualTo(100000).WithMessage("Price cannot exceed 100,000");

        RuleFor(x => x.DiscountPercentage)
            .GreaterThanOrEqualTo(0).WithMessage("Discount percentage cannot be negative")
            .LessThanOrEqualTo(100).WithMessage("Discount percentage cannot exceed 100");

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Description cannot exceed 500 characters")
            .When(x => !string.IsNullOrEmpty(x.Description));
    }
}