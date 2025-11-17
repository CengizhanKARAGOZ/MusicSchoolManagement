using MusicSchoolManagement.Core.Enitties;

namespace MusicSchoolManagement.Core.Interfaces.Repositories;

public interface IStudentRepository : IGenericRepository<Student>
{
    Task<Student?> GetWithPackagesAsync(int id);
    Task<Student?> GetWithAppointmentsAsync(int id);
    Task<IEnumerable<Student>> GetActiveStudentsAsync();
}