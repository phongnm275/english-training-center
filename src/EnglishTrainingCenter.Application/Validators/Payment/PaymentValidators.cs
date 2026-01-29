using FluentValidation;
using EnglishTrainingCenter.Application.DTOs.Payment;

namespace EnglishTrainingCenter.Application.Validators.Payment;

/// <summary>
/// Validator for CreatePaymentRequest.
/// </summary>
public class CreatePaymentRequestValidator : AbstractValidator<CreatePaymentRequest>
{
    public CreatePaymentRequestValidator()
    {
        RuleFor(x => x.StudentId)
            .GreaterThan(0).WithMessage("Student ID must be greater than 0");

        RuleFor(x => x.CourseId)
            .GreaterThan(0).WithMessage("Course ID must be greater than 0 (if provided)")
            .When(x => x.CourseId.HasValue);

        RuleFor(x => x.Amount)
            .GreaterThan(0).WithMessage("Payment amount must be greater than 0")
            .LessThanOrEqualTo(1000000000).WithMessage("Payment amount cannot exceed 1,000,000,000");

        RuleFor(x => x.PaymentMethod)
            .NotEmpty().WithMessage("Payment method is required")
            .Length(3, 100).WithMessage("Payment method must be between 3 and 100 characters")
            .Must(x => new[] { "Cash", "Bank Transfer", "Credit Card", "Check", "E-wallet", "Other" }
                .Contains(x, StringComparer.OrdinalIgnoreCase))
            .WithMessage("Payment method must be one of: Cash, Bank Transfer, Credit Card, Check, E-wallet, Other");

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Description cannot exceed 500 characters");
    }
}

/// <summary>
/// Validator for UpdatePaymentRequest.
/// </summary>
public class UpdatePaymentRequestValidator : AbstractValidator<UpdatePaymentRequest>
{
    public UpdatePaymentRequestValidator()
    {
        RuleFor(x => x.Status)
            .NotEmpty().WithMessage("Status is required")
            .Must(x => new[] { "Pending", "Completed", "Failed", "Cancelled", "Refunded", "PartiallyRefunded" }
                .Contains(x, StringComparer.OrdinalIgnoreCase))
            .WithMessage("Status must be one of: Pending, Completed, Failed, Cancelled, Refunded, PartiallyRefunded");

        RuleFor(x => x.PaymentMethod)
            .NotEmpty().WithMessage("Payment method is required")
            .Length(3, 100).WithMessage("Payment method must be between 3 and 100 characters")
            .Must(x => new[] { "Cash", "Bank Transfer", "Credit Card", "Check", "E-wallet", "Other" }
                .Contains(x, StringComparer.OrdinalIgnoreCase))
            .WithMessage("Payment method must be one of: Cash, Bank Transfer, Credit Card, Check, E-wallet, Other");

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Description cannot exceed 500 characters");
    }
}
