using MusicSchoolManagement.Core.DTOs.Users;

namespace MusicSchoolManagement.Core.DTOs.Teachers;

public class TeacherDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public UserDto? User { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public string? Specializations { get; set; }
    public decimal? HourlyRate { get; set; }
    public string? Biography { get; set; }
    public string? AvailabilityNotes { get; set; }
    public DateTime CreatedAt { get; set; }
}