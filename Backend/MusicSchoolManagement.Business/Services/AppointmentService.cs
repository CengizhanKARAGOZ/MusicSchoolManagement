using AutoMapper;
using MusicSchoolManagement.Core.DTOs.Appointments;
using MusicSchoolManagement.Core.Entities;
using MusicSchoolManagement.Core.Enums;
using MusicSchoolManagement.Core.Interfaces.Repositories;
using MusicSchoolManagement.Core.Interfaces.Services;

namespace MusicSchoolManagement.Business.Services;

public class AppointmentService : IAppointmentService
{
    #region Fields

    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    #endregion

    #region Constructor

    public AppointmentService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    #endregion

    #region Public Methods

    public async Task<IEnumerable<AppointmentDto>> GetAllAppointmentsAsync()
    {
        var appointments = await _unitOfWork.Appointments.GetAllAsync();
        return _mapper.Map<IEnumerable<AppointmentDto>>(appointments);
    }

    public async Task<IEnumerable<AppointmentDto>> GetAppointmentsByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        var appointments = await _unitOfWork.Appointments.GetByDateRangeAsync(startDate, endDate);
        return _mapper.Map<IEnumerable<AppointmentDto>>(appointments);
    }

    public async Task<IEnumerable<AppointmentDto>> GetAppointmentsByTeacherAsync(int teacherId, DateTime? date = null)
    {
        var appointments = await _unitOfWork.Appointments.GetByTeacherIdAsync(teacherId, date);
        return _mapper.Map<IEnumerable<AppointmentDto>>(appointments);
    }

    public async Task<IEnumerable<AppointmentDto>> GetAppointmentsByStudentAsync(int studentId, DateTime? date = null)
    {
        var appointments = await _unitOfWork.Appointments.GetByStudentIdAsync(studentId, date);
        return _mapper.Map<IEnumerable<AppointmentDto>>(appointments);
    }

    public async Task<IEnumerable<AppointmentDto>> GetUpcomingAppointmentsAsync(int count)
    {
        var appointments = await _unitOfWork.Appointments.GetUpcomingAppointmentsAsync(count);
        return _mapper.Map<IEnumerable<AppointmentDto>>(appointments);
    }

    public async Task<AppointmentDto?> GetAppointmentByIdAsync(int id)
    {
        var appointment = await _unitOfWork.Appointments.GetWithDetailsAsync(id);
        return appointment == null ? null : _mapper.Map<AppointmentDto>(appointment);
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
        
        var appointment = _mapper.Map<Appointment>(createDto);
        appointment.Status = AppointmentStatus.Scheduled;
        appointment.IsRecurring = false;
        appointment.CreatedBy = createdByUserId;
        
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

        return _mapper.Map<IEnumerable<AppointmentDto>>(appointments);
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
        
        _mapper.Map(updateDto, appointment);
        
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

    #endregion

    #region Private Methods

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

    #endregion
}