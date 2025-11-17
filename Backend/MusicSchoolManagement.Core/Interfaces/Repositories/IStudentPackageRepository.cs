using MusicSchoolManagement.Core.Entities;

namespace MusicSchoolManagement.Core.Interfaces.Repositories;

public interface IStudentPackageRepository : IGenericRepository<StudentPackage>
{
    Task<IEnumerable<StudentPackage>> GetByStudentIdAsync(int studentId);
    Task<StudentPackage> GetActivePackageAsync(int studentId, int courseId);
    Task<IEnumerable<StudentPackage>> GetExpiringPackagesAsync(int daysUntilExpiry);
}