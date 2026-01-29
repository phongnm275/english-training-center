using FluentValidation;
using EnglishTrainingCenter.Application.DTOs.CRM;

namespace EnglishTrainingCenter.Application.Validators.CRM;

/// <summary>
/// Validator for creating customer
/// </summary>
public class CreateCustomerValidator : AbstractValidator<CreateCustomerDto>
{
    public CreateCustomerValidator()
    {
        RuleFor(x => x.CompanyName)
            .NotEmpty().WithMessage("Company name is required")
            .MaximumLength(200).WithMessage("Company name must not exceed 200 characters");

        RuleFor(x => x.ContactName)
            .NotEmpty().WithMessage("Contact name is required")
            .MaximumLength(150).WithMessage("Contact name must not exceed 150 characters");

        RuleFor(x => x.Email)
            .EmailAddress().WithMessage("Invalid email format")
            .When(x => !string.IsNullOrEmpty(x.Email));

        RuleFor(x => x.Phone)
            .Matches(@"^\+?[0-9\s\-\(\)]{7,}$").WithMessage("Invalid phone format")
            .When(x => !string.IsNullOrEmpty(x.Phone));

        RuleFor(x => x.Status)
            .Must(x => new[] { "Active", "Inactive", "Prospect" }.Contains(x))
            .WithMessage("Status must be Active, Inactive, or Prospect");

        RuleFor(x => x.EmployeeCount)
            .GreaterThan(0).WithMessage("Employee count must be greater than 0")
            .When(x => x.EmployeeCount.HasValue);

        RuleFor(x => x.AnnualRevenue)
            .GreaterThan(0).WithMessage("Annual revenue must be greater than 0")
            .When(x => x.AnnualRevenue.HasValue);
    }
}

/// <summary>
/// Validator for updating customer
/// </summary>
public class UpdateCustomerValidator : AbstractValidator<UpdateCustomerDto>
{
    public UpdateCustomerValidator()
    {
        RuleFor(x => x.CompanyName)
            .NotEmpty().WithMessage("Company name is required")
            .MaximumLength(200).WithMessage("Company name must not exceed 200 characters");

        RuleFor(x => x.ContactName)
            .NotEmpty().WithMessage("Contact name is required")
            .MaximumLength(150).WithMessage("Contact name must not exceed 150 characters");

        RuleFor(x => x.Email)
            .EmailAddress().WithMessage("Invalid email format")
            .When(x => !string.IsNullOrEmpty(x.Email));

        RuleFor(x => x.Phone)
            .Matches(@"^\+?[0-9\s\-\(\)]{7,}$").WithMessage("Invalid phone format")
            .When(x => !string.IsNullOrEmpty(x.Phone));

        RuleFor(x => x.Status)
            .Must(x => new[] { "Active", "Inactive", "Prospect" }.Contains(x))
            .WithMessage("Status must be Active, Inactive, or Prospect");

        RuleFor(x => x.EmployeeCount)
            .GreaterThan(0).WithMessage("Employee count must be greater than 0")
            .When(x => x.EmployeeCount.HasValue);

        RuleFor(x => x.AnnualRevenue)
            .GreaterThan(0).WithMessage("Annual revenue must be greater than 0")
            .When(x => x.AnnualRevenue.HasValue);
    }
}
