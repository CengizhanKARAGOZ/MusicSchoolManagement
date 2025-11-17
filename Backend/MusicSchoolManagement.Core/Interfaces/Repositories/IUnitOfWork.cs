namespace MusicSchoolManagement.Core.Interfaces.Repositories;

public interface IUnitOfWork : IDisposable
{
    IUserRepository Users { get; }
    IStudentRepository Students { get; }
    ITeacherRepository Teachers { get; }
    IInstrumentRepository Instruments { get; }
    ICourseRepository Courses { get; }
    IClassroomRepository Classrooms { get; }
    IPackageRepository Packages { get; }
    IStudentPackageRepository StudentPackages { get; }
    IAppointmentRepository Appointments { get; }
    ITeacherAvailabilityRepository TeacherAvailabilities { get; }
    IPaymentRepository Payments { get; }
    INotificationRepository Notifications { get; }
    IAttendanceLogRepository AttendanceLogs { get; }
    
    Task<int> SaveChangesAsync();
}