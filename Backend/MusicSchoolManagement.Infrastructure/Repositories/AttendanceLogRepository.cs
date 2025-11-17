using Microsoft.EntityFrameworkCore;
using MusicSchoolManagement.Core.Entities;
using MusicSchoolManagement.Core.Interfaces.Repositories;
using MusicSchoolManagement.Infrastructure.Data;

namespace MusicSchoolManagement.Infrastructure.Repositories;

public class AttendanceLogRepository : GenericRepository<AttendanceLog>, IAttendanceLogRepository
{
    public AttendanceLogRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<AttendanceLog>> GetByAppointmentIdAsync(int appointmentId)
    {
        return await _dbSet
            .Where(al => al.AppointmentId == appointmentId)
            .Include(al => al.Student)
            .ToListAsync();
    }

    public async Task<IEnumerable<AttendanceLog>> GetByStudentIdAsync(int studentId)
    {
        return await _dbSet
            .Where(al => al.StudentId == studentId)
            .Include(al => al.Appointment)
            .OrderByDescending(al => al.RecordedAt)
            .ToListAsync();
    }

    public async Task<AttendanceLog?> GetByAppointmentAndStudentAsync(int appointmentId, int studentId)
    {
        return await _dbSet
            .FirstOrDefaultAsync(al => al.AppointmentId == appointmentId && al.StudentId == studentId);
    }
}