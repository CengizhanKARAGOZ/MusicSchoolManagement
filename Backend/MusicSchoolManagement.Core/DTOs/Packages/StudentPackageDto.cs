using MusicSchoolManagement.Core.Enums;

namespace MusicSchoolManagement.Core.DTOs.Packages;

public class StudentPackageDto
{
    public int Id { get; set; }
    public int StudentId { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public int PackageId { get; set; }
    public string PackageName { get; set; } = string.Empty;
    public int CourseId { get; set; }
    public string CourseName { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int TotalLessons { get; set; }
    public int UsedLessons { get; set; }
    public int RemainingLessons { get; set; }
    public StudentPackageStatus Status { get; set; }
    public DateTime PurchaseDate { get; set; }
}