using MusicSchoolManagement.Core.Enums;

namespace MusicSchoolManagement.Core.Entities;

public class Course : BaseEntity
{
    public int InstrumentId { get; set; }
    public string Name { get; set; } = string.Empty;
    public CourseLevel Level { get; set; }
    public CourseType Type { get; set; }
    public int Duration { get; set; }
    public decimal BasePrice { get; set; }
    public int MaxStudents { get; set; } = 1;
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;
    
    // Navigation properties
    public Instrument Instrument { get; set; } = null!;
    public ICollection<StudentPackage> StudentPackages { get; set; } = new List<StudentPackage>();
    public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
}