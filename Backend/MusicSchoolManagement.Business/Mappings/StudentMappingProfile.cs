using AutoMapper;
using MusicSchoolManagement.Core.DTOs.Students;
using MusicSchoolManagement.Core.Enitties;

namespace MusicSchoolManagement.Business.Mappings;

public class StudentMappingProfile : Profile
{
    public StudentMappingProfile()
    {
        // Student -> StudentDto
        CreateMap<Student, StudentDto>();
        
        // CreateStudentDto -> Student
        CreateMap<CreateStudentDto, Student>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true))
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.StudentPackages, opt => opt.Ignore())
            .ForMember(dest => dest.Appointments, opt => opt.Ignore())
            .ForMember(dest => dest.Payments, opt => opt.Ignore());
        
        // UpdateStudentDto -> Student
        CreateMap<UpdateStudentDto, Student>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.RegistrationDate, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.StudentPackages, opt => opt.Ignore())
            .ForMember(dest => dest.Appointments, opt => opt.Ignore())
            .ForMember(dest => dest.Payments, opt => opt.Ignore());
    }
}