using MusicSchoolManagement.Core.DTOs.Common;
using MusicSchoolManagement.Core.Interfaces.Services;

namespace MusicSchoolManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ClassroomsController : ControllerBase
{
    private readonly IClassroomService _classroomService;

    public ClassroomsController(IClassroomService classroomService)
    {
        _classroomService = classroomService;
    }

    /// <summary>
    /// Get all classrooms
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<ClassroomDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var classrooms = await _classroomService.GetAllClassroomsAsync();
        return Ok(ApiResponse<IEnumerable<ClassroomDto>>.SuccessResponse(classrooms));
    }

    /// <summary>
    /// Get active classrooms
    /// </summary>
    [HttpGet("active")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<ClassroomDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetActive()
    {
        var classrooms = await _classroomService.GetActiveClassroomsAsync();
        return Ok(ApiResponse<IEnumerable<ClassroomDto>>.SuccessResponse(classrooms));
    }

    /// <summary>
    /// Get available classrooms for date/time
    /// </summary>
    [HttpGet("available")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<ClassroomDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAvailable([FromQuery] DateTime date, [FromQuery] TimeSpan startTime, [FromQuery] TimeSpan endTime)
    {
        var classrooms = await _classroomService.GetAvailableClassroomsAsync(date, startTime, endTime);
        return Ok(ApiResponse<IEnumerable<ClassroomDto>>.SuccessResponse(classrooms));
    }

    /// <summary>
    /// Get classroom by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponse<ClassroomDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<ClassroomDto>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var classroom = await _classroomService.GetClassroomByIdAsync(id);
        return Ok(ApiResponse<ClassroomDto>.SuccessResponse(classroom));
    }

    /// <summary>
    /// Create new classroom
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ApiResponse<ClassroomDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<ClassroomDto>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateClassroomRequest request)
    {
        var classroom = await _classroomService.CreateClassroomAsync(
            request.Name, 
            request.RoomNumber, 
            request.Capacity, 
            request.SuitableInstruments, 
            request.Equipment);
        
        return CreatedAtAction(nameof(GetById), new { id = classroom.Id }, 
            ApiResponse<ClassroomDto>.SuccessResponse(classroom, "Classroom created successfully"));
    }

    /// <summary>
    /// Update classroom
    /// </summary>
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ApiResponse<ClassroomDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<ClassroomDto>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<ClassroomDto>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateClassroomRequest request)
    {
        var classroom = await _classroomService.UpdateClassroomAsync(
            id, 
            request.Name, 
            request.RoomNumber, 
            request.Capacity, 
            request.SuitableInstruments, 
            request.Equipment, 
            request.IsActive);
        
        return Ok(ApiResponse<ClassroomDto>.SuccessResponse(classroom, "Classroom updated successfully"));
    }

    /// <summary>
    /// Delete classroom
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        await _classroomService.DeleteClassroomAsync(id);
        return NoContent();
    }
}

// Request models
public class CreateClassroomRequest
{
    public string Name { get; set; } = string.Empty;
    public string? RoomNumber { get; set; }
    public int Capacity { get; set; }
    public string? SuitableInstruments { get; set; }
    public string? Equipment { get; set; }
}

public class UpdateClassroomRequest
{
    public string Name { get; set; } = string.Empty;
    public string? RoomNumber { get; set; }
    public int Capacity { get; set; }
    public string? SuitableInstruments { get; set; }
    public string? Equipment { get; set; }
    public bool IsActive { get; set; }
}