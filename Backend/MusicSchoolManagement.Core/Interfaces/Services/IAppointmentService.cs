using MusicSchoolManagement.Core.DTOs.Appointments;

namespace MusicSchoolManagement.Core.Interfaces.Services;

public interface IAppointmentService
{
    Task<IEnumerable<AppointmentDto>> GetAllAppointmentsAsync();
    Task<IEnumerable<AppointmentDto>> GetAppointmentsByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<IEnumerable<AppointmentDto>> GetAppointmentsByTeacherAsync(int teacherId, DateTime? date = null);
    Task<IEnumerable<AppointmentDto>> GetAppointmentsByStudentAsync(int studentId, DateTime? date = null);
    Task<IEnumerable<AppointmentDto>> GetUpcomingAppointmentsAsync(int count);
    Task<AppointmentDto?> GetAppointmentByIdAsync(int id);
    Task<AppointmentDto> CreateAppointmentAsync(CreateAppointmentDto createDto, int createdByUserId);
    Task<IEnumerable<AppointmentDto>> CreateRecurringAppointmentsAsync(CreateRecurringAppointmentDto createDto, int createdByUserId);
    Task<AppointmentDto?> UpdateAppointmentAsync(int id, UpdateAppointmentDto updateDto);
    Task<bool> CancelAppointmentAsync(int id, string reason);
    Task<bool> DeleteAppointmentAsync(int id);
}