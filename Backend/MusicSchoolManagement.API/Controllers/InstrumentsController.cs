using MusicSchoolManagement.Core.DTOs.Common;
using MusicSchoolManagement.Core.Interfaces.Services;

namespace MusicSchoolManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class InstrumentsController : ControllerBase
{
    private readonly IInstrumentService _instrumentService;

    public InstrumentsController(IInstrumentService instrumentService)
    {
        _instrumentService = instrumentService;
    }

    /// <summary>
    /// Get all instruments
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<InstrumentDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var instruments = await _instrumentService.GetAllInstrumentsAsync();
        return Ok(ApiResponse<IEnumerable<InstrumentDto>>.SuccessResponse(instruments));
    }

    /// <summary>
    /// Get active instruments
    /// </summary>
    [HttpGet("active")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<InstrumentDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetActive()
    {
        var instruments = await _instrumentService.GetActiveInstrumentsAsync();
        return Ok(ApiResponse<IEnumerable<InstrumentDto>>.SuccessResponse(instruments));
    }

    /// <summary>
    /// Get instrument by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponse<InstrumentDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<InstrumentDto>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var instrument = await _instrumentService.GetInstrumentByIdAsync(id);
        return Ok(ApiResponse<InstrumentDto>.SuccessResponse(instrument));
    }

    /// <summary>
    /// Create new instrument
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ApiResponse<InstrumentDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<InstrumentDto>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateInstrumentRequest request)
    {
        var instrument = await _instrumentService.CreateInstrumentAsync(request.Name, request.Description);
        return CreatedAtAction(nameof(GetById), new { id = instrument.Id }, 
            ApiResponse<InstrumentDto>.SuccessResponse(instrument, "Instrument created successfully"));
    }

    /// <summary>
    /// Update instrument
    /// </summary>
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ApiResponse<InstrumentDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<InstrumentDto>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<InstrumentDto>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateInstrumentRequest request)
    {
        var instrument = await _instrumentService.UpdateInstrumentAsync(id, request.Name, request.Description, request.IsActive);
        return Ok(ApiResponse<InstrumentDto>.SuccessResponse(instrument, "Instrument updated successfully"));
    }

    /// <summary>
    /// Delete instrument
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        await _instrumentService.DeleteInstrumentAsync(id);
        return NoContent();
    }
}

// Request models
public class CreateInstrumentRequest
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}

public class UpdateInstrumentRequest
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsActive { get; set; }
}