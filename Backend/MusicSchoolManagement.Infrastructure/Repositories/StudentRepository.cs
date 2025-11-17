using Microsoft.EntityFrameworkCore;
using MusicSchoolManagement.Core.Enitties;
using MusicSchoolManagement.Core.Interfaces.Repositories;
using MusicSchoolManagement.Infrastructure.Data;

namespace MusicSchoolManagement.Infrastructure.Repositories;

public class StudentRepository : GenericRepository<Student>, IStudentRepository
{
    public StudentRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Student?> GetWithPackagesAsync(int id)
    {
        return await _dbSet
            .Include(s => s.StudentPackages)
            .ThenInclude(sp => sp.Package)
            .Include(s => s.StudentPackages)
            .ThenInclude(sp => sp.Course)
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<Student?> GetWithAppointmentsAsync(int id)
    {
        return await _dbSet
            .Include(s => s.Appointments)
            .ThenInclude(a => a.Teacher)
            .ThenInclude(t => t.User)
            .Include(s => s.Appointments)
            .ThenInclude(a => a.Course)
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<IEnumerable<Student>> GetActiveStudentsAsync()
    {
        return await _dbSet.Where(s => s.IsActive).ToListAsync();
    }
}