using MusicSchoolManagement.Core.DTOs.Students;

namespace MusicSchoolManagement.Core.Interfaces.Services;

public interface IStudentService
{
    Task<IEnumerable<StudentDto>> GetAllStudentsAsync();
    Task<StudentDto?> GetStudentByIdAsync(int id);
    Task<IEnumerable<StudentDto>> GetActiveStudentsAsync();
    Task<StudentDto> CreateStudentAsync(CreateStudentDto createDto);
    Task<StudentDto?> UpdateStudentAsync(int id, UpdateStudentDto updateDto);
    Task<bool> DeleteStudentAsync(int id);
}