namespace MusicSchoolManagement.Core.DTOs.Common;

public class ClassroomDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? RoomNumber { get; set; }
    public int Capacity { get; set; }
    public string? SuitableInstruments { get; set; }
    public string? Equipment { get; set; }
    public bool IsActive { get; set; }
}