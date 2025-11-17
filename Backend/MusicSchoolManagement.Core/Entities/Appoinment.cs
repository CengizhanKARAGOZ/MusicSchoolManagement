using MusicSchoolManagement.Core.Enums;

namespace MusicSchoolManagement.Core.Entities;

public class Appointment : BaseEntity
{
    public int StudentId { get; set; }
    public int TeacherId { get; set; }
    public int CourseId { get; set; }
    public int? ClassroomId { get; set; }
    public int? StudentPackageId { get; set; }
    public DateTime AppointmentDate { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public AppointmentStatus Status { get; set; } = AppointmentStatus.Scheduled;
    public bool IsRecurring { get; set; } = false;
    public string? RecurringPattern { get; set; }
    public DateTime? RecurringEndDate { get; set; }
    public int? ParentAppointmentId { get; set; }
    public string? Notes { get; set; }
    public string? CancellationReason { get; set; }
    public int CreatedBy { get; set; }
    
    // Navigation properties
    public Student Student { get; set; } = null!;
    public Teacher Teacher { get; set; } = null!;
    public Course Course { get; set; } = null!;
    public Classroom? Classroom { get; set; }
    public StudentPackage? StudentPackage { get; set; }
    public Appointment? ParentAppointment { get; set; }
    public ICollection<Appointment> ChildAppointments { get; set; } = new List<Appointment>();
    public User CreatedByUser { get; set; } = null!;
    public ICollection<AttendanceLog> AttendanceLogs { get; set; } = new List<AttendanceLog>();
}