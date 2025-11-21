using MusicSchoolManagement.Core.DTOs.Students;
using MusicSchoolManagement.Core.Enitties;
using MusicSchoolManagement.Core.Interfaces.Repositories;
using MusicSchoolManagement.Core.Interfaces.Services;

namespace MusicSchoolManagement.Business.Services;

public class StudentService : IStudentService
{
    private readonly IUnitOfWork _unitOfWork;
    public StudentService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<IEnumerable<StudentDto>> GetAllStudentsAsync()
    {
        var students = await _unitOfWork.Students.GetAllAsync();
        return students.Select(MapToDto);
    }

    public async Task<StudentDto?> GetStudentByIdAsync(int id)
    {
        var student =  await _unitOfWork.Students.GetByIdAsync(id);
        return student == null ? null : MapToDto(student);
    }

    public async Task<IEnumerable<StudentDto>> GetActiveStudentsAsync()
    {
        var students = await _unitOfWork.Students.GetActiveStudentsAsync();
        return students.Select(MapToDto);
    }

    public async Task<StudentDto> CreateStudentAsync(CreateStudentDto createDto)
    {
        var student = new Student
        {
            FirstName = createDto.FirstName,
            LastName = createDto.LastName,
            Email = createDto.Email,
            PhoneNumber = createDto.PhoneNumber,
            DateOfBirth = createDto.DateOfBirth,
            ParentName = createDto.ParentName,
            ParentPhone = createDto.ParentPhone,
            ParentEmail = createDto.ParentEmail,
            Address = createDto.Address,
            EmergencyContact = createDto.EmergencyContact,
            Notes = createDto.Notes,
            RegistrationDate = createDto.RegistrationDate,
            IsActive = true
        };
        
        await _unitOfWork.Students.AddAsync(student);
        await _unitOfWork.SaveChangesAsync();

        return MapToDto(student);
    }

    public async Task<StudentDto?> UpdateStudentAsync(int id, UpdateStudentDto updateDto)
    {
        var student = await _unitOfWork.Students.GetByIdAsync(id);
        if (student == null)
            return null;

        student.FirstName = updateDto.FirstName;
        student.LastName = updateDto.LastName;
        student.Email = updateDto.Email;
        student.PhoneNumber = updateDto.PhoneNumber;
        student.DateOfBirth = updateDto.DateOfBirth;
        student.ParentName = updateDto.ParentName;
        student.ParentPhone = updateDto.ParentPhone;
        student.ParentEmail = updateDto.ParentEmail;
        student.Address = updateDto.Address;
        student.EmergencyContact = updateDto.EmergencyContact;
        student.Notes = updateDto.Notes;
        student.IsActive = updateDto.IsActive;

        _unitOfWork.Students.Update(student);
        await _unitOfWork.SaveChangesAsync();

        return MapToDto(student);
    }

    public async Task<bool> DeleteStudentAsync(int id)
    {
        var student = await _unitOfWork.Students.GetByIdAsync(id);
        if (student == null)
            return false;
        
        _unitOfWork.Students.Remove(student);
        await _unitOfWork.SaveChangesAsync();
        
        return true;
    }
    
    private static StudentDto MapToDto(Student student)
    {
        return new StudentDto
        {
            Id = student.Id,
            FirstName = student.FirstName,
            LastName = student.LastName,
            Email = student.Email,
            PhoneNumber = student.PhoneNumber,
            DateOfBirth = student.DateOfBirth,
            ParentName = student.ParentName,
            ParentPhone = student.ParentPhone,
            ParentEmail = student.ParentEmail,
            Address = student.Address,
            EmergencyContact = student.EmergencyContact,
            Notes = student.Notes,
            RegistrationDate = student.RegistrationDate,
            IsActive = student.IsActive,
            CreatedAt = student.CreatedAt
        };
    }
}