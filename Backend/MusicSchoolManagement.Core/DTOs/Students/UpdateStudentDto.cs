namespace MusicSchoolManagement.Core.DTOs.Students;

public class UpdateStudentDto
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? ParentName { get; set; }
    public string? ParentPhone { get; set; }
    public string? ParentEmail { get; set; }
    public string? Address { get; set; }
    public string? EmergencyContact { get; set; }
    public string? Notes { get; set; }
    public bool IsActive { get; set; }
}