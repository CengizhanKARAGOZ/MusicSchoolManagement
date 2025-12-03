using AutoMapper;
using MusicSchoolManagement.Core.DTOs.Packages;
using MusicSchoolManagement.Core.Entities;
using MusicSchoolManagement.Core.Enums;

namespace MusicSchoolManagement.Business.Mappings;

public class PackageMappingProfile : Profile
{
    public PackageMappingProfile()
    {
        // Package -> PackageDto
        CreateMap<Package, PackageDto>();

        // CreatePackageDto -> Package
        CreateMap<CreatePackageDto, Package>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.TotalLessons, opt => opt.MapFrom(src => src.DurationMonths * src.LessonsPerMonth))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true))
            .ForMember(dest => dest.StudentPackages, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

        // UpdatePackageDto -> Package
        CreateMap<UpdatePackageDto, Package>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.TotalLessons, opt => opt.MapFrom(src => src.DurationMonths * src.LessonsPerMonth))
            .ForMember(dest => dest.StudentPackages, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

        // StudentPackage -> StudentPackageDto
        CreateMap<StudentPackage, StudentPackageDto>()
            .ForMember(dest => dest.StudentName, opt => opt.MapFrom(src => 
                src.Student != null ? $"{src.Student.FirstName} {src.Student.LastName}" : "Unknown"))
            .ForMember(dest => dest.PackageName, opt => opt.MapFrom(src => 
                src.Package != null ? src.Package.Name : "Unknown"))
            .ForMember(dest => dest.CourseName, opt => opt.MapFrom(src => 
                src.Course != null ? src.Course.Name : "Unknown"));

        // AssignPackageDto -> StudentPackage
        CreateMap<AssignPackageDto, StudentPackage>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.EndDate, opt => opt.Ignore())
            .ForMember(dest => dest.TotalLessons, opt => opt.Ignore())
            .ForMember(dest => dest.UsedLessons, opt => opt.MapFrom(src => 0))
            .ForMember(dest => dest.RemainingLessons, opt => opt.Ignore())
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => StudentPackageStatus.Active))
            .ForMember(dest => dest.PurchaseDate, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.Student, opt => opt.Ignore())
            .ForMember(dest => dest.Package, opt => opt.Ignore())
            .ForMember(dest => dest.Course, opt => opt.Ignore())
            .ForMember(dest => dest.Payments, opt => opt.Ignore())
            .ForMember(dest => dest.Appointments, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());
    }
}