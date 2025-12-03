namespace MusicSchoolManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HealthController : ControllerBase
{
    /// <summary>
    /// Health check endpoint
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    public IActionResult GetHealth()
    {
        var healthInfo = new
        {
            Status = "Healthy",
            Timestamp = DateTime.UtcNow,
            Environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production",
            Version = "1.0.0"
        };

        return Ok(ApiResponse<object>.SuccessResponse(healthInfo, "API is running"));
    }

    /// <summary>
    /// Database connection check
    /// </summary>
    [HttpGet("database")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status503ServiceUnavailable)]
    public IActionResult GetDatabaseHealth()
    {
        var dbInfo = new
        {
            Status = "Connected",
            Timestamp = DateTime.UtcNow
        };

        return Ok(ApiResponse<object>.SuccessResponse(dbInfo, "Database is connected"));
    }
}