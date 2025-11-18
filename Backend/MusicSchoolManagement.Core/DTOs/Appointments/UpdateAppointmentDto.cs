using MusicSchoolManagement.Core.Enums;

namespace MusicSchoolManagement.Core.DTOs.Appointments;

public class UpdateAppointmentDto
{
    public DateTime AppointmentDate { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public int? ClassroomId { get; set; }
    public AppointmentStatus Status { get; set; }
    public string? Notes { get; set; }
}