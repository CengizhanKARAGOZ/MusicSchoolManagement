using MusicSchoolManagement.Core.Enitties;

namespace MusicSchoolManagement.Core.Entities;

public class Teacher : BaseEntity
{
    public int UserId { get; set; }
    public string? Specializations { get; set; }
    public decimal? HourlyRate { get; set; }
    public string? Biography { get; set; }
    public string? AvailabilityNotes { get; set; }
    
    // Navigation properties
    public User User { get; set; } = null!;
    public ICollection<TeacherAvailability> Availabilities { get; set; } = new List<TeacherAvailability>();
    public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
}