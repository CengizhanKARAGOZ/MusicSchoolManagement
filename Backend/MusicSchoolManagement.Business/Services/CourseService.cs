using MusicSchoolManagement.Core.DTOs.Courses;
using MusicSchoolManagement.Core.Entities;
using MusicSchoolManagement.Core.Interfaces.Repositories;
using MusicSchoolManagement.Core.Interfaces.Services;

namespace MusicSchoolManagement.Business.Services;

public class CourseService : ICourseService
{
    private readonly IUnitOfWork _unitOfWork;

    public CourseService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<CourseDto>> GetAllCoursesAsync()
    {
        var courses = await _unitOfWork.Courses.GetAllAsync();
        var courseDtos = new List<CourseDto>();

        foreach (var course in courses)
        {
            var instrument = await _unitOfWork.Instruments.GetByIdAsync(course.InstrumentId);
            courseDtos.Add(MapToDto(course, instrument?.Name ?? "Unknown"));
        }

        return courseDtos;
    }

    public async Task<IEnumerable<CourseDto>> GetActiveCoursesAsync()
    {
        var courses = await _unitOfWork.Courses.GetActiveCoursesAsync();
        return courses.Select(c => MapToDto(c, c.Instrument.Name));
    }

    public async Task<IEnumerable<CourseDto>> GetCoursesByInstrumentAsync(int instrumentId)
    {
        var courses = await _unitOfWork.Courses.GetByInstrumentIdAsync(instrumentId);
        return courses.Select(c => MapToDto(c, c.Instrument.Name));
    }

    public async Task<CourseDto?> GetCourseByIdAsync(int id)
    {
        var course = await _unitOfWork.Courses.GetByIdAsync(id);
        if (course == null)
            return null;
        return MapToDto(course, course.Instrument.Name);
    }

    public async Task<CourseDto> CreateCourseAsync(CreateCourseDto createDto)
    {
        var course = new Course
        {
            InstrumentId = createDto.InstrumentId,
            Name = createDto.Name,
            Level = createDto.Level,
            Type = createDto.Type,
            Duration = createDto.Duration,
            BasePrice = createDto.BasePrice,
            MaxStudents = createDto.MaxStudents,
            Description = createDto.Description,
            IsActive = true
        };
        
        await _unitOfWork.Courses.AddAsync(course);
        await _unitOfWork.SaveChangesAsync();
        
        var instrument = await _unitOfWork.Instruments.GetByIdAsync(course.InstrumentId);
        return MapToDto(course, instrument?.Name ?? "Unknown");
    }

    public async Task<CourseDto?> UpdateCourseAsync(int id, UpdateCourseDto updateDto)
    {
        var course = await _unitOfWork.Courses.GetByIdAsync(id);
        if (course == null)
            return null;

        course.Name = updateDto.Name;
        course.Level = updateDto.Level;
        course.Type = updateDto.Type;
        course.Duration = updateDto.Duration;
        course.BasePrice = updateDto.BasePrice;
        course.MaxStudents = updateDto.MaxStudents;
        course.Description = updateDto.Description;
        course.IsActive = updateDto.IsActive;

        _unitOfWork.Courses.Update(course);
        await _unitOfWork.SaveChangesAsync();

        var instrument = await _unitOfWork.Instruments.GetByIdAsync(course.InstrumentId);
        return MapToDto(course, instrument?.Name ?? "Unknown");
    }

    public async Task<bool> DeleteCourseAsync(int id)
    {
        var course = await _unitOfWork.Courses.GetByIdAsync(id);
        if (course == null)
            return false;
        
        _unitOfWork.Courses.Remove(course);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }
    
    private static CourseDto MapToDto(Course course, string instrumentName)
    {
        return new CourseDto
        {
            Id = course.Id,
            InstrumentId = course.InstrumentId,
            InstrumentName = instrumentName,
            Name = course.Name,
            Level = course.Level,
            Type = course.Type,
            Duration = course.Duration,
            BasePrice = course.BasePrice,
            MaxStudents = course.MaxStudents,
            Description = course.Description,
            IsActive = course.IsActive
        };
    }
}