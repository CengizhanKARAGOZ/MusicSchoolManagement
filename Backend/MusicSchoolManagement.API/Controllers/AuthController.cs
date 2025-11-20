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
    /// User login - returns JWT token
    /// </summary>
    [HttpPost("login")]
    [ProducesResponseType(typeof(ApiResponse<LoginResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<LoginResponseDto>), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto loginDto)
    {
        var result = await _authService.LoginAsync(loginDto);

        if (result == null)
            return Unauthorized(ApiResponse<LoginResponseDto>.ErrorResponse("Invalid email or password"));

        return Ok(ApiResponse<LoginResponseDto>.SuccessResponse(result, "Login successful"));
    }

    /// <summary>
    /// Register new user (Admin/Teacher)
    /// </summary>
    [HttpPost("register")]
    [ProducesResponseType(typeof(ApiResponse<LoginResponseDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<LoginResponseDto>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerDto)
    {
        var result = await _authService.RegisterAsync(registerDto);

        if (result == null)
            return BadRequest(ApiResponse<LoginResponseDto>.ErrorResponse("Email already exists"));

        return CreatedAtAction(nameof(Register), ApiResponse<LoginResponseDto>.SuccessResponse(result, "Registration successful"));
    }

    /// <summary>
    /// Test endpoint to check if authenticated
    /// </summary>
    [Authorize]
    [HttpGet("me")]
    public IActionResult GetCurrentUser()
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        var email = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
        var role = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;

        return Ok(ApiResponse<object>.SuccessResponse(new
        {
            userId,
            email,
            role
        }));
    }
}