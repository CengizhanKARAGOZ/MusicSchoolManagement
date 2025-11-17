using MusicSchoolManagement.Core.Enitties;
using MusicSchoolManagement.Core.Enums;

namespace MusicSchoolManagement.Core.Entities;

public class AttendanceLog : BaseEntity
{
    public int AppointmentId { get; set; }
    public int StudentId { get; set; }
    public AttendanceStatus Status { get; set; }
    public DateTime? CheckInTime { get; set; }
    public string? Notes { get; set; }
    public int RecordedBy { get; set; }
    public DateTime RecordedAt { get; set; } = DateTime.UtcNow;
    
    // Navigation properties
    public Appointment Appointment { get; set; } = null!;
    public Student Student { get; set; } = null!;
    public User RecordedByUser { get; set; } = null!;
}