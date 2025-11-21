using MusicSchoolManagement.Core.DTOs.Teachers;

namespace MusicSchoolManagement.Core.Interfaces.Services;

public interface ITeacherService
{
    Task<IEnumerable<TeacherDto>> GetAllTeachersAsync();
    Task<TeacherDto?> GetTeacherByIdAsync(int id);
    Task<TeacherDto?> GetTeacherByUserIdAsync(int userId);
    Task<TeacherDto> CreateTeacherAsync(CreateTeacherDto createDto);
    Task<TeacherDto?> UpdateTeacherAsync(int id, UpdateTeacherDto updateDto);
    Task<bool> DeleteTeacherAsync(int id);
}