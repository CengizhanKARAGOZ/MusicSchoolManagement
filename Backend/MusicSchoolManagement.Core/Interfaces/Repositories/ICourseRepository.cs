using MusicSchoolManagement.Core.Entities;

namespace MusicSchoolManagement.Core.Interfaces.Repositories;

public interface ICourseRepository : IGenericRepository<Course>
{
    Task<IEnumerable<Course>> GetByInstrumentIdAsync(int instrumentId);
    Task<IEnumerable<Course>> GetActiveCoursesAsync();
    Task<Course?> GetWithInstrumentAsync(int id);
}