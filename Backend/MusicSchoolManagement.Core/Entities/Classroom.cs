using MusicSchoolManagement.Core.Enitties;

namespace MusicSchoolManagement.Core.Entities;

public class Classroom : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? RoomNumber { get; set; }
    public int Capacity { get; set; }
    public string? SuitableInstruments { get; set; }
    public string? Equipment { get; set; }
    public bool IsActive { get; set; } = true;
    
    // Navigation properties
    public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
}