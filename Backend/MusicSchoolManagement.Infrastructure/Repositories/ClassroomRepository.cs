using Microsoft.EntityFrameworkCore;
using MusicSchoolManagement.Core.Entities;
using MusicSchoolManagement.Core.Interfaces.Repositories;
using MusicSchoolManagement.Infrastructure.Data;
using MusicSchoolManagement.Core.Enums;

namespace MusicSchoolManagement.Infrastructure.Repositories;

public class ClassroomRepository : GenericRepository<Classroom>, IClassroomRepository
{
    public ClassroomRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Classroom>> GetActiveClassroomsAsync()
    {
        return await _dbSet.Where(c => c.IsActive).ToListAsync();
    }

    public async Task<IEnumerable<Classroom>> GetAvailableClassroomsAsync(DateTime date, TimeSpan startTime, TimeSpan endTime)
    {
        var bookedClassroomIds = await _context.Appointments
            .Where(a => a.AppointmentDate.Date == date.Date &&
                        a.ClassroomId != null &&
                        a.Status != AppointmentStatus.Cancelled &&
                        ((a.StartTime <= startTime && a.EndTime > startTime) ||
                         (a.StartTime < endTime && a.EndTime >= endTime) ||
                         (a.StartTime >= startTime && a.EndTime <= endTime)))
            .Select(a => a.ClassroomId.Value)
            .Distinct()
            .ToListAsync();

        return await _dbSet
            .Where(c => c.IsActive && !bookedClassroomIds.Contains(c.Id))
            .ToListAsync();
    }
}