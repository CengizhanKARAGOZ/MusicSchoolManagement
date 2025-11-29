using MusicSchoolManagement.Core.DTOs.Payments;

namespace MusicSchoolManagement.Core.Interfaces.Services;

public interface IPaymentService
{
    Task<IEnumerable<PaymentDto>> GetAllPaymentsAsync();
    Task<IEnumerable<PaymentDto>> GetPaymentsByStudentAsync(int studentId);
    Task<IEnumerable<PaymentDto>> GetPaymentsByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<IEnumerable<PaymentDto>> GetPendingPaymentsAsync();
    Task<PaymentDto?> GetPaymentByIdAsync(int id);
    Task<PaymentDto> CreatePaymentAsync(CreatePaymentDto createDto, int createdByUserId);
    Task<decimal> GetTotalRevenueAsync(DateTime startDate, DateTime endDate);
    Task<bool> RefundPaymentAsync(int id);
}