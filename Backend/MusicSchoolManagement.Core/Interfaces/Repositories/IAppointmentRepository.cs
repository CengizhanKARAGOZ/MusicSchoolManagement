using MusicSchoolManagement.Core.Entities;

namespace MusicSchoolManagement.Core.Interfaces.Repositories;

public interface IAppointmentRepository : IGenericRepository<Appointment>
{
    Task<IEnumerable<Appointment>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<IEnumerable<Appointment>> GetByTeacherIdAsync(int teacherId, DateTime? date = null);
    Task<IEnumerable<Appointment>> GetByStudentIdAsync(int studentId, DateTime? date = null);
    Task<IEnumerable<Appointment>> GetByClassroomIdAsync(int classroomId, DateTime date);
    Task<bool> HasConflictAsync(int teacherId, int classroomId, int studentId, DateTime date, TimeSpan startTime, TimeSpan endTime, int? excludeAppointmentId = null);
    Task<Appointment?> GetWithDetailsAsync(int id);
    Task<IEnumerable<Appointment>> GetUpcomingAppointmentsAsync(int count);
}