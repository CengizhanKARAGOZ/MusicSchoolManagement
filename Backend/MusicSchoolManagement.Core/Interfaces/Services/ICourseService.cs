using MusicSchoolManagement.Core.DTOs.Courses;

namespace MusicSchoolManagement.Core.Interfaces.Services;

public interface ICourseService
{
    Task<IEnumerable<CourseDto>> GetAllCoursesAsync();
    Task<IEnumerable<CourseDto>> GetActiveCoursesAsync();
    Task<IEnumerable<CourseDto>> GetCoursesByInstrumentAsync(int instrumentId);
    Task<CourseDto?> GetCourseByIdAsync(int id);
    Task<CourseDto> CreateCourseAsync(CreateCourseDto createDto);
    Task<CourseDto?> UpdateCourseAsync(int id, UpdateCourseDto updateDto);
    Task<bool> DeleteCourseAsync(int id);
}