using Microsoft.EntityFrameworkCore;
using MusicSchoolManagement.Core.Enitties;
using MusicSchoolManagement.Core.Interfaces.Repositories;
using MusicSchoolManagement.Infrastructure.Data;

namespace MusicSchoolManagement.Infrastructure.Repositories;

public class TeacherRepository : GenericRepository<Teacher>, ITeacherRepository
{
    public TeacherRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Teacher?> GetByUserIdAsync(int userId)
    {
        return await _dbSet
            .Include(t => t.User)
            .FirstOrDefaultAsync(t => t.UserId == userId);
    }

    public async Task<Teacher?> GetWithAvailabilitiesAsync(int id)
    {
        return await _dbSet
            .Include(t => t.User)
            .Include(t => t.Availabilities)
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<Teacher?> GetWithAppointmentsAsync(int id)
    {
        return await _dbSet
            .Include(t => t.User)
            .Include(t => t.Appointments)
            .ThenInclude(a => a.Student)
            .Include(t => t.Appointments)
            .ThenInclude(a => a.Course)
            .FirstOrDefaultAsync(t => t.Id == id);
    }
}