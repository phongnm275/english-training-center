using AutoMapper;
using EnglishTrainingCenter.Application.DTOs.Payment;
using EnglishTrainingCenter.Application.Services.Payment;
using EnglishTrainingCenter.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EnglishTrainingCenter.API.Controllers;

/// <summary>
/// API controller for payment and invoice management.
/// </summary>
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[Authorize]
public class PaymentsController : ControllerBase
{
    private readonly IPaymentService _paymentService;
    private readonly ILogger<PaymentsController> _logger;

    public PaymentsController(IPaymentService paymentService, ILogger<PaymentsController> logger)
    {
        _paymentService = paymentService;
        _logger = logger;
    }

    /// <summary>
    /// Gets all payments with pagination.
    /// </summary>
    /// <param name="pageNumber">Page number (default: 1)</param>
    /// <param name="pageSize">Records per page (default: 10, max: 50)</param>
    /// <returns>Paginated list of payments</returns>
    [HttpGet]
    [ProduceResponseType(typeof(ApiResponse<PagedResult<PaymentListDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllPayments(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        try
        {
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1 || pageSize > 50) pageSize = 10;

            var result = await _paymentService.GetAllPaymentsAsync(pageNumber, pageSize);

            return Ok(new ApiResponse<PagedResult<PaymentListDto>>
            {
                Success = true,
                Message = $"Retrieved {result.Data.Count} payments",
                Data = result
            });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting payments: {ex.Message}");
            return BadRequest(new ApiResponse<string>
            {
                Success = false,
                Message = "Error retrieving payments",
                Data = ex.Message
            });
        }
    }

    /// <summary>
    /// Gets a payment by ID.
    /// </summary>
    /// <param name="id">Payment ID</param>
    /// <returns>Payment details</returns>
    [HttpGet("{id}")]
    [ProduceResponseType(typeof(ApiResponse<PaymentDetailDto>), StatusCodes.Status200OK)]
    [ProduceResponseType(typeof(ApiResponse<string>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetPaymentById(int id)
    {
        try
        {
            var result = await _paymentService.GetPaymentByIdAsync(id);

            return Ok(new ApiResponse<PaymentDetailDto>
            {
                Success = true,
                Message = "Payment retrieved successfully",
                Data = result
            });
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new ApiResponse<string>
            {
                Success = false,
                Message = "Payment not found",
                Data = ex.Message
            });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting payment {id}: {ex.Message}");
            return BadRequest(new ApiResponse<string>
            {
                Success = false,
                Message = "Error retrieving payment",
                Data = ex.Message
            });
        }
    }

    /// <summary>
    /// Creates a new payment.
    /// </summary>
    /// <param name="request">Payment creation request</param>
    /// <returns>Created payment</returns>
    [HttpPost]
    [ProduceResponseType(typeof(ApiResponse<PaymentDetailDto>), StatusCodes.Status201Created)]
    [ProduceResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreatePayment([FromBody] CreatePaymentRequest request)
    {
        try
        {
            var result = await _paymentService.CreatePaymentAsync(request);

            return CreatedAtAction(nameof(GetPaymentById), new { id = result.Id },
                new ApiResponse<PaymentDetailDto>
                {
                    Success = true,
                    Message = "Payment created successfully",
                    Data = result
                });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new ApiResponse<string>
            {
                Success = false,
                Message = "Invalid payment data",
                Data = ex.Message
            });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error creating payment: {ex.Message}");
            return BadRequest(new ApiResponse<string>
            {
                Success = false,
                Message = "Error creating payment",
                Data = ex.Message
            });
        }
    }

    /// <summary>
    /// Updates an existing payment.
    /// </summary>
    /// <param name="id">Payment ID</param>
    /// <param name="request">Payment update request</param>
    /// <returns>Updated payment</returns>
    [HttpPut("{id}")]
    [ProduceResponseType(typeof(ApiResponse<PaymentDetailDto>), StatusCodes.Status200OK)]
    [ProduceResponseType(typeof(ApiResponse<string>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdatePayment(int id, [FromBody] UpdatePaymentRequest request)
    {
        try
        {
            var result = await _paymentService.UpdatePaymentAsync(id, request);

            return Ok(new ApiResponse<PaymentDetailDto>
            {
                Success = true,
                Message = "Payment updated successfully",
                Data = result
            });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new ApiResponse<string>
            {
                Success = false,
                Message = "Cannot update payment",
                Data = ex.Message
            });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error updating payment {id}: {ex.Message}");
            return BadRequest(new ApiResponse<string>
            {
                Success = false,
                Message = "Error updating payment",
                Data = ex.Message
            });
        }
    }

    /// <summary>
    /// Deletes a payment (Admin only).
    /// </summary>
    /// <param name="id">Payment ID</param>
    /// <returns>Success status</returns>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    [ProduceResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
    [ProduceResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeletePayment(int id)
    {
        try
        {
            var result = await _paymentService.DeletePaymentAsync(id);

            return Ok(new ApiResponse<bool>
            {
                Success = result,
                Message = result ? "Payment deleted successfully" : "Failed to delete payment",
                Data = result
            });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new ApiResponse<string>
            {
                Success = false,
                Message = ex.Message,
                Data = ex.Message
            });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error deleting payment {id}: {ex.Message}");
            return BadRequest(new ApiResponse<string>
            {
                Success = false,
                Message = "Error deleting payment",
                Data = ex.Message
            });
        }
    }

    /// <summary>
    /// Gets payments by status.
    /// </summary>
    /// <param name="status">Payment status</param>
    /// <returns>Payments with specified status</returns>
    [HttpGet("status/{status}")]
    [ProduceResponseType(typeof(ApiResponse<IEnumerable<PaymentListDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPaymentsByStatus(string status)
    {
        try
        {
            var validStatuses = new[] { "Pending", "Completed", "Failed", "Cancelled", "Refunded", "PartiallyRefunded" };
            if (!validStatuses.Contains(status, StringComparer.OrdinalIgnoreCase))
                return BadRequest(new ApiResponse<string>
                {
                    Success = false,
                    Message = "Invalid status. Must be: Pending, Completed, Failed, Cancelled, Refunded, or PartiallyRefunded"
                });

            var result = await _paymentService.GetPaymentsByStatusAsync(status);

            return Ok(new ApiResponse<IEnumerable<PaymentListDto>>
            {
                Success = true,
                Message = $"Found {result.Count()} payments with status '{status}'",
                Data = result
            });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting payments by status: {ex.Message}");
            return BadRequest(new ApiResponse<string>
            {
                Success = false,
                Message = "Error retrieving payments",
                Data = ex.Message
            });
        }
    }

    /// <summary>
    /// Gets payments for a specific student.
    /// </summary>
    /// <param name="studentId">Student ID</param>
    /// <returns>Student's payments</returns>
    [HttpGet("student/{studentId}")]
    [ProduceResponseType(typeof(ApiResponse<IEnumerable<PaymentListDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPaymentsByStudent(int studentId)
    {
        try
        {
            var result = await _paymentService.GetPaymentsByStudentAsync(studentId);

            return Ok(new ApiResponse<IEnumerable<PaymentListDto>>
            {
                Success = true,
                Message = $"Found {result.Count()} payments for student {studentId}",
                Data = result
            });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting student payments: {ex.Message}");
            return BadRequest(new ApiResponse<string>
            {
                Success = false,
                Message = "Error retrieving payments",
                Data = ex.Message
            });
        }
    }

    /// <summary>
    /// Gets payments for a specific course.
    /// </summary>
    /// <param name="courseId">Course ID</param>
    /// <returns>Course's payments</returns>
    [HttpGet("course/{courseId}")]
    [ProduceResponseType(typeof(ApiResponse<IEnumerable<PaymentListDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPaymentsByCourse(int courseId)
    {
        try
        {
            var result = await _paymentService.GetPaymentsByCourseAsync(courseId);

            return Ok(new ApiResponse<IEnumerable<PaymentListDto>>
            {
                Success = true,
                Message = $"Found {result.Count()} payments for course {courseId}",
                Data = result
            });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting course payments: {ex.Message}");
            return BadRequest(new ApiResponse<string>
            {
                Success = false,
                Message = "Error retrieving payments",
                Data = ex.Message
            });
        }
    }

    /// <summary>
    /// Gets total amount paid by a student.
    /// </summary>
    /// <param name="studentId">Student ID</param>
    /// <returns>Total amount paid</returns>
    [HttpGet("student/{studentId}/total-paid")]
    [ProduceResponseType(typeof(ApiResponse<decimal>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetStudentTotalPaid(int studentId)
    {
        try
        {
            var totalPaid = await _paymentService.GetStudentTotalPaidAsync(studentId);

            return Ok(new ApiResponse<decimal>
            {
                Success = true,
                Message = "Total paid amount retrieved",
                Data = totalPaid
            });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting total paid for student {studentId}: {ex.Message}");
            return BadRequest(new ApiResponse<string>
            {
                Success = false,
                Message = "Error retrieving total paid",
                Data = ex.Message
            });
        }
    }

    /// <summary>
    /// Gets outstanding balance for a student.
    /// </summary>
    /// <param name="studentId">Student ID</param>
    /// <returns>Outstanding balance</returns>
    [HttpGet("student/{studentId}/outstanding-balance")]
    [ProduceResponseType(typeof(ApiResponse<decimal>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetStudentOutstandingBalance(int studentId)
    {
        try
        {
            var balance = await _paymentService.GetStudentOutstandingBalanceAsync(studentId);

            return Ok(new ApiResponse<decimal>
            {
                Success = true,
                Message = "Outstanding balance retrieved",
                Data = balance
            });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting outstanding balance for student {studentId}: {ex.Message}");
            return BadRequest(new ApiResponse<string>
            {
                Success = false,
                Message = "Error retrieving outstanding balance",
                Data = ex.Message
            });
        }
    }

    /// <summary>
    /// Processes a refund for a payment.
    /// </summary>
    /// <param name="paymentId">Payment ID to refund</param>
    /// <param name="refundAmount">Amount to refund</param>
    /// <param name="reason">Reason for refund</param>
    /// <returns>Refund payment details</returns>
    [HttpPost("{paymentId}/refund")]
    [Authorize(Roles = "Admin")]
    [ProduceResponseType(typeof(ApiResponse<PaymentDetailDto>), StatusCodes.Status200OK)]
    [ProduceResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ProcessRefund(int paymentId, [FromQuery] decimal refundAmount, [FromQuery] string reason)
    {
        try
        {
            if (refundAmount <= 0)
                return BadRequest(new ApiResponse<string>
                {
                    Success = false,
                    Message = "Refund amount must be greater than 0"
                });

            if (string.IsNullOrWhiteSpace(reason))
                return BadRequest(new ApiResponse<string>
                {
                    Success = false,
                    Message = "Refund reason is required"
                });

            var result = await _paymentService.ProcessRefundAsync(paymentId, refundAmount, reason);

            return Ok(new ApiResponse<PaymentDetailDto>
            {
                Success = true,
                Message = $"Refund of {refundAmount} processed successfully",
                Data = result
            });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new ApiResponse<string>
            {
                Success = false,
                Message = ex.Message,
                Data = ex.Message
            });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error processing refund: {ex.Message}");
            return BadRequest(new ApiResponse<string>
            {
                Success = false,
                Message = "Error processing refund",
                Data = ex.Message
            });
        }
    }

    /// <summary>
    /// Gets invoice for a payment.
    /// </summary>
    /// <param name="paymentId">Payment ID</param>
    /// <returns>Invoice details</returns>
    [HttpGet("{paymentId}/invoice")]
    [ProduceResponseType(typeof(ApiResponse<PaymentInvoiceDto>), StatusCodes.Status200OK)]
    [ProduceResponseType(typeof(ApiResponse<string>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetInvoice(int paymentId)
    {
        try
        {
            var invoice = await _paymentService.GetInvoiceAsync(paymentId);

            return Ok(new ApiResponse<PaymentInvoiceDto>
            {
                Success = true,
                Message = "Invoice retrieved successfully",
                Data = invoice
            });
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new ApiResponse<string>
            {
                Success = false,
                Message = "Payment not found",
                Data = ex.Message
            });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting invoice for payment {paymentId}: {ex.Message}");
            return BadRequest(new ApiResponse<string>
            {
                Success = false,
                Message = "Error retrieving invoice",
                Data = ex.Message
            });
        }
    }

    /// <summary>
    /// Gets revenue for a date range.
    /// </summary>
    /// <param name="startDate">Start date (yyyy-MM-dd format)</param>
    /// <param name="endDate">End date (yyyy-MM-dd format)</param>
    /// <returns>Total revenue</returns>
    [HttpGet("revenue")]
    [Authorize(Roles = "Admin")]
    [ProduceResponseType(typeof(ApiResponse<decimal>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetRevenue([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
    {
        try
        {
            if (endDate < startDate)
                return BadRequest(new ApiResponse<string>
                {
                    Success = false,
                    Message = "End date must be after start date"
                });

            var revenue = await _paymentService.GetRevenueAsync(startDate, endDate);

            return Ok(new ApiResponse<decimal>
            {
                Success = true,
                Message = $"Revenue from {startDate:yyyy-MM-dd} to {endDate:yyyy-MM-dd}",
                Data = revenue
            });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error calculating revenue: {ex.Message}");
            return BadRequest(new ApiResponse<string>
            {
                Success = false,
                Message = "Error calculating revenue",
                Data = ex.Message
            });
        }
    }

    /// <summary>
    /// Gets payment statistics.
    /// </summary>
    /// <returns>Payment statistics</returns>
    [HttpGet("statistics")]
    [Authorize(Roles = "Admin")]
    [ProduceResponseType(typeof(ApiResponse<PaymentStatisticsDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetStatistics()
    {
        try
        {
            var stats = await _paymentService.GetStatisticsAsync();

            return Ok(new ApiResponse<PaymentStatisticsDto>
            {
                Success = true,
                Message = "Payment statistics retrieved",
                Data = stats
            });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting statistics: {ex.Message}");
            return BadRequest(new ApiResponse<string>
            {
                Success = false,
                Message = "Error retrieving statistics",
                Data = ex.Message
            });
        }
    }
}
