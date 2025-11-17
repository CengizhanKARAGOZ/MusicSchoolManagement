using MusicSchoolManagement.Core.Enitties;

namespace MusicSchoolManagement.Core.Interfaces.Repositories;

public interface ITeacherRepository : IGenericRepository<Teacher>
{
    Task<Teacher?> GetByUserIdAsync(int userId);
    Task<Teacher?> GetWithAvailabilitiesAsync(int id);
    Task<Teacher?> GetWithAppointmentsAsync(int id);
}