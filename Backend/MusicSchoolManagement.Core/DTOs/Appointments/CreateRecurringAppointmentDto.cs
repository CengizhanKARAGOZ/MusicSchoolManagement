namespace MusicSchoolManagement.Core.DTOs.Appointments;

public class CreateRecurringAppointmentDto
{
    public int StudentId { get; set; }
    public int TeacherId { get; set; }
    public int CourseId { get; set; }
    public int? ClassroomId { get; set; }
    public int? StudentPackageId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public string RecurringPattern { get; set; } = "Weekly"; // Weekly, Biweekly
    public string? Notes { get; set; }
}