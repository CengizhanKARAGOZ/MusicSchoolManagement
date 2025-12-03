using AutoMapper;
using MusicSchoolManagement.Core.DTOs.Common;
using MusicSchoolManagement.Core.Entities;
using MusicSchoolManagement.Core.Exceptions;
using MusicSchoolManagement.Core.Interfaces.Repositories;
using MusicSchoolManagement.Core.Interfaces.Services;

namespace MusicSchoolManagement.Business.Services;

public class ClassroomService : IClassroomService
{
    #region Fields

    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    #endregion

    #region Constructor

    public ClassroomService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    #endregion

    #region Public Methods

    public async Task<IEnumerable<ClassroomDto>> GetAllClassroomsAsync()
    {
        var classrooms = await _unitOfWork.Classrooms.GetAllAsync();
        return _mapper.Map<IEnumerable<ClassroomDto>>(classrooms);
    }

    public async Task<IEnumerable<ClassroomDto>> GetActiveClassroomsAsync()
    {
        var classrooms = await _unitOfWork.Classrooms.GetActiveClassroomsAsync();
        return _mapper.Map<IEnumerable<ClassroomDto>>(classrooms);
    }

    public async Task<IEnumerable<ClassroomDto>> GetAvailableClassroomsAsync(DateTime date, TimeSpan startTime, TimeSpan endTime)
    {
        var classrooms = await _unitOfWork.Classrooms.GetAvailableClassroomsAsync(date, startTime, endTime);
        return _mapper.Map<IEnumerable<ClassroomDto>>(classrooms);
    }

    public async Task<ClassroomDto?> GetClassroomByIdAsync(int id)
    {
        var classroom = await _unitOfWork.Classrooms.GetByIdAsync(id);
        if (classroom == null)
            throw new NotFoundException("Classroom", id);

        return _mapper.Map<ClassroomDto>(classroom);
    }

    public async Task<ClassroomDto> CreateClassroomAsync(string name, string? roomNumber, int capacity, string? suitableInstruments, string? equipment)
    {
        var classroom = new Classroom
        {
            Name = name,
            RoomNumber = roomNumber,
            Capacity = capacity,
            SuitableInstruments = suitableInstruments,
            Equipment = equipment,
            IsActive = true
        };

        await _unitOfWork.Classrooms.AddAsync(classroom);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<ClassroomDto>(classroom);
    }

    public async Task<ClassroomDto?> UpdateClassroomAsync(int id, string name, string? roomNumber, int capacity, string? suitableInstruments, string? equipment, bool isActive)
    {
        var classroom = await _unitOfWork.Classrooms.GetByIdAsync(id);
        if (classroom == null)
            throw new NotFoundException("Classroom", id);

        classroom.Name = name;
        classroom.RoomNumber = roomNumber;
        classroom.Capacity = capacity;
        classroom.SuitableInstruments = suitableInstruments;
        classroom.Equipment = equipment;
        classroom.IsActive = isActive;

        _unitOfWork.Classrooms.Update(classroom);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<ClassroomDto>(classroom);
    }

    public async Task<bool> DeleteClassroomAsync(int id)
    {
        var classroom = await _unitOfWork.Classrooms.GetByIdAsync(id);
        if (classroom == null)
            throw new NotFoundException("Classroom", id);

        _unitOfWork.Classrooms.Remove(classroom);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }

    #endregion
}