using FluentValidation;
using EnglishTrainingCenter.Application.DTOs.CRM;

namespace EnglishTrainingCenter.Application.Validators.CRM;

/// <summary>
/// Validator for creating lead
/// </summary>
public class CreateLeadValidator : AbstractValidator<CreateLeadDto>
{
    public CreateLeadValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required")
            .MaximumLength(100).WithMessage("First name must not exceed 100 characters");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required")
            .MaximumLength(100).WithMessage("Last name must not exceed 100 characters");

        RuleFor(x => x.Email)
            .EmailAddress().WithMessage("Invalid email format")
            .When(x => !string.IsNullOrEmpty(x.Email));

        RuleFor(x => x.Phone)
            .Matches(@"^\+?[0-9\s\-\(\)]{7,}$").WithMessage("Invalid phone format")
            .When(x => !string.IsNullOrEmpty(x.Phone));

        RuleFor(x => x.Source)
            .NotEmpty().WithMessage("Source is required")
            .Must(x => new[] { "Website", "Email", "Phone", "Referral", "Event" }.Contains(x))
            .WithMessage("Source must be Website, Email, Phone, Referral, or Event");

        RuleFor(x => x.Status)
            .Must(x => new[] { "New", "Qualified", "Contacted", "Unqualified", "Converted" }.Contains(x))
            .WithMessage("Status must be New, Qualified, Contacted, Unqualified, or Converted");

        RuleFor(x => x.EstimatedValue)
            .GreaterThan(0).WithMessage("Estimated value must be greater than 0")
            .When(x => x.EstimatedValue.HasValue);

        RuleFor(x => x.InterestLevel)
            .Must(x => new[] { "Hot", "Warm", "Cold" }.Contains(x))
            .WithMessage("Interest level must be Hot, Warm, or Cold")
            .When(x => !string.IsNullOrEmpty(x.InterestLevel));
    }
}

/// <summary>
/// Validator for updating lead
/// </summary>
public class UpdateLeadValidator : AbstractValidator<UpdateLeadDto>
{
    public UpdateLeadValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required")
            .MaximumLength(100).WithMessage("First name must not exceed 100 characters");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required")
            .MaximumLength(100).WithMessage("Last name must not exceed 100 characters");

        RuleFor(x => x.Email)
            .EmailAddress().WithMessage("Invalid email format")
            .When(x => !string.IsNullOrEmpty(x.Email));

        RuleFor(x => x.Phone)
            .Matches(@"^\+?[0-9\s\-\(\)]{7,}$").WithMessage("Invalid phone format")
            .When(x => !string.IsNullOrEmpty(x.Phone));

        RuleFor(x => x.Source)
            .NotEmpty().WithMessage("Source is required")
            .Must(x => new[] { "Website", "Email", "Phone", "Referral", "Event" }.Contains(x))
            .WithMessage("Source must be Website, Email, Phone, Referral, or Event");

        RuleFor(x => x.Status)
            .Must(x => new[] { "New", "Qualified", "Contacted", "Unqualified", "Converted" }.Contains(x))
            .WithMessage("Status must be New, Qualified, Contacted, Unqualified, or Converted");

        RuleFor(x => x.EstimatedValue)
            .GreaterThan(0).WithMessage("Estimated value must be greater than 0")
            .When(x => x.EstimatedValue.HasValue);

        RuleFor(x => x.InterestLevel)
            .Must(x => new[] { "Hot", "Warm", "Cold" }.Contains(x))
            .WithMessage("Interest level must be Hot, Warm, or Cold")
            .When(x => !string.IsNullOrEmpty(x.InterestLevel));
    }
}

/// <summary>
/// Validator for converting lead to customer
/// </summary>
public class ConvertLeadToCustomerValidator : AbstractValidator<ConvertLeadToCustomerDto>
{
    public ConvertLeadToCustomerValidator()
    {
        RuleFor(x => x.LeadId)
            .GreaterThan(0).WithMessage("Lead ID must be greater than 0");

        RuleFor(x => x.City)
            .MaximumLength(100).WithMessage("City must not exceed 100 characters")
            .When(x => !string.IsNullOrEmpty(x.City));

        RuleFor(x => x.Country)
            .MaximumLength(100).WithMessage("Country must not exceed 100 characters")
            .When(x => !string.IsNullOrEmpty(x.Country));
    }
}
