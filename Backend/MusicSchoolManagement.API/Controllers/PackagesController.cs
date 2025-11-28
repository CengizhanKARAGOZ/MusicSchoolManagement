using MusicSchoolManagement.Core.DTOs.Packages;
using MusicSchoolManagement.Core.Interfaces.Services;

namespace MusicSchoolManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PackagesController : ControllerBase
{
    private readonly IPackageService _packageService;

    public PackagesController(IPackageService packageService)
    {
        _packageService = packageService;
    }

    /// <summary>
    /// Get all packages
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<PackageDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var packages = await _packageService.GetAllPackagesAsync();
        return Ok(ApiResponse<IEnumerable<PackageDto>>.SuccessResponse(packages));
    }

    /// <summary>
    /// Get active packages only
    /// </summary>
    [HttpGet("active")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<PackageDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetActive()
    {
        var packages = await _packageService.GetActivePackagesAsync();
        return Ok(ApiResponse<IEnumerable<PackageDto>>.SuccessResponse(packages));
    }

    /// <summary>
    /// Get package by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponse<PackageDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<PackageDto>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var package = await _packageService.GetPackageByIdAsync(id);
        
        if (package == null)
            return NotFound(ApiResponse<PackageDto>.ErrorResponse("Package not found"));

        return Ok(ApiResponse<PackageDto>.SuccessResponse(package));
    }
    
    /// <summary>
    /// Create new package
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ApiResponse<PackageDto>), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreatePackageDto createDto)
    {
        var package = await _packageService.CreatePackageAsync(createDto);
        return CreatedAtAction(nameof(GetById), new { id = package.Id }, 
            ApiResponse<PackageDto>.SuccessResponse(package, "Package created successfully"));
    }

    /// <summary>
    /// Update package
    /// </summary>
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ApiResponse<PackageDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<PackageDto>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdatePackageDto updateDto)
    {
        var package = await _packageService.UpdatePackageAsync(id, updateDto);
        
        if (package == null)
            return NotFound(ApiResponse<PackageDto>.ErrorResponse("Package not found"));

        return Ok(ApiResponse<PackageDto>.SuccessResponse(package, "Package updated successfully"));
    }

    /// <summary>
    /// Delete package
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _packageService.DeletePackageAsync(id);
        
        if (!result)
            return NotFound(ApiResponse<object>.ErrorResponse("Package not found"));

        return NoContent();
    }

    #region Student Package Operations

    /// <summary>
    /// Get student's packages
    /// </summary>
    [HttpGet("student/{studentId}")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<StudentPackageDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetStudentPackages(int studentId)
    {
        var packages = await _packageService.GetStudentPackagesAsync(studentId);
        return Ok(ApiResponse<IEnumerable<StudentPackageDto>>.SuccessResponse(packages));
    }

    /// <summary>
    /// Get student package by ID
    /// </summary>
    [HttpGet("student-package/{id}")]
    [ProducesResponseType(typeof(ApiResponse<StudentPackageDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<StudentPackageDto>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetStudentPackageById(int id)
    {
        var package = await _packageService.GetStudentPackageByIdAsync(id);
        
        if (package == null)
            return NotFound(ApiResponse<StudentPackageDto>.ErrorResponse("Student package not found"));

        return Ok(ApiResponse<StudentPackageDto>.SuccessResponse(package));
    }

    /// <summary>
    /// Assign package to student
    /// </summary>
    [HttpPost("assign")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ApiResponse<StudentPackageDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<StudentPackageDto>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AssignPackage([FromBody] AssignPackageDto assignDto)
    {
        try
        {
            var studentPackage = await _packageService.AssignPackageToStudentAsync(assignDto);
            return CreatedAtAction(nameof(GetStudentPackageById), new { id = studentPackage.Id }, 
                ApiResponse<StudentPackageDto>.SuccessResponse(studentPackage, "Package assigned successfully"));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ApiResponse<StudentPackageDto>.ErrorResponse(ex.Message));
        }
    }

    /// <summary>
    /// Cancel student package
    /// </summary>
    [HttpPost("student-package/{id}/cancel")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CancelPackage(int id)
    {
        var result = await _packageService.CancelStudentPackageAsync(id);
        
        if (!result)
            return NotFound(ApiResponse<object>.ErrorResponse("Student package not found"));

        return NoContent();
    }

    #endregion
}