using MusicSchoolManagement.Core.DTOs.Teachers;
using MusicSchoolManagement.Core.Interfaces.Services;

namespace MusicSchoolManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TeachersController : ControllerBase
{
    private readonly ITeacherService _teacherService;

    public TeachersController(ITeacherService teacherService)
    {
        _teacherService = teacherService;
    }

    /// <summary>
    /// Get all teachers
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<TeacherDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var teachers = await _teacherService.GetAllTeachersAsync();
        return Ok(ApiResponse<IEnumerable<TeacherDto>>.SuccessResponse(teachers));
    }

    /// <summary>
    /// Get teacher by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponse<TeacherDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<TeacherDto>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var teacher = await _teacherService.GetTeacherByIdAsync(id);
        return Ok(ApiResponse<TeacherDto>.SuccessResponse(teacher));
    }

    /// <summary>
    /// Get teacher by user ID
    /// </summary>
    [HttpGet("user/{userId}")]
    [ProducesResponseType(typeof(ApiResponse<TeacherDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<TeacherDto>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByUserId(int userId)
    {
        var teacher = await _teacherService.GetTeacherByUserIdAsync(userId);
        return Ok(ApiResponse<TeacherDto>.SuccessResponse(teacher));
    }

    /// <summary>
    /// Create new teacher
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ApiResponse<TeacherDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<TeacherDto>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<TeacherDto>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<TeacherDto>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create([FromBody] CreateTeacherDto createDto)
    {
        var teacher = await _teacherService.CreateTeacherAsync(createDto);
        return CreatedAtAction(nameof(GetById), new { id = teacher.Id }, 
            ApiResponse<TeacherDto>.SuccessResponse(teacher, "Teacher created successfully"));
    }

    /// <summary>
    /// Update teacher
    /// </summary>
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ApiResponse<TeacherDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<TeacherDto>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<TeacherDto>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateTeacherDto updateDto)
    {
        var teacher = await _teacherService.UpdateTeacherAsync(id, updateDto);
        return Ok(ApiResponse<TeacherDto>.SuccessResponse(teacher, "Teacher updated successfully"));
    }

    /// <summary>
    /// Delete teacher
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        await _teacherService.DeleteTeacherAsync(id);
        return NoContent();
    }
}