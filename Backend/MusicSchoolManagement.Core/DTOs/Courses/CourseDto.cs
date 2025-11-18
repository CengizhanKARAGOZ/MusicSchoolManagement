using MusicSchoolManagement.Core.Enums;

namespace MusicSchoolManagement.Core.DTOs.Courses;

public class CourseDto
{
    public int Id { get; set; }
    public int InstrumentId { get; set; }
    public string InstrumentName { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public CourseLevel Level { get; set; }
    public CourseType Type { get; set; }
    public int Duration { get; set; }
    public decimal BasePrice { get; set; }
    public int MaxStudents { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; }
}