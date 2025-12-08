using AutoMapper;
using MusicSchoolManagement.Core.DTOs.Auth;
using MusicSchoolManagement.Core.Enitties;
using MusicSchoolManagement.Core.Entities;

namespace MusicSchoolManagement.Business.Mappings;

public class AuthMappingProfile : Profile
{
    public AuthMappingProfile()
    {
        // User -> LoginResponseDto
        CreateMap<User, LoginResponseDto>()
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Token, opt => opt.Ignore())
            .ForMember(dest => dest.RefreshToken, opt => opt.Ignore())
            .ForMember(dest => dest.ExpiresAt, opt => opt.Ignore());
        
        // RegisterRequestDto -> User
        CreateMap<RegisterRequestDto, User>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true))
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());
    }
}