using AutoMapper;
using MusicSchoolManagement.Business.Helpers;
using MusicSchoolManagement.Core.DTOs.Auth;
using MusicSchoolManagement.Core.Enitties;
using MusicSchoolManagement.Core.Exceptions;
using MusicSchoolManagement.Core.Helpers;
using MusicSchoolManagement.Core.Interfaces.Repositories;
using MusicSchoolManagement.Core.Interfaces.Services;

namespace MusicSchoolManagement.Business.Services;

public class AuthService : IAuthService
{
    #region Fields

    private readonly IUnitOfWork _unitOfWork;
    private readonly JwtHelper _jwtHelper;
    private readonly IMapper _mapper;

    #endregion

    #region Constructor

    public AuthService(IUnitOfWork unitOfWork, JwtHelper jwtHelper, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _jwtHelper = jwtHelper;
        _mapper = mapper;
    }

    #endregion

    #region Public Methods

    public async Task<LoginResponseDto?> LoginAsync(LoginRequestDto loginDto)
    {
        var user = await _unitOfWork.Users.GetByEmailAsync(loginDto.Email);
        
        if (user == null)
            throw new UnauthorizedException("Invalid email or password");

        if (!user.IsActive)
            throw new ForbiddenException("User account is inactive");

        if (!PasswordHelper.VerifyPassword(loginDto.Password, user.PasswordHash))
            throw new UnauthorizedException("Invalid email or password");

        var token = _jwtHelper.GenerateToken(user);
        var refreshToken = _jwtHelper.GenerateRefreshToken();

        var response = _mapper.Map<LoginResponseDto>(user);
        response.Token = token;
        response.RefreshToken = refreshToken;
        response.ExpiresAt = DateTime.UtcNow.AddMinutes(60);

        return response;
    }

    public async Task<LoginResponseDto?> RegisterAsync(RegisterRequestDto registerDto)
    {
        var existingUser = await _unitOfWork.Users.GetByEmailAsync(registerDto.Email);
        if (existingUser != null)
            throw new ConflictException($"User with email '{registerDto.Email}' already exists");

        var user = _mapper.Map<User>(registerDto);
        user.PasswordHash = PasswordHelper.HashPassword(registerDto.Password);
        user.IsActive = true;

        await _unitOfWork.Users.AddAsync(user);
        await _unitOfWork.SaveChangesAsync();

        var token = _jwtHelper.GenerateToken(user);
        var refreshToken = _jwtHelper.GenerateRefreshToken();

        var response = _mapper.Map<LoginResponseDto>(user);
        response.Token = token;
        response.RefreshToken = refreshToken;
        response.ExpiresAt = DateTime.UtcNow.AddMinutes(60);

        return response;
    }

    #endregion
}