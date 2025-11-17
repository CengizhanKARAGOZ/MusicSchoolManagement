using MusicSchoolManagement.Core.Enitties;
using MusicSchoolManagement.Core.Enums;

namespace MusicSchoolManagement.Core.Entities;

public class TeacherAvailability : BaseEntity
{
    public int TeacherId { get; set; }
    public WeekDay DayOfWeek { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public bool IsAvailable { get; set; } = true;
    
    // Navigation property
    public Teacher Teacher { get; set; } = null!;
}