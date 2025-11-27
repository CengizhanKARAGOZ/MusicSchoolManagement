using MusicSchoolManagement.Core.DTOs.Courses;
using MusicSchoolManagement.Core.Interfaces.Services;

namespace MusicSchoolManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CoursesController : ControllerBase
{
    private readonly ICourseService _courseService;

    public CoursesController(ICourseService courseService)
    {
        _courseService = courseService;
    }

    /// <summary>
    /// Get all courses
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<CourseDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var courses = await _courseService.GetAllCoursesAsync();
        return Ok(ApiResponse<IEnumerable<CourseDto>>.SuccessResponse(courses));
    }

    /// <summary>
    /// Get active courses only
    /// </summary>
    [HttpGet("active")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<CourseDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetActive()
    {
        var courses = await _courseService.GetActiveCoursesAsync();
        return Ok(ApiResponse<IEnumerable<CourseDto>>.SuccessResponse(courses));
    }

    /// <summary>
    /// Get courses by instrument
    /// </summary>
    [HttpGet("instrument/{instrumentId}")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<CourseDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByInstrument(int instrumentId)
    {
        var courses = await _courseService.GetCoursesByInstrumentAsync(instrumentId);
        return Ok(ApiResponse<IEnumerable<CourseDto>>.SuccessResponse(courses));
    }

    /// <summary>
    /// Get course by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponse<CourseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<CourseDto>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var course = await _courseService.GetCourseByIdAsync(id);
        
        if (course == null)
            return NotFound(ApiResponse<CourseDto>.ErrorResponse("Course not found"));
        
        return Ok(ApiResponse<CourseDto>.SuccessResponse(course));
    }
    
    /// <summary>
    /// Create new course
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ApiResponse<CourseDto>), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreateCourseDto createDto)
    {
        var course = await _courseService.CreateCourseAsync(createDto);
        return CreatedAtAction(nameof(GetById), new { id = course.Id }, 
            ApiResponse<CourseDto>.SuccessResponse(course, "Course created successfully"));
    }

    /// <summary>
    /// Update course
    /// </summary>
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ApiResponse<CourseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<CourseDto>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateCourseDto updateDto)
    {
        var course = await _courseService.UpdateCourseAsync(id, updateDto);

        if (course == null)
            return NotFound(ApiResponse<CourseDto>.ErrorResponse("Course not found"));

        return Ok(ApiResponse<CourseDto>.SuccessResponse(course, "Course updated successfully"));
    }
    
    /// <summary>
    /// Delete course
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _courseService.DeleteCourseAsync(id);
        
        if (!success)
            return NotFound(ApiResponse<object>.ErrorResponse("Course not found"));
        
        return NoContent();
    }
}