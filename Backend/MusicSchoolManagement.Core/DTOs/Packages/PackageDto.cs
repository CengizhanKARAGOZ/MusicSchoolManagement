namespace MusicSchoolManagement.Core.DTOs.Packages;

public class PackageDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int DurationMonths { get; set; }
    public int LessonsPerMonth { get; set; }
    public int TotalLessons { get; set; }
    public decimal Price { get; set; }
    public decimal DiscountPercentage { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; }
}