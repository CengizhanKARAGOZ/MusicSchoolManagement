using FluentValidation;
using MusicSchoolManagement.Core.DTOs.Appointments;

namespace MusicSchoolManagement.API.Validators;

public class CreateAppointmentDtoValidator : AbstractValidator<CreateAppointmentDto>
{
    public CreateAppointmentDtoValidator()
    {
        RuleFor(x => x.StudentId)
            .GreaterThan(0).WithMessage("Student ID is required");

        RuleFor(x => x.TeacherId)
            .GreaterThan(0).WithMessage("Teacher ID is required");

        RuleFor(x => x.CourseId)
            .GreaterThan(0).WithMessage("Course ID is required");

        RuleFor(x => x.AppointmentDate)
            .NotEmpty().WithMessage("Appointment date is required")
            .GreaterThanOrEqualTo(DateTime.Today).WithMessage("Appointment date cannot be in the past");

        RuleFor(x => x.StartTime)
            .NotEmpty().WithMessage("Start time is required");

        RuleFor(x => x.EndTime)
            .NotEmpty().WithMessage("End time is required")
            .GreaterThan(x => x.StartTime).WithMessage("End time must be after start time");

        RuleFor(x => x)
            .Must(x => (x.EndTime - x.StartTime).TotalMinutes >= 30)
            .WithMessage("Appointment must be at least 30 minutes long");

        RuleFor(x => x)
            .Must(x => (x.EndTime - x.StartTime).TotalHours <= 4)
            .WithMessage("Appointment cannot exceed 4 hours");
    }
}