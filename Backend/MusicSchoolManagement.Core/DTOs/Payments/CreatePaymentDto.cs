using MusicSchoolManagement.Core.Enums;

namespace MusicSchoolManagement.Core.DTOs.Payments;

public class CreatePaymentDto
{
    public int StudentId { get; set; }
    public int? StudentPackageId { get; set; }
    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; } = DateTime.UtcNow;
    public PaymentMethod PaymentMethod { get; set; }
    public PaymentStatus Status { get; set; } = PaymentStatus.Completed;
    public string? TransactionReference { get; set; }
    public string? Notes { get; set; }
}