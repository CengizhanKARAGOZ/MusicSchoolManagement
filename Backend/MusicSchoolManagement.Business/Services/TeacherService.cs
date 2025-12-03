using AutoMapper;
using MusicSchoolManagement.Core.DTOs.Teachers;
using MusicSchoolManagement.Core.Enitties;
using MusicSchoolManagement.Core.Interfaces.Repositories;
using MusicSchoolManagement.Core.Interfaces.Services;

namespace MusicSchoolManagement.Business.Services;

public class TeacherService : ITeacherService
{
    #region Fields

    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    #endregion

    #region Constructor

    public TeacherService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    #endregion

    #region Public Methods

    public async Task<IEnumerable<TeacherDto>> GetAllTeachersAsync()
    {
        var teachers = await _unitOfWork.Teachers.GetAllAsync();
        var teacherDtos = new List<TeacherDto>();

        foreach (var teacher in teachers)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(teacher.UserId);
            if (user != null)
            {
                teacher.User = user;
                teacherDtos.Add(_mapper.Map<TeacherDto>(teacher));
            }
        }

        return teacherDtos;
    }

    public async Task<TeacherDto?> GetTeacherByIdAsync(int id)
    {
        var teacher = await _unitOfWork.Teachers.GetByUserIdAsync(id);
        if (teacher == null)
            return null;

        var user = await _unitOfWork.Users.GetByIdAsync(teacher.UserId);
        if (user == null)
            return null;

        teacher.User = user;
        return _mapper.Map<TeacherDto>(teacher);
    }

    public async Task<TeacherDto?> GetTeacherByUserIdAsync(int userId)
    {
        var teacher = await _unitOfWork.Teachers.GetByUserIdAsync(userId);
        if (teacher == null)
            return null;

        var user = await _unitOfWork.Users.GetByIdAsync(userId);
        if (user == null)
            return null;

        teacher.User = user;
        return _mapper.Map<TeacherDto>(teacher);
    }

    public async Task<TeacherDto> CreateTeacherAsync(CreateTeacherDto createDto)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(createDto.UserId);
        if (user == null)
            throw new InvalidOperationException("User not found");

        if (user.Role != Core.Enums.UserRole.Teacher)
            throw new InvalidOperationException("User must have Teacher role");

        var existingTeacher = await _unitOfWork.Teachers.GetByUserIdAsync(createDto.UserId);
        if (existingTeacher != null)
            throw new InvalidOperationException("Teacher profile already exists for this user");

        var teacher = _mapper.Map<Teacher>(createDto);

        await _unitOfWork.Teachers.AddAsync(teacher);
        await _unitOfWork.SaveChangesAsync();

        teacher.User = user;
        return _mapper.Map<TeacherDto>(teacher);
    }

    public async Task<TeacherDto?> UpdateTeacherAsync(int id, UpdateTeacherDto updateDto)
    {
        var teacher = await _unitOfWork.Teachers.GetByIdAsync(id);
        if (teacher == null)
            return null;

        _mapper.Map(updateDto, teacher);

        _unitOfWork.Teachers.Update(teacher);
        await _unitOfWork.SaveChangesAsync();

        var user = await _unitOfWork.Users.GetByIdAsync(teacher.UserId);
        if (user == null)
            return null;

        teacher.User = user;
        return _mapper.Map<TeacherDto>(teacher);
    }

    public async Task<bool> DeleteTeacherAsync(int id)
    {
        var teacher = await _unitOfWork.Teachers.GetByIdAsync(id);
        if (teacher == null)
            return false;

        _unitOfWork.Teachers.Remove(teacher);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }

    #endregion
}