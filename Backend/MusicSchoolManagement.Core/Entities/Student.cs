

using MusicSchoolManagement.Core.Entities;

namespace MusicSchoolManagement.Core.Enitties;

public class Student : BaseEntity
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
    public DateTime RegistrationDate { get; set; }
    public bool IsActive { get; set; } = true;
    
    // Navigation properties
    public ICollection<StudentPackage> StudentPackages { get; set; } = new List<StudentPackage>();
    public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    public ICollection<Payment> Payments { get; set; } = new List<Payment>();
    public ICollection<AttendanceLog> AttendanceLogs { get; set; } = new List<AttendanceLog>();
}