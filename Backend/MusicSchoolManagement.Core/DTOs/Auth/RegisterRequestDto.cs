using MusicSchoolManagement.Core.Enums;

namespace MusicSchoolManagement.Core.DTOs.Auth;

public class RegisterRequestDto
{
    public string FirstName { get; set; } = string.Empty;
    public string Lastname { get; set; } = String.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public UserRole Role { get; set; }
}