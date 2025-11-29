using System.Security.Claims;
using MusicSchoolManagement.Core.DTOs.Payments;
using MusicSchoolManagement.Core.Interfaces.Services;

namespace MusicSchoolManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PaymentsController : ControllerBase
{
    private readonly IPaymentService _paymentService;

    public PaymentsController(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    /// <summary>
    /// Get all payments
    /// </summary>
    [HttpGet]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<PaymentDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var payments = await _paymentService.GetAllPaymentsAsync();
        return Ok(ApiResponse<IEnumerable<PaymentDto>>.SuccessResponse(payments));
    }

    /// <summary>
    /// Get payments by student
    /// </summary>
    [HttpGet("student/{studentId}")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<PaymentDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByStudent(int studentId)
    {
        var payments = await _paymentService.GetPaymentsByStudentAsync(studentId);
        return Ok(ApiResponse<IEnumerable<PaymentDto>>.SuccessResponse(payments));
    }

    /// <summary>
    /// Get payments by date range
    /// </summary>
    [HttpGet("range")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<PaymentDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByDateRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
    {
        var payments = await _paymentService.GetPaymentsByDateRangeAsync(startDate, endDate);
        return Ok(ApiResponse<IEnumerable<PaymentDto>>.SuccessResponse(payments));
    }

    /// <summary>
    /// Get pending payments
    /// </summary>
    [HttpGet("pending")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<PaymentDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPending()
    {
        var payments = await _paymentService.GetPendingPaymentsAsync();
        return Ok(ApiResponse<IEnumerable<PaymentDto>>.SuccessResponse(payments));
    }

    /// <summary>
    /// Get payment by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponse<PaymentDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<PaymentDto>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var payment = await _paymentService.GetPaymentByIdAsync(id);
        
        if (payment == null)
            return NotFound(ApiResponse<PaymentDto>.ErrorResponse("Payment not found"));

        return Ok(ApiResponse<PaymentDto>.SuccessResponse(payment));
    }

    /// <summary>
    /// Create new payment
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ApiResponse<PaymentDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<PaymentDto>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreatePaymentDto createDto)
    {
        try
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var payment = await _paymentService.CreatePaymentAsync(createDto, userId);
            
            return CreatedAtAction(nameof(GetById), new { id = payment.Id }, 
                ApiResponse<PaymentDto>.SuccessResponse(payment, "Payment created successfully"));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ApiResponse<PaymentDto>.ErrorResponse(ex.Message));
        }
    }

    /// <summary>
    /// Get total revenue for date range
    /// </summary>
    [HttpGet("revenue")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ApiResponse<decimal>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetRevenue([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
    {
        var revenue = await _paymentService.GetTotalRevenueAsync(startDate, endDate);
        return Ok(ApiResponse<decimal>.SuccessResponse(revenue, $"Total revenue: {revenue:C}"));
    }

    /// <summary>
    /// Refund payment
    /// </summary>
    [HttpPost("{id}/refund")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Refund(int id)
    {
        try
        {
            var result = await _paymentService.RefundPaymentAsync(id);
            
            if (!result)
                return NotFound(ApiResponse<object>.ErrorResponse("Payment not found"));

            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ApiResponse<object>.ErrorResponse(ex.Message));
        }
    }
}