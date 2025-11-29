using FluentValidation;
using MusicSchoolManagement.Core.DTOs.Appointments;

namespace MusicSchoolManagement.API.Validators;

public class CreateRecurringAppointmentDtoValidator : AbstractValidator<CreateRecurringAppointmentDto>
{
    public CreateRecurringAppointmentDtoValidator()
    {
        RuleFor(x => x.StudentId)
            .GreaterThan(0).WithMessage("Student ID is required");

        RuleFor(x => x.TeacherId)
            .GreaterThan(0).WithMessage("Teacher ID is required");

        RuleFor(x => x.CourseId)
            .GreaterThan(0).WithMessage("Course ID is required");

        RuleFor(x => x.StartDate)
            .NotEmpty().WithMessage("Start date is required")
            .GreaterThanOrEqualTo(DateTime.Today).WithMessage("Start date cannot be in the past");

        RuleFor(x => x.EndDate)
            .NotEmpty().WithMessage("End date is required")
            .GreaterThan(x => x.StartDate).WithMessage("End date must be after start date");

        RuleFor(x => x)
            .Must(x => (x.EndDate - x.StartDate).TotalDays <= 365)
            .WithMessage("Recurring appointments cannot span more than 1 year");

        RuleFor(x => x.StartTime)
            .NotEmpty().WithMessage("Start time is required");

        RuleFor(x => x.EndTime)
            .NotEmpty().WithMessage("End time is required")
            .GreaterThan(x => x.StartTime).WithMessage("End time must be after start time");

        RuleFor(x => x.RecurringPattern)
            .NotEmpty().WithMessage("Recurring pattern is required")
            .Must(x => x == "Weekly" || x == "Biweekly")
            .WithMessage("Recurring pattern must be 'Weekly' or 'Biweekly'");
    }
}