using MusicSchoolManagement.Core.Entities;

namespace MusicSchoolManagement.Core.Interfaces.Repositories;

public interface IAttendanceLogRepository : IGenericRepository<AttendanceLog>
{
    Task<IEnumerable<AttendanceLog>> GetByAppointmentIdAsync(int appointmentId);
    Task<IEnumerable<AttendanceLog>> GetByStudentIdAsync(int studentId);
    Task<AttendanceLog?> GetByAppointmentAndStudentAsync(int appointmentId, int studentId);
}