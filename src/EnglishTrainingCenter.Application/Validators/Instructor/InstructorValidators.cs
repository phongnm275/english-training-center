using EnglishTrainingCenter.Application.DTOs.Instructor;
using FluentValidation;

namespace EnglishTrainingCenter.Application.Validators.Instructor;

/// <summary>
/// Validator for creating a new instructor
/// </summary>
public class CreateInstructorRequestValidator : AbstractValidator<CreateInstructorRequest>
{
    public CreateInstructorRequestValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required")
            .Length(2, 50).WithMessage("First name must be between 2 and 50 characters");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required")
            .Length(2, 50).WithMessage("Last name must be between 2 and 50 characters");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Email must be in valid format")
            .MaximumLength(100).WithMessage("Email must not exceed 100 characters");

        RuleFor(x => x.PhoneNumber)
            .Matches(@"^\+?[0-9\s\-\(\)]{10,}$")
            .When(x => !string.IsNullOrEmpty(x.PhoneNumber))
            .WithMessage("Phone number must be in valid format");

        RuleFor(x => x.Specialization)
            .NotEmpty().WithMessage("Specialization is required")
            .Length(3, 100).WithMessage("Specialization must be between 3 and 100 characters");

        RuleFor(x => x.Qualification)
            .NotEmpty().WithMessage("Qualification is required")
            .Must(q => new[] { "High School", "Bachelor", "Master", "PhD" }.Contains(q))
            .WithMessage("Qualification must be one of: High School, Bachelor, Master, PhD");

        RuleFor(x => x.YearsOfExperience)
            .GreaterThanOrEqualTo(0).WithMessage("Years of experience cannot be negative")
            .LessThanOrEqualTo(70).WithMessage("Years of experience cannot exceed 70");

        RuleFor(x => x.BaseSalary)
            .GreaterThan(0).WithMessage("Base salary must be greater than zero")
            .LessThanOrEqualTo(10000000).WithMessage("Base salary cannot exceed 10,000,000");

        RuleFor(x => x.SalaryFrequency)
            .NotEmpty().WithMessage("Salary frequency is required")
            .Must(f => new[] { "Monthly", "Quarterly", "Yearly" }.Contains(f))
            .WithMessage("Salary frequency must be one of: Monthly, Quarterly, Yearly");
    }
}

/// <summary>
/// Validator for updating an instructor
/// </summary>
public class UpdateInstructorRequestValidator : AbstractValidator<UpdateInstructorRequest>
{
    public UpdateInstructorRequestValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required")
            .Length(2, 50).WithMessage("First name must be between 2 and 50 characters");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required")
            .Length(2, 50).WithMessage("Last name must be between 2 and 50 characters");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Email must be in valid format")
            .MaximumLength(100).WithMessage("Email must not exceed 100 characters");

        RuleFor(x => x.PhoneNumber)
            .Matches(@"^\+?[0-9\s\-\(\)]{10,}$")
            .When(x => !string.IsNullOrEmpty(x.PhoneNumber))
            .WithMessage("Phone number must be in valid format");

        RuleFor(x => x.Specialization)
            .NotEmpty().WithMessage("Specialization is required")
            .Length(3, 100).WithMessage("Specialization must be between 3 and 100 characters");

        RuleFor(x => x.Qualification)
            .NotEmpty().WithMessage("Qualification is required")
            .Must(q => new[] { "High School", "Bachelor", "Master", "PhD" }.Contains(q))
            .WithMessage("Qualification must be one of: High School, Bachelor, Master, PhD");

        RuleFor(x => x.YearsOfExperience)
            .GreaterThanOrEqualTo(0).WithMessage("Years of experience cannot be negative")
            .LessThanOrEqualTo(70).WithMessage("Years of experience cannot exceed 70");

        RuleFor(x => x.BaseSalary)
            .GreaterThan(0).WithMessage("Base salary must be greater than zero")
            .LessThanOrEqualTo(10000000).WithMessage("Base salary cannot exceed 10,000,000");

        RuleFor(x => x.SalaryFrequency)
            .NotEmpty().WithMessage("Salary frequency is required")
            .Must(f => new[] { "Monthly", "Quarterly", "Yearly" }.Contains(f))
            .WithMessage("Salary frequency must be one of: Monthly, Quarterly, Yearly");
    }
}
