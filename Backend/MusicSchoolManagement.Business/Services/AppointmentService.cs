using MusicSchoolManagement.Core.DTOs.Appointments;
using MusicSchoolManagement.Core.Entities;
using MusicSchoolManagement.Core.Enums;
using MusicSchoolManagement.Core.Interfaces.Repositories;
using MusicSchoolManagement.Core.Interfaces.Services;

namespace MusicSchoolManagement.Business.Services;

public class AppointmentService : IAppointmentService
{
    private readonly IUnitOfWork _unitOfWork;

    public AppointmentService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<AppointmentDto>> GetAllAppointmentsAsync()
    {
        var appointments = await _unitOfWork.Appointments.GetAllAsync();
        return appointments.Select(MapToDto);
    }

    public async Task<IEnumerable<AppointmentDto>> GetAppointmentsByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        var appointments = await _unitOfWork.Appointments.GetByDateRangeAsync(startDate, endDate);
        return appointments.Select(MapToDto);
    }

    public async Task<IEnumerable<AppointmentDto>> GetAppointmentsByTeacherAsync(int teacherId, DateTime? date = null)
    {
        var appointments = await _unitOfWork.Appointments.GetByTeacherIdAsync(teacherId, date);
        return appointments.Select(MapToDto);
    }

    public async Task<IEnumerable<AppointmentDto>> GetAppointmentsByStudentAsync(int studentId, DateTime? date = null)
    {
        var appointments =  await _unitOfWork.Appointments.GetByStudentIdAsync(studentId, date);
        return appointments.Select(MapToDto);
    }

    public async Task<IEnumerable<AppointmentDto>> GetUpcomingAppointmentsAsync(int count)
    {
        var appointments = await _unitOfWork.Appointments.GetUpcomingAppointmentsAsync(count);
        return appointments.Select(MapToDto);
    }

    public async Task<AppointmentDto?> GetAppointmentByIdAsync(int id)
    {
        var appointment =  await _unitOfWork.Appointments.GetByIdAsync(id);
        return appointment == null ? null : MapToDto(appointment);
    }

    public async Task<AppointmentDto> CreateAppointmentAsync(CreateAppointmentDto createDto, int createdByUserId)
    {
        await ValidateEntitiesAsync(createDto.StudentId, createDto.TeacherId, createDto.CourseId, createDto.ClassroomId);
        
        var hasConflict = await _unitOfWork.Appointments.HasConflictAsync(
            createDto.TeacherId,
            createDto.ClassroomId ?? 0,
            createDto.StudentId,
            createDto.AppointmentDate,
            createDto.StartTime,
            createDto.EndTime);

        if (hasConflict)
            throw new InvalidOperationException("Time slot conflict detected");
        
        var appointment = new Appointment
        {
            StudentId = createDto.StudentId,
            TeacherId = createDto.TeacherId,
            CourseId = createDto.CourseId,
            ClassroomId = createDto.ClassroomId,
            StudentPackageId = createDto.StudentPackageId,
            AppointmentDate = createDto.AppointmentDate,
            StartTime = createDto.StartTime,
            EndTime = createDto.EndTime,
            Status = AppointmentStatus.Scheduled,
            IsRecurring = false,
            Notes = createDto.Notes,
            CreatedBy = createdByUserId
        };
        
        await _unitOfWork.Appointments.AddAsync(appointment);
        await _unitOfWork.SaveChangesAsync();

        if (createDto.StudentPackageId.HasValue)
        {
            await UpdateStudentPackageUsageAsync(createDto.StudentPackageId.Value);
        }

        return await GetAppointmentByIdAsync(appointment.Id) ??
               throw new InvalidOperationException("Failed to retrieve created appointment");
    }

    public async Task<IEnumerable<AppointmentDto>> CreateRecurringAppointmentsAsync(CreateRecurringAppointmentDto createDto, int createdByUserId)
    {
        await ValidateEntitiesAsync(createDto.StudentId, createDto.TeacherId, createDto.CourseId, createDto.ClassroomId);
        
        var appointments = new List<Appointment>();
        var currentDate = createDto.StartDate;
        
        var parentAppointment = new Appointment
        {
            StudentId = createDto.StudentId,
            TeacherId = createDto.TeacherId,
            CourseId = createDto.CourseId,
            ClassroomId = createDto.ClassroomId,
            StudentPackageId = createDto.StudentPackageId,
            AppointmentDate = currentDate,
            StartTime = createDto.StartTime,
            EndTime = createDto.EndTime,
            Status = AppointmentStatus.Scheduled,
            IsRecurring = true,
            RecurringPattern = createDto.RecurringPattern,
            RecurringEndDate = createDto.EndDate,
            Notes = createDto.Notes,
            CreatedBy = createdByUserId
        };
        
        await _unitOfWork.Appointments.AddAsync(parentAppointment);
        await _unitOfWork.SaveChangesAsync();
        
        appointments.Add(parentAppointment);
        
        int increment = createDto.RecurringPattern.ToLower() == "weekly" ? 7 : 14;

        currentDate = currentDate.AddDays(increment);
        
        while (currentDate <= createDto.EndDate)
        {
            // Check for conflicts
            var hasConflict = await _unitOfWork.Appointments.HasConflictAsync(
                createDto.TeacherId,
                createDto.ClassroomId ?? 0,
                createDto.StudentId,
                currentDate,
                createDto.StartTime,
                createDto.EndTime);

            if (!hasConflict)
            {
                var childAppointment = new Appointment
                {
                    StudentId = createDto.StudentId,
                    TeacherId = createDto.TeacherId,
                    CourseId = createDto.CourseId,
                    ClassroomId = createDto.ClassroomId,
                    StudentPackageId = createDto.StudentPackageId,
                    AppointmentDate = currentDate,
                    StartTime = createDto.StartTime,
                    EndTime = createDto.EndTime,
                    Status = AppointmentStatus.Scheduled,
                    IsRecurring = true,
                    RecurringPattern = createDto.RecurringPattern,
                    ParentAppointmentId = parentAppointment.Id,
                    Notes = createDto.Notes,
                    CreatedBy = createdByUserId
                };

                appointments.Add(childAppointment);
            }

            currentDate = currentDate.AddDays(increment);
        }

        await _unitOfWork.Appointments.AddRangeAsync(appointments.Skip(1));
        await _unitOfWork.SaveChangesAsync();

        // Update student package if provided
        if (createDto.StudentPackageId.HasValue)
        {
            var studentPackage = await _unitOfWork.StudentPackages.GetByIdAsync(createDto.StudentPackageId.Value);
            if (studentPackage != null)
            {
                studentPackage.UsedLessons += appointments.Count;
                studentPackage.RemainingLessons = studentPackage.TotalLessons - studentPackage.UsedLessons;
                _unitOfWork.StudentPackages.Update(studentPackage);
                await _unitOfWork.SaveChangesAsync();
            }
        }

        return appointments.Select(a => MapToDto(a));
    }

    public async Task<AppointmentDto?> UpdateAppointmentAsync(int id, UpdateAppointmentDto updateDto)
    {
        var appointment = await _unitOfWork.Appointments.GetByIdAsync(id);
        if (appointment == null)
            return null;
        
        var hasConflict = await _unitOfWork.Appointments.HasConflictAsync(
            appointment.TeacherId,
            updateDto.ClassroomId ?? 0,
            appointment.StudentId,
            updateDto.AppointmentDate,
            updateDto.StartTime,
            updateDto.EndTime,
            id);

        if (hasConflict)
            throw new InvalidOperationException("Time slot conflict detected");
        
        appointment.AppointmentDate = updateDto.AppointmentDate;
        appointment.StartTime = updateDto.StartTime;
        appointment.EndTime = updateDto.EndTime;
        appointment.ClassroomId = updateDto.ClassroomId;
        appointment.Status = updateDto.Status;
        appointment.Notes = updateDto.Notes;
        
        _unitOfWork.Appointments.Update(appointment);
        await _unitOfWork.SaveChangesAsync();

        return await GetAppointmentByIdAsync(id);
    }

    public async Task<bool> CancelAppointmentAsync(int id, string reason)
    {
        var appointment = await _unitOfWork.Appointments.GetByIdAsync(id);
        if (appointment == null)
            return false;
        
        appointment.Status = AppointmentStatus.Cancelled;
        appointment.CancellationReason = reason;
        
        _unitOfWork.Appointments.Update(appointment);
        await _unitOfWork.SaveChangesAsync();
        
        if (appointment.StudentPackageId.HasValue)
        {
            var studentPackage = await _unitOfWork.StudentPackages.GetByIdAsync(appointment.StudentPackageId.Value);
            if (studentPackage != null && studentPackage.UsedLessons > 0)
            {
                studentPackage.UsedLessons--;
                studentPackage.RemainingLessons++;
                
                _unitOfWork.StudentPackages.Update(studentPackage);
                await _unitOfWork.SaveChangesAsync();
            }
        }

        return true;
    }

    public async Task<bool> DeleteAppointmentAsync(int id)
    {
        var appointment = await _unitOfWork.Appointments.GetByIdAsync(id);
        if (appointment == null)
            return false;
        
        _unitOfWork.Appointments.Remove(appointment);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }
    
    private async Task ValidateEntitiesAsync(int studentId, int teacherId, int courseId, int? classroomId)
    {
        var student = await _unitOfWork.Students.GetByIdAsync(studentId);
        if (student == null)
            throw new InvalidOperationException("Student not found");

        var teacher = await _unitOfWork.Teachers.GetByIdAsync(teacherId);
        if (teacher == null)
            throw new InvalidOperationException("Teacher not found");

        var course = await _unitOfWork.Courses.GetByIdAsync(courseId);
        if (course == null)
            throw new InvalidOperationException("Course not found");

        if (classroomId.HasValue)
        {
            var classroom = await _unitOfWork.Classrooms.GetByIdAsync(classroomId.Value);
            if (classroom == null)
                throw new InvalidOperationException("Classroom not found");
        }
    }
    
    private async Task UpdateStudentPackageUsageAsync(int studentPackageId)
    {
        var studentPackage = await _unitOfWork.StudentPackages.GetByIdAsync(studentPackageId);
        if (studentPackage != null)
        {
            studentPackage.UsedLessons++;
            studentPackage.RemainingLessons = studentPackage.TotalLessons - studentPackage.UsedLessons;
            
            if (studentPackage.RemainingLessons <= 0)
            {
                studentPackage.Status = StudentPackageStatus.Completed;
            }
            
            _unitOfWork.StudentPackages.Update(studentPackage);
            await _unitOfWork.SaveChangesAsync();
        }
    }
    private static AppointmentDto MapToDto(Appointment appointment)
    {
        return new AppointmentDto
        {
            Id = appointment.Id,
            StudentId = appointment.StudentId,
            StudentName = appointment.Student != null 
                ? $"{appointment.Student.FirstName} {appointment.Student.LastName}" 
                : "Unknown",
            TeacherId = appointment.TeacherId,
            TeacherName = appointment.Teacher?.User != null 
                ? $"{appointment.Teacher.User.FirstName} {appointment.Teacher.User.LastName}" 
                : "Unknown",
            CourseId = appointment.CourseId,
            CourseName = appointment.Course?.Name ?? "Unknown",
            ClassroomId = appointment.ClassroomId,
            ClassroomName = appointment.Classroom?.Name,
            AppointmentDate = appointment.AppointmentDate,
            StartTime = appointment.StartTime,
            EndTime = appointment.EndTime,
            Status = appointment.Status,
            IsRecurring = appointment.IsRecurring,
            RecurringPattern = appointment.RecurringPattern,
            RecurringEndDate = appointment.RecurringEndDate,
            Notes = appointment.Notes,
            CreatedAt = appointment.CreatedAt
        };
    }
    
}