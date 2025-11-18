using MusicSchoolManagement.Core.Enums;

namespace MusicSchoolManagement.Core.DTOs.Courses;

public class UpdateCourseDto
{
    public string Name { get; set; } = string.Empty;
    public CourseLevel Level { get; set; }
    public CourseType Type { get; set; }
    public int Duration { get; set; }
    public decimal BasePrice { get; set; }
    public int MaxStudents { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; }
}