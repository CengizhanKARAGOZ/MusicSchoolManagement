using Microsoft.EntityFrameworkCore;
using MusicSchoolManagement.Core.Entities;
using MusicSchoolManagement.Core.Enums;
using MusicSchoolManagement.Core.Interfaces.Repositories;
using MusicSchoolManagement.Infrastructure.Data;

namespace MusicSchoolManagement.Infrastructure.Repositories;

public class AppointmentRepository : GenericRepository<Appointment>, IAppointmentRepository
{
    public AppointmentRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Appointment>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        return await _dbSet
            .Where(a => a.AppointmentDate >= startDate && a.AppointmentDate <= endDate)
            .Include(a => a.Student)
            .Include(a => a.Teacher).ThenInclude(t => t.User)
            .Include(a => a.Course)
            .Include(a => a.Classroom)
            .OrderBy(a => a.AppointmentDate)
            .ThenBy(a => a.StartTime)
            .ToListAsync();
    }

    public async Task<IEnumerable<Appointment>> GetByTeacherIdAsync(int teacherId, DateTime? date = null)
    {
        IQueryable<Appointment> query = _dbSet.Where(a => a.TeacherId == teacherId);
        
        if (date.HasValue)
            query = query.Where(a => a.AppointmentDate.Date == date.Value.Date);

        return await query
            .Include(a => a.Student)
            .Include(a => a.Course)
            .Include(a => a.Classroom)
            .OrderBy(a => a.AppointmentDate)
            .ThenBy(a => a.StartTime)
            .ToListAsync();
    }

    public async Task<IEnumerable<Appointment>> GetByStudentIdAsync(int studentId, DateTime? date = null)
    {
        IQueryable<Appointment> query = _dbSet.Where(a => a.StudentId == studentId);
        
        if (date.HasValue)
            query = query.Where(a => a.AppointmentDate.Date == date.Value.Date);

        return await query
            .Include(a => a.Teacher).ThenInclude(t => t.User)
            .Include(a => a.Course)
            .Include(a => a.Classroom)
            .OrderBy(a => a.AppointmentDate)
            .ThenBy(a => a.StartTime)
            .ToListAsync();
    }

    public async Task<IEnumerable<Appointment>> GetByClassroomIdAsync(int classroomId, DateTime date)
    {
        return await _dbSet
            .Where(a => a.ClassroomId == classroomId && a.AppointmentDate.Date == date.Date)
            .OrderBy(a => a.StartTime)
            .ToListAsync();
    }

    public async Task<bool> HasConflictAsync(int teacherId, int classroomId, int studentId, DateTime date, TimeSpan startTime, TimeSpan endTime, int? excludeAppointmentId = null)
    {
        IQueryable<Appointment> query = _dbSet.Where(a => 
            a.AppointmentDate.Date == date.Date &&
            a.Status != AppointmentStatus.Cancelled &&
            a.StartTime < endTime && a.EndTime > startTime);

        if (excludeAppointmentId.HasValue)
            query = query.Where(a => a.Id != excludeAppointmentId.Value);

        // Check teacher conflict
        bool teacherConflict = await query.AnyAsync(a => a.TeacherId == teacherId);
        if (teacherConflict) return true;

        // Check classroom conflict
        bool classroomConflict = await query.AnyAsync(a => a.ClassroomId == classroomId);
        if (classroomConflict) return true;

        // Check student conflict
        bool studentConflict = await query.AnyAsync(a => a.StudentId == studentId);
        if (studentConflict) return true;

        return false;
    }

    public async Task<Appointment?> GetWithDetailsAsync(int id)
    {
        return await _dbSet
            .Include(a => a.Student)
            .Include(a => a.Teacher).ThenInclude(t => t.User)
            .Include(a => a.Course).ThenInclude(c => c.Instrument)
            .Include(a => a.Classroom)
            .Include(a => a.StudentPackage)
            .Include(a => a.AttendanceLogs)
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<IEnumerable<Appointment>> GetUpcomingAppointmentsAsync(int count)
    {
        return await _dbSet
            .Where(a => a.AppointmentDate >= DateTime.UtcNow.Date && 
                        a.Status == AppointmentStatus.Scheduled)
            .Include(a => a.Student)
            .Include(a => a.Teacher).ThenInclude(t => t.User)
            .Include(a => a.Course)
            .OrderBy(a => a.AppointmentDate)
            .ThenBy(a => a.StartTime)
            .Take(count)
            .ToListAsync();
    }
}