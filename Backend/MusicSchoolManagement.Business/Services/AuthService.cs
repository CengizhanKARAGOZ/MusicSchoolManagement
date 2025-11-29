using MusicSchoolManagement.Business.Helpers;
using MusicSchoolManagement.Core.DTOs.Auth;
using MusicSchoolManagement.Core.Enitties;
using MusicSchoolManagement.Core.Helpers;
using MusicSchoolManagement.Core.Interfaces.Repositories;
using MusicSchoolManagement.Core.Interfaces.Services;

namespace MusicSchoolManagement.Business.Services;

public class AuthService : IAuthService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly JwtHelper _jwtHelper;


    public AuthService(IUnitOfWork unitOfWork, JwtHelper jwtHelper)
    {
        _unitOfWork = unitOfWork;
        _jwtHelper = jwtHelper;
    }

public async Task<LoginResponseDto?> LoginAsync(LoginRequestDto loginDto)
    {
        // Find user by email
        var user = await _unitOfWork.Users.GetByEmailAsync(loginDto.Email);
        
        if (user == null || !user.IsActive)
            return null;

        // Verify password
        if (!PasswordHelper.VerifyPassword(loginDto.Password, user.PasswordHash))
            return null;

        // Generate tokens
        var token = _jwtHelper.GenerateToken(user);
        var refreshToken = _jwtHelper.GenerateRefreshToken();

        return new LoginResponseDto
        {
            UserId = user.Id,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Role = user.Role,
            Token = token,
            RefreshToken = refreshToken,
            ExpiresAt = DateTime.UtcNow.AddMinutes(60)
        };
    }

    public async Task<LoginResponseDto?> RegisterAsync(RegisterRequestDto registerDto)
    {
        var existingUser = await _unitOfWork.Users.GetByEmailAsync(registerDto.Email);
        if (existingUser != null)
            return null;

        var user = new User
        {
            FirstName = registerDto.FirstName,
            LastName = registerDto.LastName,
            Email = registerDto.Email,
            PhoneNumber = registerDto.PhoneNumber,
            PasswordHash = PasswordHelper.HashPassword(registerDto.Password),
            Role = registerDto.Role,
            IsActive = true
        };

        await _unitOfWork.Users.AddAsync(user);
        await _unitOfWork.SaveChangesAsync();

        var token = _jwtHelper.GenerateToken(user);
        var refreshToken = _jwtHelper.GenerateRefreshToken();

        return new LoginResponseDto
        {
            UserId = user.Id,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Role = user.Role,
            Token = token,
            RefreshToken = refreshToken,
            ExpiresAt = DateTime.UtcNow.AddMinutes(60)
        };
    }
}