using MusicSchoolManagement.Core.Enums;

namespace MusicSchoolManagement.Core.DTOs.Payments;

public class PaymentDto
{
    public int Id { get; set; }
    public int StudentId { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public int? StudentPackageId { get; set; }
    public string? PackageName { get; set; }
    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public PaymentStatus Status { get; set; }
    public string? TransactionReference { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
}