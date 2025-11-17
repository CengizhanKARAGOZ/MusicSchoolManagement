using Microsoft.EntityFrameworkCore;
using MusicSchoolManagement.Core.Entities;
using MusicSchoolManagement.Core.Enums;
using MusicSchoolManagement.Core.Interfaces.Repositories;
using MusicSchoolManagement.Infrastructure.Data;

namespace MusicSchoolManagement.Infrastructure.Repositories;

public class PaymentRepository : GenericRepository<Payment>, IPaymentRepository
{
    public PaymentRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Payment>> GetByStudentIdAsync(int studentId)
    {
        return await _dbSet
            .Where(p => p.StudentId == studentId)
            .Include(p => p.StudentPackage)
            .OrderByDescending(p => p.PaymentDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Payment>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        return await _dbSet
            .Where(p => p.PaymentDate >= startDate && p.PaymentDate <= endDate)
            .Include(p => p.Student)
            .OrderByDescending(p => p.PaymentDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Payment>> GetPendingPaymentsAsync()
    {
        return await _dbSet
            .Where(p => p.Status == PaymentStatus.Pending)
            .Include(p => p.Student)
            .OrderBy(p => p.PaymentDate)
            .ToListAsync();
    }

    public async Task<decimal> GetTotalRevenueAsync(DateTime startDate, DateTime endDate)
    {
        return await _dbSet
            .Where(p => p.PaymentDate >= startDate && 
                        p.PaymentDate <= endDate && 
                        p.Status == PaymentStatus.Completed)
            .SumAsync(p => p.Amount);
    }
}