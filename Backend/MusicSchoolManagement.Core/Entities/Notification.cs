using MusicSchoolManagement.Core.Enums;

namespace MusicSchoolManagement.Core.Entities;

public class Notification : BaseEntity
{
    public int? UserId { get; set; }
    public int? StudentId { get; set; }
    public NotificationType Type { get; set; }
    public string? Subject { get; set; }
    public string Message { get; set; } = string.Empty;
    public NotificationStatus Status { get; set; } = NotificationStatus.Pending;
    public DateTime? SentAt { get; set; }
    
    // Navigation properties
    public User? User { get; set; }
    public Student? Student { get; set; }
}