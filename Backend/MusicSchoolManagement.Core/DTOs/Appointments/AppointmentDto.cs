using MusicSchoolManagement.Core.Enums;

namespace MusicSchoolManagement.Core.DTOs.Appointments;

public class AppointmentDto
{
    public int Id { get; set; }
    public int StudentId { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public int TeacherId { get; set; }
    public string TeacherName { get; set; } = string.Empty;
    public int CourseId { get; set; }
    public string CourseName { get; set; } = string.Empty;
    public int? ClassroomId { get; set; }
    public string? ClassroomName { get; set; }
    public DateTime AppointmentDate { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public AppointmentStatus Status { get; set; }
    public bool IsRecurring { get; set; }
    public string? RecurringPattern { get; set; }
    public DateTime? RecurringEndDate { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
}