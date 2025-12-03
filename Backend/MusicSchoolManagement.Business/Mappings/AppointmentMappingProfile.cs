using AutoMapper;
using MusicSchoolManagement.Core.DTOs.Appointments;
using MusicSchoolManagement.Core.Entities;
using MusicSchoolManagement.Core.Enums;

namespace MusicSchoolManagement.Business.Mappings;

public class AppointmentMappingProfile : Profile
{
    public AppointmentMappingProfile()
    {
        // Appointment -> AppointmentDto
        CreateMap<Appointment, AppointmentDto>()
            .ForMember(dest => dest.StudentName, opt => opt.MapFrom(src => 
                src.Student != null ? $"{src.Student.FirstName} {src.Student.LastName}" : "Unknown"))
            .ForMember(dest => dest.TeacherName, opt => opt.MapFrom(src => 
                src.Teacher != null && src.Teacher.User != null 
                    ? $"{src.Teacher.User.FirstName} {src.Teacher.User.LastName}" 
                    : "Unknown"))
            .ForMember(dest => dest.CourseName, opt => opt.MapFrom(src => 
                src.Course != null ? src.Course.Name : "Unknown"))
            .ForMember(dest => dest.ClassroomName, opt => opt.MapFrom(src => 
                src.Classroom != null ? src.Classroom.Name : null));

        // CreateAppointmentDto -> Appointment
        CreateMap<CreateAppointmentDto, Appointment>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => AppointmentStatus.Scheduled))
            .ForMember(dest => dest.IsRecurring, opt => opt.MapFrom(src => false))
            .ForMember(dest => dest.RecurringPattern, opt => opt.Ignore())
            .ForMember(dest => dest.RecurringEndDate, opt => opt.Ignore())
            .ForMember(dest => dest.ParentAppointmentId, opt => opt.Ignore())
            .ForMember(dest => dest.CancellationReason, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
            .ForMember(dest => dest.Student, opt => opt.Ignore())
            .ForMember(dest => dest.Teacher, opt => opt.Ignore())
            .ForMember(dest => dest.Course, opt => opt.Ignore())
            .ForMember(dest => dest.Classroom, opt => opt.Ignore())
            .ForMember(dest => dest.StudentPackage, opt => opt.Ignore())
            .ForMember(dest => dest.AttendanceLogs, opt => opt.Ignore())
            .ForMember(dest => dest.ParentAppointment, opt => opt.Ignore())
            .ForMember(dest => dest.ChildAppointments, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

        // UpdateAppointmentDto -> Appointment (for updating only specific fields)
        CreateMap<UpdateAppointmentDto, Appointment>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.StudentId, opt => opt.Ignore())
            .ForMember(dest => dest.TeacherId, opt => opt.Ignore())
            .ForMember(dest => dest.CourseId, opt => opt.Ignore())
            .ForMember(dest => dest.StudentPackageId, opt => opt.Ignore())
            .ForMember(dest => dest.IsRecurring, opt => opt.Ignore())
            .ForMember(dest => dest.RecurringPattern, opt => opt.Ignore())
            .ForMember(dest => dest.RecurringEndDate, opt => opt.Ignore())
            .ForMember(dest => dest.ParentAppointmentId, opt => opt.Ignore())
            .ForMember(dest => dest.CancellationReason, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
            .ForMember(dest => dest.Student, opt => opt.Ignore())
            .ForMember(dest => dest.Teacher, opt => opt.Ignore())
            .ForMember(dest => dest.Course, opt => opt.Ignore())
            .ForMember(dest => dest.Classroom, opt => opt.Ignore())
            .ForMember(dest => dest.StudentPackage, opt => opt.Ignore())
            .ForMember(dest => dest.AttendanceLogs, opt => opt.Ignore())
            .ForMember(dest => dest.ParentAppointment, opt => opt.Ignore())
            .ForMember(dest => dest.ChildAppointments, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());
    }
}