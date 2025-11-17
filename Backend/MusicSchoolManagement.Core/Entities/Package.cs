using MusicSchoolManagement.Core.Enitties;

namespace MusicSchoolManagement.Core.Entities;

public class Package : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public int DurationMonths { get; set; }
    public int LessonsPerMonth { get; set; }
    public int TotalLessons { get; set; }
    public decimal Price { get; set; }
    public decimal DiscountPercentage { get; set; } = 0;
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;
    
    // Navigation properties
    public ICollection<StudentPackage> StudentPackages { get; set; } = new List<StudentPackage>();
}