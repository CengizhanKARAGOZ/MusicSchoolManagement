using MusicSchoolManagement.Core.DTOs.Common;
using MusicSchoolManagement.Core.Entities;
using MusicSchoolManagement.Core.Interfaces.Repositories;
using MusicSchoolManagement.Core.Interfaces.Services;

namespace MusicSchoolManagement.Business.Services;

public class ClassroomService : IClassroomService
{
    private readonly IUnitOfWork _unitOfWork;

    public ClassroomService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<ClassroomDto>> GetAllClassroomsAsync()
    {
        var classrooms = await _unitOfWork.Classrooms.GetAllAsync();
        return classrooms.Select(MapToDto);
    }

    public async Task<IEnumerable<ClassroomDto>> GetActiveClassroomsAsync()
    {
        var classrooms =  await _unitOfWork.Classrooms.GetActiveClassroomsAsync();
        return classrooms.Select(MapToDto);
    }

    public async Task<IEnumerable<ClassroomDto>> GetAvailableClassroomsAsync(DateTime date, TimeSpan startTime, TimeSpan endTime)
    {
        var classrooms =  await _unitOfWork.Classrooms.GetAvailableClassroomsAsync(date, startTime, endTime);
        return classrooms.Select(MapToDto);
    }

    public async Task<ClassroomDto?> GetClassroomByIdAsync(int id)
    {
        var classroom = await _unitOfWork.Classrooms.GetByIdAsync(id);
        return classroom == null ? null : MapToDto(classroom);
    }

    public async Task<ClassroomDto> CreateClassroomAsync(string name, string? roomNumber, int capacity, string? suitableInstruments,
        string? equipment)
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

        return MapToDto(classroom);
    }

    public async Task<ClassroomDto?> UpdateClassroomAsync(int id, string name, string? roomNumber, int capacity, string? suitableInstruments,
        string? equipment, bool isActive)
    {
        var classroom = await _unitOfWork.Classrooms.GetByIdAsync(id);
        if (classroom == null)
            return null;

        classroom.Name = name;
        classroom.RoomNumber = roomNumber;
        classroom.Capacity = capacity;
        classroom.SuitableInstruments = suitableInstruments;
        classroom.Equipment = equipment;
        classroom.IsActive = isActive;

        _unitOfWork.Classrooms.Update(classroom);
        await _unitOfWork.SaveChangesAsync();

        return MapToDto(classroom);
    }

    public async Task<bool> DeleteClassroomAsync(int id)
    {
        var classroom = await _unitOfWork.Classrooms.GetByIdAsync(id);
        if (classroom == null)
            return false;
        
        _unitOfWork.Classrooms.Remove(classroom);
        await _unitOfWork.SaveChangesAsync();
        
        return true;
    }
    
    private static ClassroomDto MapToDto(Classroom classroom)
    {
        return new ClassroomDto
        {
            Id = classroom.Id,
            Name = classroom.Name,
            RoomNumber = classroom.RoomNumber,
            Capacity = classroom.Capacity,
            SuitableInstruments = classroom.SuitableInstruments,
            Equipment = classroom.Equipment,
            IsActive = classroom.IsActive
        };
    }
}