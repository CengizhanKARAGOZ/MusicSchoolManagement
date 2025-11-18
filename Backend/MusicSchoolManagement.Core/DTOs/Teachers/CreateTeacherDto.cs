namespace MusicSchoolManagement.Core.DTOs.Teachers;

public class CreateTeacherDto
{
    public int UserId { get; set; }
    public string? Specializations { get; set; }
    public decimal? HourlyRate { get; set; }
    public string? Biography { get; set; }
    public string? AvailabilityNotes { get; set; }
}