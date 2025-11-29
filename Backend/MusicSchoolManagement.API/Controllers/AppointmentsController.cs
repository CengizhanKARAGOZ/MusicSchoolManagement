using System.Security.Claims;
using MusicSchoolManagement.Core.DTOs.Appointments;
using MusicSchoolManagement.Core.Interfaces.Services;

namespace MusicSchoolManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AppointmentsController : ControllerBase
{
    private readonly IAppointmentService _appointmentService;

    public AppointmentsController(IAppointmentService appointmentService)
    {
        _appointmentService = appointmentService;
    }

    /// <summary>
    /// Get all appointments (current month +/- 1)
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<AppointmentDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var appointments = await _appointmentService.GetAllAppointmentsAsync();
        return Ok(ApiResponse<IEnumerable<AppointmentDto>>.SuccessResponse(appointments));
    }
    
    /// <summary>
    /// Get appointments by date range
    /// </summary>
    [HttpGet("range")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<AppointmentDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByDateRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
    {
        var appointments = await _appointmentService.GetAppointmentsByDateRangeAsync(startDate, endDate);
        return Ok(ApiResponse<IEnumerable<AppointmentDto>>.SuccessResponse(appointments));
    }

    /// <summary>
    /// Get teacher's appointments
    /// </summary>
    [HttpGet("teacher/{teacherId}")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<AppointmentDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByTeacher(int teacherId, [FromQuery] DateTime? date = null)
    {
        var appointments = await _appointmentService.GetAppointmentsByTeacherAsync(teacherId, date);
        return Ok(ApiResponse<IEnumerable<AppointmentDto>>.SuccessResponse(appointments));
    }

    /// <summary>
    /// Get student's appointments
    /// </summary>
    [HttpGet("student/{studentId}")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<AppointmentDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByStudent(int studentId, [FromQuery] DateTime? date = null)
    {
        var appointments = await _appointmentService.GetAppointmentsByStudentAsync(studentId, date);
        return Ok(ApiResponse<IEnumerable<AppointmentDto>>.SuccessResponse(appointments));
    }

    /// <summary>
    /// Get upcoming appointments
    /// </summary>
    [HttpGet("upcoming")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<AppointmentDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUpcoming([FromQuery] int count = 10)
    {
        var appointments = await _appointmentService.GetUpcomingAppointmentsAsync(count);
        return Ok(ApiResponse<IEnumerable<AppointmentDto>>.SuccessResponse(appointments));
    }

    /// <summary>
    /// Get appointment by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponse<AppointmentDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<AppointmentDto>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var appointment = await _appointmentService.GetAppointmentByIdAsync(id);
        
        if (appointment == null)
            return NotFound(ApiResponse<AppointmentDto>.ErrorResponse("Appointment not found"));

        return Ok(ApiResponse<AppointmentDto>.SuccessResponse(appointment));
    }

    /// <summary>
    /// Create single appointment
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Admin,Teacher")]
    [ProducesResponseType(typeof(ApiResponse<AppointmentDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<AppointmentDto>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateAppointmentDto createDto)
    {
        try
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var appointment = await _appointmentService.CreateAppointmentAsync(createDto, userId);
            
            return CreatedAtAction(nameof(GetById), new { id = appointment.Id }, 
                ApiResponse<AppointmentDto>.SuccessResponse(appointment, "Appointment created successfully"));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ApiResponse<AppointmentDto>.ErrorResponse(ex.Message));
        }
    }

    /// <summary>
    /// Create recurring appointments
    /// </summary>
    [HttpPost("recurring")]
    [Authorize(Roles = "Admin,Teacher")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<AppointmentDto>>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<AppointmentDto>>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateRecurring([FromBody] CreateRecurringAppointmentDto createDto)
    {
        try
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var appointments = await _appointmentService.CreateRecurringAppointmentsAsync(createDto, userId);
            
            return CreatedAtAction(nameof(GetAll), 
                ApiResponse<IEnumerable<AppointmentDto>>.SuccessResponse(appointments, $"{appointments.Count()} recurring appointments created successfully"));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ApiResponse<IEnumerable<AppointmentDto>>.ErrorResponse(ex.Message));
        }
    }

    /// <summary>
    /// Update appointment
    /// </summary>
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Teacher")]
    [ProducesResponseType(typeof(ApiResponse<AppointmentDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<AppointmentDto>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<AppointmentDto>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateAppointmentDto updateDto)
    {
        try
        {
            var appointment = await _appointmentService.UpdateAppointmentAsync(id, updateDto);
            
            if (appointment == null)
                return NotFound(ApiResponse<AppointmentDto>.ErrorResponse("Appointment not found"));

            return Ok(ApiResponse<AppointmentDto>.SuccessResponse(appointment, "Appointment updated successfully"));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ApiResponse<AppointmentDto>.ErrorResponse(ex.Message));
        }
    }

    /// <summary>
    /// Cancel appointment
    /// </summary>
    [HttpPost("{id}/cancel")]
    [Authorize(Roles = "Admin,Teacher")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Cancel(int id, [FromBody] CancelAppointmentRequest request)
    {
        var result = await _appointmentService.CancelAppointmentAsync(id, request.Reason);
        
        if (!result)
            return NotFound(ApiResponse<object>.ErrorResponse("Appointment not found"));

        return NoContent();
    }

    /// <summary>
    /// Delete appointment
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _appointmentService.DeleteAppointmentAsync(id);
        
        if (!result)
            return NotFound(ApiResponse<object>.ErrorResponse("Appointment not found"));

        return NoContent();
    }
}

// Request model
public class CancelAppointmentRequest
{
    public string Reason { get; set; } = string.Empty;
}