using MusicSchoolManagement.Core.Entities;

namespace MusicSchoolManagement.Core.Interfaces.Repositories;

public interface IPaymentRepository : IGenericRepository<Payment>
{
    Task<IEnumerable<Payment>> GetByStudentIdAsync(int studentId);
    Task<IEnumerable<Payment>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<IEnumerable<Payment>> GetPendingPaymentsAsync();
    Task<decimal> GetTotalRevenueAsync(DateTime startDate, DateTime endDate);
}