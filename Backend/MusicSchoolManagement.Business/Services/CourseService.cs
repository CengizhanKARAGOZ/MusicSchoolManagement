using AutoMapper;
using MusicSchoolManagement.Core.DTOs.Courses;
using MusicSchoolManagement.Core.Entities;
using MusicSchoolManagement.Core.Exceptions;
using MusicSchoolManagement.Core.Interfaces.Repositories;
using MusicSchoolManagement.Core.Interfaces.Services;

namespace MusicSchoolManagement.Business.Services;

public class CourseService : ICourseService
{
    #region Fields

    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    #endregion

    #region Constructor

    public CourseService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    #endregion

    #region Public Methods

    public async Task<IEnumerable<CourseDto>> GetAllCoursesAsync()
    {
        var courses = await _unitOfWork.Courses.GetAllAsync();
        var courseDtos = new List<CourseDto>();

        foreach (var course in courses)
        {
            var instrument = await _unitOfWork.Instruments.GetByIdAsync(course.InstrumentId);
            course.Instrument = instrument!;
            courseDtos.Add(_mapper.Map<CourseDto>(course));
        }

        return courseDtos;
    }

    public async Task<IEnumerable<CourseDto>> GetActiveCoursesAsync()
    {
        var courses = await _unitOfWork.Courses.GetActiveCoursesAsync();
        return _mapper.Map<IEnumerable<CourseDto>>(courses);
    }

    public async Task<IEnumerable<CourseDto>> GetCoursesByInstrumentAsync(int instrumentId)
    {
        var courses = await _unitOfWork.Courses.GetByInstrumentIdAsync(instrumentId);
        return _mapper.Map<IEnumerable<CourseDto>>(courses);
    }

    public async Task<CourseDto?> GetCourseByIdAsync(int id)
    {
        var course = await _unitOfWork.Courses.GetWithInstrumentAsync(id);
        if (course == null)
            throw new NotFoundException("Course", id);

        return _mapper.Map<CourseDto>(course);
    }

    public async Task<CourseDto> CreateCourseAsync(CreateCourseDto createDto)
    {
        var instrument = await _unitOfWork.Instruments.GetByIdAsync(createDto.InstrumentId);
        if (instrument == null)
            throw new NotFoundException("Instrument", createDto.InstrumentId);

        var course = _mapper.Map<Course>(createDto);

        await _unitOfWork.Courses.AddAsync(course);
        await _unitOfWork.SaveChangesAsync();

        course.Instrument = instrument;
        
        return _mapper.Map<CourseDto>(course);
    }

    public async Task<CourseDto?> UpdateCourseAsync(int id, UpdateCourseDto updateDto)
    {
        var course = await _unitOfWork.Courses.GetByIdAsync(id);
        if (course == null)
            throw new NotFoundException("Course", id);

        _mapper.Map(updateDto, course);

        _unitOfWork.Courses.Update(course);
        await _unitOfWork.SaveChangesAsync();

        var instrument = await _unitOfWork.Instruments.GetByIdAsync(course.InstrumentId);
        course.Instrument = instrument!;
        
        return _mapper.Map<CourseDto>(course);
    }

    public async Task<bool> DeleteCourseAsync(int id)
    {
        var course = await _unitOfWork.Courses.GetByIdAsync(id);
        if (course == null)
            throw new NotFoundException("Course", id);

        _unitOfWork.Courses.Remove(course);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }

    #endregion
}