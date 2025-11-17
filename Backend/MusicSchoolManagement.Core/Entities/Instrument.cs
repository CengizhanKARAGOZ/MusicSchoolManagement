namespace MusicSchoolManagement.Core.Enitties;

public class Instrument : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;
    
    // Navigation properties
    public ICollection<Course> Courses { get; set; } = new List<Course>();
}