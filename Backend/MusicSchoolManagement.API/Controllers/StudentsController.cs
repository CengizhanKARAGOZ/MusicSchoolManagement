using MusicSchoolManagement.Core.DTOs.Students;
using MusicSchoolManagement.Core.Interfaces.Services;

namespace MusicSchoolManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class StudentsController : ControllerBase
{
    private readonly IStudentService _studentService;

    public StudentsController(IStudentService studentService)
    {
        _studentService = studentService;
    }

    /// <summary>
    /// Get all students
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<StudentDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var students = await _studentService.GetAllStudentsAsync();
        return Ok(ApiResponse<IEnumerable<StudentDto>>.SuccessResponse(students));
    }

    /// <summary>
    /// Get active students
    /// </summary>
    [HttpGet("active")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<StudentDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetActive()
    {
        var students = await _studentService.GetActiveStudentsAsync();
        return Ok(ApiResponse<IEnumerable<StudentDto>>.SuccessResponse(students));
    }

    /// <summary>
    /// Get student by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponse<StudentDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<StudentDto>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var student = await _studentService.GetStudentByIdAsync(id);
        return Ok(ApiResponse<StudentDto>.SuccessResponse(student));
    }

    /// <summary>
    /// Create new student
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ApiResponse<StudentDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<StudentDto>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateStudentDto createDto)
    {
        var student = await _studentService.CreateStudentAsync(createDto);
        return CreatedAtAction(nameof(GetById), new { id = student.Id }, 
            ApiResponse<StudentDto>.SuccessResponse(student, "Student created successfully"));
    }

    /// <summary>
    /// Update student
    /// </summary>
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ApiResponse<StudentDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<StudentDto>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<StudentDto>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateStudentDto updateDto)
    {
        var student = await _studentService.UpdateStudentAsync(id, updateDto);
        return Ok(ApiResponse<StudentDto>.SuccessResponse(student, "Student updated successfully"));
    }

    /// <summary>
    /// Delete student
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        await _studentService.DeleteStudentAsync(id);
        return NoContent();
    }
}