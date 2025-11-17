using MusicSchoolManagement.Core.Entities;

namespace MusicSchoolManagement.Core.Interfaces.Repositories;

public interface INotificationRepository : IGenericRepository<Notification>
{
    Task<IEnumerable<Notification>> GetPendingNotificationsAsync();
    Task<IEnumerable<Notification>> GetByUserIdAsync(int userId);
    Task<IEnumerable<Notification>> GetByStudentIdAsync(int studentId);
}