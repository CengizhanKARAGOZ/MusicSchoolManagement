using MusicSchoolManagement.Core.Interfaces.Repositories;
using MusicSchoolManagement.Infrastructure.Data;

namespace MusicSchoolManagement.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;

    public IUserRepository Users { get; private set; }
    public IStudentRepository Students { get; private set; }
    public ITeacherRepository Teachers { get; private set; }
    public IInstrumentRepository Instruments { get; private set; }
    public ICourseRepository Courses { get; private set; }
    public IClassroomRepository Classrooms { get; private set; }
    public IPackageRepository Packages { get; private set; }
    public IStudentPackageRepository StudentPackages { get; private set; }
    public IAppointmentRepository Appointments { get; private set; }
    public ITeacherAvailabilityRepository TeacherAvailabilities { get; private set; }
    public IPaymentRepository Payments { get; private set; }
    public INotificationRepository Notifications { get; private set; }
    public IAttendanceLogRepository AttendanceLogs { get; private set; }

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
        Users = new UserRepository(context);
        Students = new StudentRepository(context);
        Teachers = new TeacherRepository(context);
        Instruments = new InstrumentRepository(context);
        Courses = new CourseRepository(context);
        Classrooms = new ClassroomRepository(context);
        Packages = new PackageRepository(context);
        StudentPackages = new StudentPackageRepository(context);
        Appointments = new AppointmentRepository(context);
        TeacherAvailabilities = new TeacherAvailabilityRepository(context);
        Payments = new PaymentRepository(context);
        Notifications = new NotificationRepository(context);
        AttendanceLogs = new AttendanceLogRepository(context);
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}