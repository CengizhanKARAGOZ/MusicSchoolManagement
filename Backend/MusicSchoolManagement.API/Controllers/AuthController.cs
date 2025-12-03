using MusicSchoolManagement.Core.DTOs.Auth;
using MusicSchoolManagement.Core.Interfaces.Services;

namespace MusicSchoolManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
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
}