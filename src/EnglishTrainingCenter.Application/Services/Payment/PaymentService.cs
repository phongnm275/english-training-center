using AutoMapper;
using EnglishTrainingCenter.Application.DTOs.Payment;
using EnglishTrainingCenter.Common;
using EnglishTrainingCenter.Domain.Entities;
using EnglishTrainingCenter.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace EnglishTrainingCenter.Application.Services.Payment;

/// <summary>
/// Service implementation for payment and invoice management.
/// Handles payment processing, financial tracking, and invoicing.
/// </summary>
public class PaymentService : IPaymentService
{
    private readonly IRepository<Domain.Entities.Payment> _paymentRepository;
    private readonly IRepository<Student> _studentRepository;
    private readonly IRepository<Course> _courseRepository;
    private readonly IRepository<StudentCourse> _studentCourseRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<PaymentService> _logger;

    public PaymentService(
        IRepository<Domain.Entities.Payment> paymentRepository,
        IRepository<Student> studentRepository,
        IRepository<Course> courseRepository,
        IRepository<StudentCourse> studentCourseRepository,
        IMapper mapper,
        ILogger<PaymentService> logger)
    {
        _paymentRepository = paymentRepository;
        _studentRepository = studentRepository;
        _courseRepository = courseRepository;
        _studentCourseRepository = studentCourseRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<PagedResult<PaymentListDto>> GetAllPaymentsAsync(int pageNumber = 1, int pageSize = 10)
    {
        try
        {
            _logger.LogInformation($"Getting all payments with pagination (page: {pageNumber}, size: {pageSize})");

            var payments = (await _paymentRepository.GetAllAsync())
                .OrderByDescending(p => p.PaymentDate)
                .ToList();

            var totalCount = payments.Count;
            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
            var hasNextPage = pageNumber < totalPages;
            var hasPreviousPage = pageNumber > 1;

            var pagedPayments = payments
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var dtos = _mapper.Map<List<PaymentListDto>>(pagedPayments);

            _logger.LogInformation($"Successfully retrieved {pagedPayments.Count} payments from page {pageNumber}");

            return new PagedResult<PaymentListDto>
            {
                Data = dtos,
                TotalCount = totalCount,
                CurrentPage = pageNumber,
                PageSize = pageSize,
                TotalPages = totalPages,
                HasPreviousPage = hasPreviousPage,
                HasNextPage = hasNextPage
            };
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving payments: {ex.Message}");
            throw;
        }
    }

    public async Task<PaymentDetailDto> GetPaymentByIdAsync(int paymentId)
    {
        try
        {
            _logger.LogInformation($"Getting payment with ID: {paymentId}");

            var payment = (await _paymentRepository.GetAllAsync())
                .FirstOrDefault(p => p.Id == paymentId);

            if (payment == null)
            {
                _logger.LogWarning($"Payment with ID {paymentId} not found");
                throw new InvalidOperationException($"Payment with ID {paymentId} not found");
            }

            var paymentDto = _mapper.Map<PaymentDetailDto>(payment);

            // Load student and course info
            var student = (await _studentRepository.GetAllAsync())
                .FirstOrDefault(s => s.Id == payment.StudentId);
            
            var course = (await _courseRepository.GetAllAsync())
                .FirstOrDefault(c => c.Id == payment.CourseId);

            if (student != null)
                paymentDto.StudentName = student.FullName;

            if (course != null)
                paymentDto.CourseName = course.CourseName;

            _logger.LogInformation($"Successfully retrieved payment {paymentId}");
            return paymentDto;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving payment {paymentId}: {ex.Message}");
            throw;
        }
    }

    public async Task<PaymentDetailDto> CreatePaymentAsync(CreatePaymentRequest request)
    {
        try
        {
            _logger.LogInformation($"Creating new payment for student {request.StudentId}");

            // Verify student exists
            var student = (await _studentRepository.GetAllAsync())
                .FirstOrDefault(s => s.Id == request.StudentId);

            if (student == null)
            {
                _logger.LogWarning($"Student with ID {request.StudentId} not found");
                throw new InvalidOperationException($"Student with ID {request.StudentId} not found");
            }

            // Verify course exists if courseId is provided
            if (request.CourseId.HasValue && request.CourseId > 0)
            {
                var course = (await _courseRepository.GetAllAsync())
                    .FirstOrDefault(c => c.Id == request.CourseId);

                if (course == null)
                {
                    _logger.LogWarning($"Course with ID {request.CourseId} not found");
                    throw new InvalidOperationException($"Course with ID {request.CourseId} not found");
                }
            }

            var payment = _mapper.Map<Domain.Entities.Payment>(request);
            payment.PaymentDate = DateTime.UtcNow;
            payment.Status = "Completed";
            payment.CreatedDate = DateTime.UtcNow;

            // Generate invoice number: INV-YYYYMMDD-XXXXX
            var invoiceNumber = GenerateInvoiceNumber();
            payment.InvoiceNumber = invoiceNumber;

            var createdPayment = await _paymentRepository.AddAsync(payment);
            await _paymentRepository.SaveChangesAsync();

            _logger.LogInformation($"Successfully created payment with ID: {createdPayment.Id}, Invoice: {invoiceNumber}");

            return _mapper.Map<PaymentDetailDto>(createdPayment);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error creating payment: {ex.Message}");
            throw;
        }
    }

    public async Task<PaymentDetailDto> UpdatePaymentAsync(int paymentId, UpdatePaymentRequest request)
    {
        try
        {
            _logger.LogInformation($"Updating payment with ID: {paymentId}");

            var payment = (await _paymentRepository.GetAllAsync())
                .FirstOrDefault(p => p.Id == paymentId);

            if (payment == null)
            {
                _logger.LogWarning($"Payment with ID {paymentId} not found");
                throw new InvalidOperationException($"Payment with ID {paymentId} not found");
            }

            // Prevent changing status if payment is already refunded
            if (payment.Status == "Refunded" && request.Status != "Refunded")
            {
                _logger.LogWarning($"Cannot change status of refunded payment {paymentId}");
                throw new InvalidOperationException("Cannot change status of refunded payment");
            }

            _mapper.Map(request, payment);
            payment.ModifiedDate = DateTime.UtcNow;

            _paymentRepository.Update(payment);
            await _paymentRepository.SaveChangesAsync();

            _logger.LogInformation($"Successfully updated payment {paymentId}");

            return _mapper.Map<PaymentDetailDto>(payment);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error updating payment {paymentId}: {ex.Message}");
            throw;
        }
    }

    public async Task<bool> DeletePaymentAsync(int paymentId)
    {
        try
        {
            _logger.LogInformation($"Deleting payment with ID: {paymentId}");

            var payment = (await _paymentRepository.GetAllAsync())
                .FirstOrDefault(p => p.Id == paymentId);

            if (payment == null)
            {
                _logger.LogWarning($"Payment with ID {paymentId} not found");
                throw new InvalidOperationException($"Payment with ID {paymentId} not found");
            }

            // Prevent deletion of completed payments (soft delete via status)
            if (payment.Status == "Completed")
            {
                _logger.LogWarning($"Cannot delete completed payment {paymentId}");
                throw new InvalidOperationException("Cannot delete completed payment. Mark as cancelled instead.");
            }

            _paymentRepository.Delete(payment);
            await _paymentRepository.SaveChangesAsync();

            _logger.LogInformation($"Successfully deleted payment {paymentId}");
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error deleting payment {paymentId}: {ex.Message}");
            throw;
        }
    }

    public async Task<IEnumerable<PaymentListDto>> GetPaymentsByStatusAsync(string status)
    {
        try
        {
            _logger.LogInformation($"Getting payments by status: {status}");

            var payments = (await _paymentRepository.GetAllAsync())
                .Where(p => p.Status.Equals(status, StringComparison.OrdinalIgnoreCase))
                .OrderByDescending(p => p.PaymentDate)
                .ToList();

            var dtos = _mapper.Map<List<PaymentListDto>>(payments);

            _logger.LogInformation($"Found {dtos.Count} payments with status '{status}'");
            return dtos;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving payments by status: {ex.Message}");
            throw;
        }
    }

    public async Task<IEnumerable<PaymentListDto>> GetPaymentsByStudentAsync(int studentId)
    {
        try
        {
            _logger.LogInformation($"Getting payments for student: {studentId}");

            var payments = (await _paymentRepository.GetAllAsync())
                .Where(p => p.StudentId == studentId)
                .OrderByDescending(p => p.PaymentDate)
                .ToList();

            var dtos = _mapper.Map<List<PaymentListDto>>(payments);

            _logger.LogInformation($"Found {dtos.Count} payments for student {studentId}");
            return dtos;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving student payments: {ex.Message}");
            throw;
        }
    }

    public async Task<IEnumerable<PaymentListDto>> GetPaymentsByCourseAsync(int courseId)
    {
        try
        {
            _logger.LogInformation($"Getting payments for course: {courseId}");

            var payments = (await _paymentRepository.GetAllAsync())
                .Where(p => p.CourseId == courseId)
                .OrderByDescending(p => p.PaymentDate)
                .ToList();

            var dtos = _mapper.Map<List<PaymentListDto>>(payments);

            _logger.LogInformation($"Found {dtos.Count} payments for course {courseId}");
            return dtos;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving course payments: {ex.Message}");
            throw;
        }
    }

    public async Task<decimal> GetStudentTotalPaidAsync(int studentId)
    {
        try
        {
            var payments = (await _paymentRepository.GetAllAsync())
                .Where(p => p.StudentId == studentId && p.Status == "Completed")
                .ToList();

            return payments.Sum(p => p.Amount);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error calculating total paid for student {studentId}: {ex.Message}");
            throw;
        }
    }

    public async Task<decimal> GetStudentOutstandingBalanceAsync(int studentId)
    {
        try
        {
            var studentCourses = (await _studentCourseRepository.GetAllAsync())
                .Where(sc => sc.StudentId == studentId)
                .ToList();

            var courses = (await _courseRepository.GetAllAsync()).ToList();
            var payments = (await _paymentRepository.GetAllAsync())
                .Where(p => p.StudentId == studentId && p.Status == "Completed")
                .ToList();

            decimal totalOwed = 0;
            foreach (var studentCourse in studentCourses)
            {
                var course = courses.FirstOrDefault(c => c.Id == studentCourse.CourseId);
                if (course != null)
                    totalOwed += course.Fee;
            }

            decimal totalPaid = payments.Sum(p => p.Amount);
            return Math.Max(0, totalOwed - totalPaid);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error calculating outstanding balance for student {studentId}: {ex.Message}");
            throw;
        }
    }

    public async Task<PaymentDetailDto> ProcessRefundAsync(int paymentId, decimal refundAmount, string reason)
    {
        try
        {
            _logger.LogInformation($"Processing refund for payment {paymentId}: {refundAmount}");

            var originalPayment = (await _paymentRepository.GetAllAsync())
                .FirstOrDefault(p => p.Id == paymentId);

            if (originalPayment == null)
            {
                _logger.LogWarning($"Payment with ID {paymentId} not found");
                throw new InvalidOperationException($"Payment with ID {paymentId} not found");
            }

            if (refundAmount > originalPayment.Amount)
            {
                _logger.LogWarning($"Refund amount {refundAmount} exceeds payment amount {originalPayment.Amount}");
                throw new InvalidOperationException("Refund amount cannot exceed payment amount");
            }

            // Update original payment status
            originalPayment.Status = refundAmount == originalPayment.Amount ? "Refunded" : "PartiallyRefunded";
            originalPayment.ModifiedDate = DateTime.UtcNow;
            _paymentRepository.Update(originalPayment);

            // Create refund record
            var refundPayment = new Domain.Entities.Payment
            {
                StudentId = originalPayment.StudentId,
                CourseId = originalPayment.CourseId,
                Amount = -refundAmount,
                PaymentDate = DateTime.UtcNow,
                Status = "Refunded",
                PaymentMethod = originalPayment.PaymentMethod,
                Description = $"Refund for {reason}",
                InvoiceNumber = $"REF-{originalPayment.InvoiceNumber}",
                CreatedDate = DateTime.UtcNow
            };

            var createdRefund = await _paymentRepository.AddAsync(refundPayment);
            await _paymentRepository.SaveChangesAsync();

            _logger.LogInformation($"Successfully processed refund: {refundAmount} for payment {paymentId}");

            return _mapper.Map<PaymentDetailDto>(createdRefund);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error processing refund: {ex.Message}");
            throw;
        }
    }

    public async Task<PaymentInvoiceDto> GetInvoiceAsync(int paymentId)
    {
        try
        {
            _logger.LogInformation($"Generating invoice for payment {paymentId}");

            var payment = (await _paymentRepository.GetAllAsync())
                .FirstOrDefault(p => p.Id == paymentId);

            if (payment == null)
            {
                _logger.LogWarning($"Payment with ID {paymentId} not found");
                throw new InvalidOperationException($"Payment with ID {paymentId} not found");
            }

            var student = (await _studentRepository.GetAllAsync())
                .FirstOrDefault(s => s.Id == payment.StudentId);

            var course = (await _courseRepository.GetAllAsync())
                .FirstOrDefault(c => c.Id == payment.CourseId);

            var invoice = new PaymentInvoiceDto
            {
                InvoiceNumber = payment.InvoiceNumber,
                PaymentId = payment.Id,
                StudentName = student?.FullName ?? "Unknown",
                StudentEmail = student?.Email ?? string.Empty,
                CourseName = course?.CourseName ?? "General Payment",
                Amount = payment.Amount,
                PaymentDate = payment.PaymentDate,
                Status = payment.Status,
                PaymentMethod = payment.PaymentMethod,
                Description = payment.Description,
                CreatedDate = payment.CreatedDate
            };

            return invoice;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error generating invoice for payment {paymentId}: {ex.Message}");
            throw;
        }
    }

    public async Task<decimal> GetRevenueAsync(DateTime startDate, DateTime endDate)
    {
        try
        {
            _logger.LogInformation($"Calculating revenue from {startDate} to {endDate}");

            var payments = (await _paymentRepository.GetAllAsync())
                .Where(p => p.PaymentDate >= startDate && 
                           p.PaymentDate <= endDate &&
                           p.Status == "Completed")
                .ToList();

            var revenue = payments.Sum(p => p.Amount);
            return revenue;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error calculating revenue: {ex.Message}");
            throw;
        }
    }

    public async Task<PaymentStatisticsDto> GetStatisticsAsync()
    {
        try
        {
            _logger.LogInformation("Calculating payment statistics");

            var allPayments = (await _paymentRepository.GetAllAsync()).ToList();

            var completedPayments = allPayments.Where(p => p.Status == "Completed").ToList();
            var pendingPayments = allPayments.Where(p => p.Status == "Pending").ToList();
            var refundedPayments = allPayments.Where(p => p.Status == "Refunded").ToList();

            var stats = new PaymentStatisticsDto
            {
                TotalPayments = allPayments.Count,
                CompletedPayments = completedPayments.Count,
                PendingPayments = pendingPayments.Count,
                RefundedPayments = refundedPayments.Count,
                TotalRevenue = completedPayments.Sum(p => p.Amount),
                TotalRefunded = Math.Abs(refundedPayments.Sum(p => p.Amount)),
                AveragePaymentAmount = completedPayments.Any() ? completedPayments.Average(p => p.Amount) : 0,
                HighestPaymentAmount = completedPayments.Any() ? completedPayments.Max(p => p.Amount) : 0,
                LowestPaymentAmount = completedPayments.Any() ? completedPayments.Min(p => p.Amount) : 0
            };

            return stats;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error calculating statistics: {ex.Message}");
            throw;
        }
    }

    private string GenerateInvoiceNumber()
    {
        var date = DateTime.UtcNow.ToString("yyyyMMdd");
        var random = new Random().Next(10000, 99999);
        return $"INV-{date}-{random}";
    }
}
