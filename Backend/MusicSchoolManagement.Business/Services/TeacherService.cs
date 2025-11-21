using MusicSchoolManagement.Core.DTOs.Teachers;
using MusicSchoolManagement.Core.Enitties;
using MusicSchoolManagement.Core.Enums;
using MusicSchoolManagement.Core.Interfaces.Repositories;
using MusicSchoolManagement.Core.Interfaces.Services;

namespace MusicSchoolManagement.Business.Services;

public class TeacherService : ITeacherService
{
    private readonly IUnitOfWork _unitOfWork;

    public TeacherService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<TeacherDto>> GetAllTeachersAsync()
    {
        var teachers = await _unitOfWork.Teachers.GetAllAsync();
        var teacherDtos = new List<TeacherDto>();

        foreach (var teacher in teachers)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(teacher.UserId);
            if (user != null)
            {
                teacherDtos.Add(MapToDto(teacher, user));
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

        return MapToDto(teacher, user);
    }

    public async Task<TeacherDto?> GetTeacherByUserIdAsync(int userId)
    {
        var teacher = await _unitOfWork.Teachers.GetByUserIdAsync(userId);
        if (teacher == null)
            return null;

        var user = await _unitOfWork.Users.GetByIdAsync(userId);
        if (user == null)
            return null;

        return MapToDto(teacher, user);
    }

    public async Task<TeacherDto> CreateTeacherAsync(CreateTeacherDto createDto)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(createDto.UserId);
        if (user == null)
            throw new InvalidOperationException("User not found");

        if (user.Role != UserRole.Teacher)
            throw new InvalidOperationException("User must have Teacher role");

        var existingTeacher = await _unitOfWork.Teachers.GetByUserIdAsync(createDto.UserId);
        if (existingTeacher != null)
            throw new InvalidOperationException("Teacher profile already exists for this user");

        var teacher = new Teacher
        {
            UserId = createDto.UserId,
            Specializations = createDto.Specializations,
            HourlyRate = createDto.HourlyRate,
            Biography = createDto.Biography,
            AvailabilityNotes = createDto.AvailabilityNotes
        };

        await _unitOfWork.Teachers.AddAsync(teacher);
        await _unitOfWork.SaveChangesAsync();

        return MapToDto(teacher, user);
    }

    public async Task<TeacherDto?> UpdateTeacherAsync(int id, UpdateTeacherDto updateDto)
    {
        var teacher = await _unitOfWork.Teachers.GetByIdAsync(id);
        if (teacher == null)
            return null;

        teacher.Specializations = updateDto.Specializations;
        teacher.HourlyRate = updateDto.HourlyRate;
        teacher.Biography = updateDto.Biography;
        teacher.AvailabilityNotes = updateDto.AvailabilityNotes;

        _unitOfWork.Teachers.Update(teacher);
        await _unitOfWork.SaveChangesAsync();

        var user = await _unitOfWork.Users.GetByIdAsync(teacher.UserId);
        if (user == null)
            return null;

        return MapToDto(teacher, user);
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

    private static TeacherDto MapToDto(Teacher teacher, User user)
    {
        return new TeacherDto
        {
            Id = teacher.Id,
            UserId = teacher.UserId,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            Specializations = teacher.Specializations,
            HourlyRate = teacher.HourlyRate,
            Biography = teacher.Biography,
            AvailabilityNotes = teacher.AvailabilityNotes,
            CreatedAt = teacher.CreatedAt
        };
    }
}