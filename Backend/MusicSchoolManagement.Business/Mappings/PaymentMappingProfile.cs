using AutoMapper;
using MusicSchoolManagement.Core.DTOs.Payments;
using MusicSchoolManagement.Core.Entities;

namespace MusicSchoolManagement.Business.Mappings;

public class PaymentMappingProfile : Profile
{
    public PaymentMappingProfile()
    {
        // Payment -> PaymentDto
        CreateMap<Payment, PaymentDto>()
            .ForMember(dest => dest.StudentName, opt => opt.MapFrom(src => 
                src.Student != null ? $"{src.Student.FirstName} {src.Student.LastName}" : "Unknown"))
            .ForMember(dest => dest.PackageName, opt => opt.MapFrom(src => 
                src.StudentPackage != null && src.StudentPackage.Package != null 
                    ? src.StudentPackage.Package.Name 
                    : null));

        // CreatePaymentDto -> Payment
        CreateMap<CreatePaymentDto, Payment>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
            .ForMember(dest => dest.Student, opt => opt.Ignore())
            .ForMember(dest => dest.StudentPackage, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());
    }
}