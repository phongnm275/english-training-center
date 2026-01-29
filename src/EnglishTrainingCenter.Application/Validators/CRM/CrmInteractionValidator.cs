using FluentValidation;
using EnglishTrainingCenter.Application.DTOs.CRM;

namespace EnglishTrainingCenter.Application.Validators.CRM;

/// <summary>
/// Validator for creating CRM interaction
/// </summary>
public class CreateCrmInteractionValidator : AbstractValidator<CreateCrmInteractionDto>
{
    public CreateCrmInteractionValidator()
    {
        RuleFor(x => x.InteractionType)
            .NotEmpty().WithMessage("Interaction type is required")
            .Must(x => new[] { "Call", "Email", "Meeting", "Demo" }.Contains(x))
            .WithMessage("Interaction type must be Call, Email, Meeting, or Demo");

        RuleFor(x => x.Subject)
            .NotEmpty().WithMessage("Subject is required")
            .MaximumLength(200).WithMessage("Subject must not exceed 200 characters");

        RuleFor(x => x.Description)
            .MaximumLength(2000).WithMessage("Description must not exceed 2000 characters");

        RuleFor(x => x.InteractionDate)
            .NotEmpty().WithMessage("Interaction date is required")
            .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Interaction date cannot be in the future");

        RuleFor(x => x.DurationMinutes)
            .GreaterThan(0).WithMessage("Duration must be greater than 0")
            .When(x => x.DurationMinutes.HasValue);

        RuleFor(x => x.Outcome)
            .Must(x => new[] { "Positive", "Negative", "Neutral", "Follow-up Required" }.Contains(x))
            .WithMessage("Outcome must be Positive, Negative, Neutral, or Follow-up Required")
            .When(x => !string.IsNullOrEmpty(x.Outcome));

        RuleFor(x => x.NextFollowUpDate)
            .GreaterThan(DateTime.UtcNow).WithMessage("Next follow-up date must be in the future")
            .When(x => x.NextFollowUpDate.HasValue);
    }
}

/// <summary>
/// Validator for updating CRM interaction
/// </summary>
public class UpdateCrmInteractionValidator : AbstractValidator<UpdateCrmInteractionDto>
{
    public UpdateCrmInteractionValidator()
    {
        RuleFor(x => x.InteractionType)
            .NotEmpty().WithMessage("Interaction type is required")
            .Must(x => new[] { "Call", "Email", "Meeting", "Demo" }.Contains(x))
            .WithMessage("Interaction type must be Call, Email, Meeting, or Demo");

        RuleFor(x => x.Subject)
            .NotEmpty().WithMessage("Subject is required")
            .MaximumLength(200).WithMessage("Subject must not exceed 200 characters");

        RuleFor(x => x.Description)
            .MaximumLength(2000).WithMessage("Description must not exceed 2000 characters");

        RuleFor(x => x.InteractionDate)
            .NotEmpty().WithMessage("Interaction date is required")
            .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Interaction date cannot be in the future");

        RuleFor(x => x.DurationMinutes)
            .GreaterThan(0).WithMessage("Duration must be greater than 0")
            .When(x => x.DurationMinutes.HasValue);

        RuleFor(x => x.Outcome)
            .Must(x => new[] { "Positive", "Negative", "Neutral", "Follow-up Required" }.Contains(x))
            .WithMessage("Outcome must be Positive, Negative, Neutral, or Follow-up Required")
            .When(x => !string.IsNullOrEmpty(x.Outcome));

        RuleFor(x => x.NextFollowUpDate)
            .GreaterThan(DateTime.UtcNow).WithMessage("Next follow-up date must be in the future")
            .When(x => x.NextFollowUpDate.HasValue);
    }
}
