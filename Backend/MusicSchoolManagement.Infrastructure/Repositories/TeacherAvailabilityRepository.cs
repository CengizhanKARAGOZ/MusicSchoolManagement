using Microsoft.EntityFrameworkCore;
using MusicSchoolManagement.Core.Entities;
using MusicSchoolManagement.Core.Enums;
using MusicSchoolManagement.Core.Interfaces.Repositories;
using MusicSchoolManagement.Infrastructure.Data;

namespace MusicSchoolManagement.Infrastructure.Repositories;

public class TeacherAvailabilityRepository : GenericRepository<TeacherAvailability>, ITeacherAvailabilityRepository
{
    public TeacherAvailabilityRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<TeacherAvailability>> GetByTeacherIdAsync(int teacherId)
    {
        return await _dbSet
            .Where(ta => ta.TeacherId == teacherId)
            .OrderBy(ta => ta.DayOfWeek)
            .ThenBy(ta => ta.StartTime)
            .ToListAsync();
    }

    public async Task<TeacherAvailability?> GetByTeacherAndDayAsync(int teacherId, WeekDay dayOfWeek)
    {
        return await _dbSet
            .FirstOrDefaultAsync(ta => ta.TeacherId == teacherId && ta.DayOfWeek == dayOfWeek);
    }
}