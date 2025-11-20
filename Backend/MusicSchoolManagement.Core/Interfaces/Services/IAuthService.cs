using MusicSchoolManagement.Core.DTOs.Auth;

namespace MusicSchoolManagement.Core.Interfaces.Services;

public interface IAuthService
{
    Task<LoginResponseDto?> LoginAsync(LoginRequestDto loginDto);
    Task<LoginResponseDto?> RegisterAsync(RegisterRequestDto registerDto);
}