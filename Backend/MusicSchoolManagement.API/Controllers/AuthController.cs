using System.Security.Claims;
using MusicSchoolManagement.Core.DTOs.Auth;
using MusicSchoolManagement.Core.Helpers;
using MusicSchoolManagement.Core.Interfaces.Services;
using MusicSchoolManagement.Infrastructure.Data;

namespace MusicSchoolManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly ApplicationDbContext _context;
    public AuthController(IAuthService authService, ApplicationDbContext context)
    {
        _authService = authService;
        _context = context;
    }

    /// <summary>
    /// User login
    /// </summary>
    [HttpPost("login")]
    [ProducesResponseType(typeof(ApiResponse<LoginResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<LoginResponseDto>), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiResponse<LoginResponseDto>), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto loginDto)
    {
        var response = await _authService.LoginAsync(loginDto);
        return Ok(ApiResponse<LoginResponseDto>.SuccessResponse(response, "Login successful"));
    }

    /// <summary>
    /// User registration
    /// </summary>
    [HttpPost("register")]
    [ProducesResponseType(typeof(ApiResponse<LoginResponseDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<LoginResponseDto>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<LoginResponseDto>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerDto)
    {
        var response = await _authService.RegisterAsync(registerDto);
        return CreatedAtAction(nameof(Login), 
            ApiResponse<LoginResponseDto>.SuccessResponse(response, "Registration successful"));
    }
    
    [HttpPost("change-password")]
    [Authorize]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim))
        {
            return Unauthorized(ApiResponse<object>.ErrorResponse("User not authenticated"));
        }

        var userId = int.Parse(userIdClaim);
        var user = await _context.Users.FindAsync(userId);

        if (user == null)
        {
            return NotFound(ApiResponse<object>.ErrorResponse("User not found"));
        }

        if (!PasswordHelper.VerifyPassword(dto.OldPassword, user.PasswordHash))
        {
            return BadRequest(ApiResponse<object>.ErrorResponse("Current password is incorrect"));
        }

        user.PasswordHash = PasswordHelper.HashPassword(dto.NewPassword);
        user.PasswordChangeRequired = false;
        user.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return Ok(ApiResponse<object>.SuccessResponse(null, "Password changed successfully"));
    }
}