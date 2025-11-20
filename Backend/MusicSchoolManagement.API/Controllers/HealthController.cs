using MusicSchoolManagement.Infrastructure.Data;

namespace MusicSchoolManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HealthController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    public HealthController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new
        {
            status = "healthy",
            timestamp = DateTime.UtcNow,
            version = "1.0.0"
        });
    }

    [HttpGet("db")]
    public async Task<IActionResult> CheckDatabase()
    {
        try
        {
            var canConnect = await _context.Database.CanConnectAsync();
            return Ok(new
            {
                database = canConnect ? "connected" : "disconnected",
                timestamp = DateTime.UtcNow
            });
        }
        catch (Exception e)
        {
            return StatusCode(500, new
            {
                database = "error",
                message = e.Message,
                timestamp = DateTime.UtcNow
            });
        }
    }
}