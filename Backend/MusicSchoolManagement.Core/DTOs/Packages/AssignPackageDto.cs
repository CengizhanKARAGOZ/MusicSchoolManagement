namespace MusicSchoolManagement.Core.DTOs.Packages;

public class AssignPackageDto
{
    public int StudentId { get; set; }
    public int PackageId { get; set; }
    public int CourseId { get; set; }
    public DateTime StartDate { get; set; }
}