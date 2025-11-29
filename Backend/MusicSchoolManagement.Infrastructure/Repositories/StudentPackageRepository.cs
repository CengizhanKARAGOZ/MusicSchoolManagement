using Microsoft.EntityFrameworkCore;
using MusicSchoolManagement.Core.Entities;
using MusicSchoolManagement.Core.Enums;
using MusicSchoolManagement.Core.Interfaces.Repositories;
using MusicSchoolManagement.Infrastructure.Data;

namespace MusicSchoolManagement.Infrastructure.Repositories;

public class StudentPackageRepository : GenericRepository<StudentPackage>, IStudentPackageRepository
{
    public StudentPackageRepository(ApplicationDbContext context) : base(context)
    {
    }

    // Override GetByIdAsync to include navigation properties
    public override async Task<StudentPackage?> GetByIdAsync(int id)
    {
        return await _dbSet
            .Include(sp => sp.Student)
            .Include(sp => sp.Package)
            .Include(sp => sp.Course)
            .FirstOrDefaultAsync(sp => sp.Id == id);
    }

    public async Task<IEnumerable<StudentPackage>> GetByStudentIdAsync(int studentId)
    {
        return await _dbSet
            .Where(sp => sp.StudentId == studentId)
            .Include(sp => sp.Student)
            .Include(sp => sp.Package)
            .Include(sp => sp.Course)
            .ToListAsync();
    }

    public async Task<StudentPackage?> GetActivePackageAsync(int studentId, int courseId)
    {
        return await _dbSet
            .Where(sp => sp.StudentId == studentId && 
                         sp.CourseId == courseId && 
                         sp.Status == StudentPackageStatus.Active)
            .Include(sp => sp.Package)
            .Include(sp => sp.Course)
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<StudentPackage>> GetExpiringPackagesAsync(int daysUntilExpiry)
    {
        var targetDate = DateTime.UtcNow.AddDays(daysUntilExpiry);
        return await _dbSet
            .Where(sp => sp.Status == StudentPackageStatus.Active && 
                         sp.EndDate <= targetDate)
            .Include(sp => sp.Student)
            .Include(sp => sp.Package)
            .ToListAsync();
    }
}