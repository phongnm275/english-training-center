namespace EnglishTrainingCenter.Application.DTOs.Payment;

/// <summary>
/// Request DTO for creating a new payment.
/// </summary>
public class CreatePaymentRequest
{
    /// <summary>
    /// Student ID associated with payment.
    /// </summary>
    public int StudentId { get; set; }

    /// <summary>
    /// Course ID associated with payment (optional).
    /// </summary>
    public int? CourseId { get; set; }

    /// <summary>
    /// Payment amount.
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// Payment method (e.g., "Cash", "Bank Transfer", "Credit Card", "Check").
    /// </summary>
    public string PaymentMethod { get; set; } = string.Empty;

    /// <summary>
    /// Optional description or notes for payment.
    /// </summary>
    public string Description { get; set; } = string.Empty;
}

/// <summary>
/// Request DTO for updating an existing payment.
/// </summary>
public class UpdatePaymentRequest
{
    /// <summary>
    /// Payment status (e.g., "Pending", "Completed", "Failed", "Cancelled").
    /// </summary>
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// Payment method (e.g., "Cash", "Bank Transfer", "Credit Card", "Check").
    /// </summary>
    public string PaymentMethod { get; set; } = string.Empty;

    /// <summary>
    /// Optional description or notes for payment.
    /// </summary>
    public string Description { get; set; } = string.Empty;
}

/// <summary>
/// Response DTO for payment list (paginated results).
/// </summary>
public class PaymentListDto
{
    /// <summary>
    /// Unique payment identifier.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Invoice number for reference.
    /// </summary>
    public string InvoiceNumber { get; set; } = string.Empty;

    /// <summary>
    /// Student ID.
    /// </summary>
    public int StudentId { get; set; }

    /// <summary>
    /// Course ID (if applicable).
    /// </summary>
    public int? CourseId { get; set; }

    /// <summary>
    /// Payment amount.
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// Payment date.
    /// </summary>
    public DateTime PaymentDate { get; set; }

    /// <summary>
    /// Payment status.
    /// </summary>
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// Payment method used.
    /// </summary>
    public string PaymentMethod { get; set; } = string.Empty;
}

/// <summary>
/// Response DTO for detailed payment information.
/// </summary>
public class PaymentDetailDto
{
    /// <summary>
    /// Unique payment identifier.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Invoice number for reference.
    /// </summary>
    public string InvoiceNumber { get; set; } = string.Empty;

    /// <summary>
    /// Student ID.
    /// </summary>
    public int StudentId { get; set; }

    /// <summary>
    /// Student name (populated from Student entity).
    /// </summary>
    public string StudentName { get; set; } = string.Empty;

    /// <summary>
    /// Course ID (if applicable).
    /// </summary>
    public int? CourseId { get; set; }

    /// <summary>
    /// Course name (populated from Course entity).
    /// </summary>
    public string CourseName { get; set; } = string.Empty;

    /// <summary>
    /// Payment amount.
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// Payment date.
    /// </summary>
    public DateTime PaymentDate { get; set; }

    /// <summary>
    /// Payment status.
    /// </summary>
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// Payment method used.
    /// </summary>
    public string PaymentMethod { get; set; } = string.Empty;

    /// <summary>
    /// Description or notes.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// When payment was created.
    /// </summary>
    public DateTime CreatedDate { get; set; }

    /// <summary>
    /// When payment was last modified.
    /// </summary>
    public DateTime ModifiedDate { get; set; }
}

/// <summary>
/// Invoice DTO for payment invoicing.
/// </summary>
public class PaymentInvoiceDto
{
    /// <summary>
    /// Invoice number.
    /// </summary>
    public string InvoiceNumber { get; set; } = string.Empty;

    /// <summary>
    /// Payment ID reference.
    /// </summary>
    public int PaymentId { get; set; }

    /// <summary>
    /// Student full name.
    /// </summary>
    public string StudentName { get; set; } = string.Empty;

    /// <summary>
    /// Student email address.
    /// </summary>
    public string StudentEmail { get; set; } = string.Empty;

    /// <summary>
    /// Course name (if applicable).
    /// </summary>
    public string CourseName { get; set; } = string.Empty;

    /// <summary>
    /// Payment amount.
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// Payment date.
    /// </summary>
    public DateTime PaymentDate { get; set; }

    /// <summary>
    /// Payment status.
    /// </summary>
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// Payment method used.
    /// </summary>
    public string PaymentMethod { get; set; } = string.Empty;

    /// <summary>
    /// Payment description.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Invoice created date.
    /// </summary>
    public DateTime CreatedDate { get; set; }
}

/// <summary>
/// Payment statistics DTO.
/// </summary>
public class PaymentStatisticsDto
{
    /// <summary>
    /// Total number of payments.
    /// </summary>
    public int TotalPayments { get; set; }

    /// <summary>
    /// Number of completed payments.
    /// </summary>
    public int CompletedPayments { get; set; }

    /// <summary>
    /// Number of pending payments.
    /// </summary>
    public int PendingPayments { get; set; }

    /// <summary>
    /// Number of refunded payments.
    /// </summary>
    public int RefundedPayments { get; set; }

    /// <summary>
    /// Total revenue from completed payments.
    /// </summary>
    public decimal TotalRevenue { get; set; }

    /// <summary>
    /// Total refunded amount.
    /// </summary>
    public decimal TotalRefunded { get; set; }

    /// <summary>
    /// Average payment amount.
    /// </summary>
    public decimal AveragePaymentAmount { get; set; }

    /// <summary>
    /// Highest payment amount.
    /// </summary>
    public decimal HighestPaymentAmount { get; set; }

    /// <summary>
    /// Lowest payment amount.
    /// </summary>
    public decimal LowestPaymentAmount { get; set; }
}
