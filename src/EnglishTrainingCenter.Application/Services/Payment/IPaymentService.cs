using EnglishTrainingCenter.Application.DTOs.Payment;
using EnglishTrainingCenter.Common;

namespace EnglishTrainingCenter.Application.Services.Payment;

/// <summary>
/// Service interface for payment and invoice management.
/// Handles payment processing, invoice generation, refunds, and financial reporting.
/// </summary>
public interface IPaymentService
{
    /// <summary>
    /// Retrieves all payments with pagination.
    /// </summary>
    /// <param name="pageNumber">Page number (1-based)</param>
    /// <param name="pageSize">Number of records per page</param>
    /// <returns>Paginated list of payments</returns>
    Task<PagedResult<PaymentListDto>> GetAllPaymentsAsync(int pageNumber = 1, int pageSize = 10);

    /// <summary>
    /// Retrieves a specific payment by ID.
    /// </summary>
    /// <param name="paymentId">Payment ID</param>
    /// <returns>Payment details</returns>
    Task<PaymentDetailDto> GetPaymentByIdAsync(int paymentId);

    /// <summary>
    /// Creates a new payment record.
    /// </summary>
    /// <param name="request">Payment creation request</param>
    /// <returns>Created payment details</returns>
    Task<PaymentDetailDto> CreatePaymentAsync(CreatePaymentRequest request);

    /// <summary>
    /// Updates an existing payment.
    /// </summary>
    /// <param name="paymentId">Payment ID</param>
    /// <param name="request">Payment update request</param>
    /// <returns>Updated payment details</returns>
    Task<PaymentDetailDto> UpdatePaymentAsync(int paymentId, UpdatePaymentRequest request);

    /// <summary>
    /// Deletes a payment record.
    /// </summary>
    /// <param name="paymentId">Payment ID</param>
    /// <returns>True if successful</returns>
    Task<bool> DeletePaymentAsync(int paymentId);

    /// <summary>
    /// Retrieves payments by status.
    /// </summary>
    /// <param name="status">Payment status (e.g., "Pending", "Completed", "Failed", "Refunded")</param>
    /// <returns>List of payments with specified status</returns>
    Task<IEnumerable<PaymentListDto>> GetPaymentsByStatusAsync(string status);

    /// <summary>
    /// Retrieves all payments for a specific student.
    /// </summary>
    /// <param name="studentId">Student ID</param>
    /// <returns>List of student's payments</returns>
    Task<IEnumerable<PaymentListDto>> GetPaymentsByStudentAsync(int studentId);

    /// <summary>
    /// Retrieves payments for a specific course.
    /// </summary>
    /// <param name="courseId">Course ID</param>
    /// <returns>List of payments for course</returns>
    Task<IEnumerable<PaymentListDto>> GetPaymentsByCourseAsync(int courseId);

    /// <summary>
    /// Gets total amount paid by a student.
    /// </summary>
    /// <param name="studentId">Student ID</param>
    /// <returns>Total amount paid</returns>
    Task<decimal> GetStudentTotalPaidAsync(int studentId);

    /// <summary>
    /// Gets outstanding balance for a student.
    /// </summary>
    /// <param name="studentId">Student ID</param>
    /// <returns>Outstanding balance amount</returns>
    Task<decimal> GetStudentOutstandingBalanceAsync(int studentId);

    /// <summary>
    /// Processes a refund for a payment.
    /// </summary>
    /// <param name="paymentId">Payment ID to refund</param>
    /// <param name="refundAmount">Amount to refund</param>
    /// <param name="reason">Reason for refund</param>
    /// <returns>Refund payment details</returns>
    Task<PaymentDetailDto> ProcessRefundAsync(int paymentId, decimal refundAmount, string reason);

    /// <summary>
    /// Gets invoice details for a payment.
    /// </summary>
    /// <param name="paymentId">Payment ID</param>
    /// <returns>Invoice details</returns>
    Task<PaymentInvoiceDto> GetInvoiceAsync(int paymentId);

    /// <summary>
    /// Gets total revenue for a date range.
    /// </summary>
    /// <param name="startDate">Start date</param>
    /// <param name="endDate">End date</param>
    /// <returns>Total revenue</returns>
    Task<decimal> GetRevenueAsync(DateTime startDate, DateTime endDate);

    /// <summary>
    /// Gets payment statistics.
    /// </summary>
    /// <returns>Payment statistics</returns>
    Task<PaymentStatisticsDto> GetStatisticsAsync();
}
