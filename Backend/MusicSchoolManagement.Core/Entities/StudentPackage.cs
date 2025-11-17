using MusicSchoolManagement.Core.Enums;

namespace MusicSchoolManagement.Core.Entities;

public class StudentPackage : BaseEntity
{
    public int StudentId { get; set; }
    public int PackageId { get; set; }
    public int CourseId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int TotalLessons { get; set; }
    public int UsedLessons { get; set; } = 0;
    public int RemainingLessons { get; set; }
    public StudentPackageStatus Status { get; set; } = StudentPackageStatus.Active;
    public DateTime PurchaseDate { get; set; } = DateTime.UtcNow;
    
    // Navigation properties
    public Student Student { get; set; } = null!;
    public Package Package { get; set; } = null!;
    public Course Course { get; set; } = null!;
    public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    public ICollection<Payment> Payments { get; set; } = new List<Payment>();
}