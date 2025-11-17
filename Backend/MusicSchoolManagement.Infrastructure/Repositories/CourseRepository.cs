using Microsoft.EntityFrameworkCore;
using MusicSchoolManagement.Core.Entities;
using MusicSchoolManagement.Core.Interfaces.Repositories;
using MusicSchoolManagement.Infrastructure.Data;

namespace MusicSchoolManagement.Infrastructure.Repositories;

public class CourseRepository : GenericRepository<Course>, ICourseRepository
{
    public CourseRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Course>> GetByInstrumentIdAsync(int instrumentId)
    {
        return await _dbSet
            .Where(c => c.InstrumentId == instrumentId)
            .Include(c => c.Instrument)
            .ToListAsync();
    }

    public async Task<IEnumerable<Course>> GetActiveCoursesAsync()
    {
        return await _dbSet
            .Where(c => c.IsActive)
            .Include(c => c.Instrument)
            .ToListAsync();
    }

    public async Task<Course?> GetWithInstrumentAsync(int id)
    {
        return await _dbSet
            .Include(c => c.Instrument)
            .FirstOrDefaultAsync(c => c.Id == id);
    }
}