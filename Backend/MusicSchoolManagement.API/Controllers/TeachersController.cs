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
        
        if (teacher == null)
            return NotFound(ApiResponse<TeacherDto>.ErrorResponse("Teacher not found"));

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
        
        if (teacher == null)
            return NotFound(ApiResponse<TeacherDto>.ErrorResponse("Teacher not found"));

        return Ok(ApiResponse<TeacherDto>.SuccessResponse(teacher));
    }

    /// <summary>
    /// Create teacher profile for existing user
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ApiResponse<TeacherDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<TeacherDto>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateTeacherDto createDto)
    {
        try
        {
            var teacher = await _teacherService.CreateTeacherAsync(createDto);
            return CreatedAtAction(nameof(GetById), new { id = teacher.Id }, 
                ApiResponse<TeacherDto>.SuccessResponse(teacher, "Teacher profile created successfully"));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ApiResponse<TeacherDto>.ErrorResponse(ex.Message));
        }
    }

    /// <summary>
    /// Update teacher profile
    /// </summary>
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ApiResponse<TeacherDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<TeacherDto>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateTeacherDto updateDto)
    {
        var teacher = await _teacherService.UpdateTeacherAsync(id, updateDto);
        
        if (teacher == null)
            return NotFound(ApiResponse<TeacherDto>.ErrorResponse("Teacher not found"));

        return Ok(ApiResponse<TeacherDto>.SuccessResponse(teacher, "Teacher updated successfully"));
    }

    /// <summary>
    /// Delete teacher profile
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _teacherService.DeleteTeacherAsync(id);
        
        if (!result)
            return NotFound(ApiResponse<object>.ErrorResponse("Teacher not found"));

        return NoContent();
    }
}