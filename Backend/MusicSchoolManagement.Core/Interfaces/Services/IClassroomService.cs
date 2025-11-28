using MusicSchoolManagement.Core.DTOs.Common;

namespace MusicSchoolManagement.Core.Interfaces.Services;

public interface IClassroomService
{
    Task<IEnumerable<ClassroomDto>> GetAllClassroomsAsync();
    Task<IEnumerable<ClassroomDto>> GetActiveClassroomsAsync();
    Task<IEnumerable<ClassroomDto>> GetAvailableClassroomsAsync(DateTime date, TimeSpan startTime, TimeSpan endTime);
    Task<ClassroomDto?> GetClassroomByIdAsync(int id);
    Task<ClassroomDto> CreateClassroomAsync(string name, string? roomNumber, int capacity, string? suitableInstruments, string? equipment);
    Task<ClassroomDto?> UpdateClassroomAsync(int id, string name, string? roomNumber, int capacity, string? suitableInstruments, string? equipment, bool isActive);
    Task<bool> DeleteClassroomAsync(int id);
}