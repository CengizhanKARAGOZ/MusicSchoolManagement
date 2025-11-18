namespace MusicSchoolManagement.Core.DTOs.Appointments;

public class CreateAppointmentDto
{
    public int StudentId { get; set; }
    public int TeacherId { get; set; }
    public int CourseId { get; set; }
    public int? ClassroomId { get; set; }
    public int? StudentPackageId { get; set; }
    public DateTime AppointmentDate { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public string? Notes { get; set; }
}