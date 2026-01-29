using FluentValidation;
using EnglishTrainingCenter.Application.DTOs.CRM;

namespace EnglishTrainingCenter.Application.Validators.CRM;

/// <summary>
/// Validator for creating opportunity
/// </summary>
public class CreateOpportunityValidator : AbstractValidator<CreateOpportunityDto>
{
    public CreateOpportunityValidator()
    {
        RuleFor(x => x.CustomerId)
            .GreaterThan(0).WithMessage("Customer ID is required");

        RuleFor(x => x.OpportunityName)
            .NotEmpty().WithMessage("Opportunity name is required")
            .MaximumLength(200).WithMessage("Opportunity name must not exceed 200 characters");

        RuleFor(x => x.Description)
            .MaximumLength(2000).WithMessage("Description must not exceed 2000 characters");

        RuleFor(x => x.Value)
            .GreaterThan(0).WithMessage("Value must be greater than 0");

        RuleFor(x => x.Currency)
            .NotEmpty().WithMessage("Currency is required")
            .Length(3).WithMessage("Currency code must be 3 characters");

        RuleFor(x => x.Stage)
            .Must(x => new[] { "Prospect", "Qualification", "Proposal", "Negotiation", "Closed Won", "Closed Lost" }.Contains(x))
            .WithMessage("Stage must be Prospect, Qualification, Proposal, Negotiation, Closed Won, or Closed Lost");

        RuleFor(x => x.Probability)
            .GreaterThanOrEqualTo(0).WithMessage("Probability must be >= 0")
            .LessThanOrEqualTo(100).WithMessage("Probability must be <= 100")
            .When(x => x.Probability.HasValue);

        RuleFor(x => x.EstimatedCloseDate)
            .GreaterThan(DateTime.UtcNow).WithMessage("Estimated close date must be in the future")
            .When(x => x.EstimatedCloseDate.HasValue);
    }
}

/// <summary>
/// Validator for updating opportunity
/// </summary>
public class UpdateOpportunityValidator : AbstractValidator<UpdateOpportunityDto>
{
    public UpdateOpportunityValidator()
    {
        RuleFor(x => x.CustomerId)
            .GreaterThan(0).WithMessage("Customer ID is required");

        RuleFor(x => x.OpportunityName)
            .NotEmpty().WithMessage("Opportunity name is required")
            .MaximumLength(200).WithMessage("Opportunity name must not exceed 200 characters");

        RuleFor(x => x.Description)
            .MaximumLength(2000).WithMessage("Description must not exceed 2000 characters");

        RuleFor(x => x.Value)
            .GreaterThan(0).WithMessage("Value must be greater than 0");

        RuleFor(x => x.Currency)
            .NotEmpty().WithMessage("Currency is required")
            .Length(3).WithMessage("Currency code must be 3 characters");

        RuleFor(x => x.Stage)
            .Must(x => new[] { "Prospect", "Qualification", "Proposal", "Negotiation", "Closed Won", "Closed Lost" }.Contains(x))
            .WithMessage("Stage must be Prospect, Qualification, Proposal, Negotiation, Closed Won, or Closed Lost");

        RuleFor(x => x.Probability)
            .GreaterThanOrEqualTo(0).WithMessage("Probability must be >= 0")
            .LessThanOrEqualTo(100).WithMessage("Probability must be <= 100")
            .When(x => x.Probability.HasValue);

        RuleFor(x => x.EstimatedCloseDate)
            .GreaterThan(DateTime.UtcNow).WithMessage("Estimated close date must be in the future")
            .When(x => x.EstimatedCloseDate.HasValue);
    }
}

/// <summary>
/// Validator for updating opportunity stage
/// </summary>
public class UpdateOpportunityStageValidator : AbstractValidator<UpdateOpportunityStageDto>
{
    public UpdateOpportunityStageValidator()
    {
        RuleFor(x => x.Stage)
            .NotEmpty().WithMessage("Stage is required")
            .Must(x => new[] { "Prospect", "Qualification", "Proposal", "Negotiation", "Closed Won", "Closed Lost" }.Contains(x))
            .WithMessage("Stage must be Prospect, Qualification, Proposal, Negotiation, Closed Won, or Closed Lost");

        RuleFor(x => x.Probability)
            .GreaterThanOrEqualTo(0).WithMessage("Probability must be >= 0")
            .LessThanOrEqualTo(100).WithMessage("Probability must be <= 100")
            .When(x => x.Probability.HasValue);
    }
}
