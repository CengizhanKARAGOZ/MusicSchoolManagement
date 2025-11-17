using MusicSchoolManagement.Core.Entities;

namespace MusicSchoolManagement.Core.Interfaces.Repositories;

public interface IClassroomRepository : IGenericRepository<Classroom>
{
    Task<IEnumerable<Classroom>> GetActiveClassroomsAsync();
    Task<IEnumerable<Classroom>> GetAvailableClassroomsAsync(DateTime date, TimeSpan startTime, TimeSpan endTime);
}