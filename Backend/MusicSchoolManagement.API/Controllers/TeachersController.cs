using MusicSchoolManagement.Core.DTOs.Teachers;
using MusicSchoolManagement.Core.DTOs.Common;
using MusicSchoolManagement.Core.Interfaces.Services;
using MusicSchoolManagement.Core.Helpers;
using MusicSchoolManagement.Core.Entities;
using MusicSchoolManagement.Core.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using MusicSchoolManagement.Core.Enitties;
using MusicSchoolManagement.Infrastructure.Data;

namespace MusicSchoolManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TeachersController : ControllerBase
{
    private readonly ITeacherService _teacherService;
    private readonly ApplicationDbContext _context;

    public TeachersController(ITeacherService teacherService, ApplicationDbContext context)
    {
        _teacherService = teacherService;
        _context = context;
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
    /// Create new teacher with user account (Admin only)
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CreateTeacherWithUser([FromBody] CreateTeacherWithUserDto dto)
    {
        if (await _context.Users.AnyAsync(u => u.Email == dto.Email))
        {
            return BadRequest(ApiResponse<object>.ErrorResponse("A user with this email already exists"));
        }

        var temporaryPassword = PasswordGenerator.GenerateStrongPassword(12);

        var user = new User
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            PhoneNumber = dto.PhoneNumber,
            PasswordHash = PasswordHelper.HashPassword(temporaryPassword),
            Role = UserRole.Teacher,
            IsActive = true,
            PasswordChangeRequired = true,
            CreatedAt = DateTime.UtcNow
        };

        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        // 2. Create Teacher
        var teacher = new Teacher
        {
            UserId = user.Id,
            Specializations = dto.Specializations,
            HourlyRate = dto.HourlyRate,
            Biography = dto.Biography,
            AvailabilityNotes = dto.AvailabilityNotes,
            CreatedAt = DateTime.UtcNow
        };

        await _context.Teachers.AddAsync(teacher);
        await _context.SaveChangesAsync();

        // TODO: Send email with temporary password
        // await _emailService.SendWelcomeEmailAsync(user.Email, temporaryPassword);

        return Created(string.Empty, ApiResponse<object>.SuccessResponse(new
        {
            UserId = user.Id,
            TeacherId = teacher.Id,
            Email = user.Email,
            TemporaryPassword = temporaryPassword,
            Note = "Please share this password securely with the teacher. They will be required to change it on first login."
        }, "Teacher created successfully. Temporary password has been generated."));
    }

    /// <summary>
    /// Update teacher details
    /// </summary>
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Teacher")]
    [ProducesResponseType(typeof(ApiResponse<TeacherDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<TeacherDto>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<TeacherDto>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateTeacherDto updateDto)
    {
        var teacher = await _teacherService.UpdateTeacherAsync(id, updateDto);
        return Ok(ApiResponse<TeacherDto>.SuccessResponse(teacher, "Teacher updated successfully"));
    }

    /// <summary>
    /// Delete teacher (Admin only)
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