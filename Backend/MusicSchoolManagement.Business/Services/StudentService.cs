using AutoMapper;
using MusicSchoolManagement.Core.DTOs.Students;
using MusicSchoolManagement.Core.Enitties;
using MusicSchoolManagement.Core.Exceptions;
using MusicSchoolManagement.Core.Interfaces.Repositories;
using MusicSchoolManagement.Core.Interfaces.Services;

namespace MusicSchoolManagement.Business.Services;

public class StudentService : IStudentService
{
    #region Fields

    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    #endregion

    #region Constructor

    public StudentService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    #endregion

    #region Public Methods

    public async Task<IEnumerable<StudentDto>> GetAllStudentsAsync()
    {
        var students = await _unitOfWork.Students.GetAllAsync();
        return _mapper.Map<IEnumerable<StudentDto>>(students);
    }

    public async Task<StudentDto?> GetStudentByIdAsync(int id)
    {
        var student = await _unitOfWork.Students.GetByIdAsync(id);
        if (student == null)
            throw new NotFoundException("Student", id);
        return _mapper.Map<StudentDto>(student);
    }

    public async Task<IEnumerable<StudentDto>> GetActiveStudentsAsync()
    {
        var students = await _unitOfWork.Students.GetActiveStudentsAsync();
        return _mapper.Map<IEnumerable<StudentDto>>(students);
    }

    public async Task<StudentDto> CreateStudentAsync(CreateStudentDto createDto)
    {
        var student = _mapper.Map<Student>(createDto);
        
        await _unitOfWork.Students.AddAsync(student);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<StudentDto>(student);
    }

    public async Task<StudentDto?> UpdateStudentAsync(int id, UpdateStudentDto updateDto)
    {
        var student = await _unitOfWork.Students.GetByIdAsync(id);
        if (student == null)
            throw new NotFoundException("Student", id);

        _mapper.Map(updateDto, student);

        _unitOfWork.Students.Update(student);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<StudentDto>(student);
    }

    public async Task<bool> DeleteStudentAsync(int id)
    {
        var student = await _unitOfWork.Students.GetByIdAsync(id);
        if (student == null)
            throw new NotFoundException("Student", id);

        _unitOfWork.Students.Remove(student);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }

    #endregion
}