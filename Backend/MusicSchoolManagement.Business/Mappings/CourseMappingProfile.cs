using AutoMapper;
using MusicSchoolManagement.Core.DTOs.Courses;
using MusicSchoolManagement.Core.Entities;

namespace MusicSchoolManagement.Business.Mappings;

public class CourseMappingProfile : Profile
{
    public CourseMappingProfile()
    {
        // Course -> CourseDto
        CreateMap<Course, CourseDto>()
            .ForMember(dest => dest.InstrumentName, opt => opt.MapFrom(src => src.Instrument.Name));

        // CreateCourseDto -> Course
        CreateMap<CreateCourseDto, Course>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true))
            .ForMember(dest => dest.Instrument, opt => opt.Ignore())
            .ForMember(dest => dest.Appointments, opt => opt.Ignore())
            .ForMember(dest => dest.StudentPackages, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

        // UpdateCourseDto -> Course
        CreateMap<UpdateCourseDto, Course>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.InstrumentId, opt => opt.Ignore())
            .ForMember(dest => dest.Instrument, opt => opt.Ignore())
            .ForMember(dest => dest.Appointments, opt => opt.Ignore())
            .ForMember(dest => dest.StudentPackages, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());
    }
}