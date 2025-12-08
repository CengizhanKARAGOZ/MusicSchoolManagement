namespace MusicSchoolManagement.Core.DTOs.Teachers;

public class CreateTeacherWithUserDto
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string? Specializations { get; set; }
    public decimal? HourlyRate { get; set; }
    public string? Biography { get; set; }
    public string? AvailabilityNotes { get; set; }
}