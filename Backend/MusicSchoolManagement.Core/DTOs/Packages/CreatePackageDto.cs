namespace MusicSchoolManagement.Core.DTOs.Packages;

public class CreatePackageDto
{
    public string Name { get; set; } = string.Empty;
    public int DurationMonths { get; set; }
    public int LessonsPerMonth { get; set; }
    public decimal Price { get; set; }
    public decimal DiscountPercentage { get; set; } = 0;
    public string? Description { get; set; }
}