using MusicSchoolManagement.Core.Enums;

namespace MusicSchoolManagement.Core.Entities;

public class Payment : BaseEntity
{
    public int StudentId { get; set; }
    public int? StudentPackageId { get; set; }
    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; } = DateTime.UtcNow;
    public PaymentMethod PaymentMethod { get; set; }
    public PaymentStatus Status { get; set; } = PaymentStatus.Completed;
    public string? TransactionReference { get; set; }
    public string? Notes { get; set; }
    public int CreatedBy { get; set; }
    
    // Navigation properties
    public Student Student { get; set; } = null!;
    public StudentPackage? StudentPackage { get; set; }
    public User CreatedByUser { get; set; } = null!;
}