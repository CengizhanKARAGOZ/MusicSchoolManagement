using AutoMapper;
using MusicSchoolManagement.Core.DTOs.Teachers;
using MusicSchoolManagement.Core.DTOs.Users;
using MusicSchoolManagement.Core.Enitties;
using MusicSchoolManagement.Core.Entities;

namespace MusicSchoolManagement.Business.Mappings;

public class TeacherMappingProfile : Profile
{
    public TeacherMappingProfile()
    {
        // User -> UserDto
        CreateMap<User, UserDto>();

        // Teacher -> TeacherDto (with nested User object)
        CreateMap<Teacher, TeacherDto>()
            .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User));

        // CreateTeacherDto -> Teacher
        CreateMap<CreateTeacherDto, Teacher>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.User, opt => opt.Ignore())
            .ForMember(dest => dest.Appointments, opt => opt.Ignore())
            .ForMember(dest => dest.Availabilities, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

        // UpdateTeacherDto -> Teacher
        CreateMap<UpdateTeacherDto, Teacher>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.UserId, opt => opt.Ignore())
            .ForMember(dest => dest.User, opt => opt.Ignore())
            .ForMember(dest => dest.Appointments, opt => opt.Ignore())
            .ForMember(dest => dest.Availabilities, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());
        
        // CreateTeacherWithUserDto -> Teacher
        CreateMap<CreateTeacherWithUserDto, Teacher>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.UserId, opt => opt.Ignore())
            .ForMember(dest => dest.User, opt => opt.Ignore())
            .ForMember(dest => dest.Appointments, opt => opt.Ignore())
            .ForMember(dest => dest.Availabilities, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());
    }
}