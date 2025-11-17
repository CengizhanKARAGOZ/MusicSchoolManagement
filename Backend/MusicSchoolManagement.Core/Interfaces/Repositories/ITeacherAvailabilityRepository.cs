using MusicSchoolManagement.Core.Entities;
using MusicSchoolManagement.Core.Enums;

namespace MusicSchoolManagement.Core.Interfaces.Repositories;

public interface ITeacherAvailabilityRepository : IGenericRepository<TeacherAvailability>
{
    Task<IEnumerable<TeacherAvailability>> GetByTeacherIdAsync(int teacherId);
    Task<TeacherAvailability?> GetByTeacherAndDayAsync(int teacherId, WeekDay dayOfWeek);
}