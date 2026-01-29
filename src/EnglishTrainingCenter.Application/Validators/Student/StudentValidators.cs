using EnglishTrainingCenter.Application.DTOs.Student;
using FluentValidation;

namespace EnglishTrainingCenter.Application.Validators.Student;

/// <summary>
/// Validator for CreateStudentRequest
/// </summary>
public class CreateStudentRequestValidator : AbstractValidator<CreateStudentRequest>
{
    public CreateStudentRequestValidator()
    {
        RuleFor(x => x.UserId)
            .GreaterThan(0)
            .WithMessage("User ID must be greater than 0");

        RuleFor(x => x.FullName)
            .NotEmpty()
            .WithMessage("Full name is required")
            .Length(2, 256)
            .WithMessage("Full name must be between 2 and 256 characters");

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email is required")
            .EmailAddress()
            .WithMessage("Invalid email address")
            .MaximumLength(256)
            .WithMessage("Email cannot exceed 256 characters");

        RuleFor(x => x.Phone)
            .MaximumLength(20)
            .WithMessage("Phone number cannot exceed 20 characters")
            .Matches(@"^[\d\s\-\+\(\)]+$", null)
            .When(x => !string.IsNullOrEmpty(x.Phone))
            .WithMessage("Phone number contains invalid characters");

        RuleFor(x => x.DateOfBirth)
            .LessThan(DateTime.Today)
            .When(x => x.DateOfBirth.HasValue)
            .WithMessage("Date of birth must be in the past")
            .GreaterThan(DateTime.Today.AddYears(-120))
            .When(x => x.DateOfBirth.HasValue)
            .WithMessage("Date of birth is unrealistic");

        RuleFor(x => x.Address)
            .MaximumLength(500)
            .WithMessage("Address cannot exceed 500 characters");

        RuleFor(x => x.Avatar)
            .MaximumLength(500)
            .WithMessage("Avatar URL cannot exceed 500 characters")
            .Matches(@"^(https?://)?[\w\-]+(\.[\w\-]+)+[/#?]?.*$", null)
            .When(x => !string.IsNullOrEmpty(x.Avatar))
            .WithMessage("Avatar must be a valid URL");
    }
}

/// <summary>
/// Validator for UpdateStudentRequest
/// </summary>
public class UpdateStudentRequestValidator : AbstractValidator<UpdateStudentRequest>
{
    public UpdateStudentRequestValidator()
    {
        RuleFor(x => x.FullName)
            .NotEmpty()
            .WithMessage("Full name is required")
            .Length(2, 256)
            .WithMessage("Full name must be between 2 and 256 characters");

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email is required")
            .EmailAddress()
            .WithMessage("Invalid email address")
            .MaximumLength(256)
            .WithMessage("Email cannot exceed 256 characters");

        RuleFor(x => x.Phone)
            .MaximumLength(20)
            .WithMessage("Phone number cannot exceed 20 characters")
            .Matches(@"^[\d\s\-\+\(\)]+$", null)
            .When(x => !string.IsNullOrEmpty(x.Phone))
            .WithMessage("Phone number contains invalid characters");

        RuleFor(x => x.DateOfBirth)
            .LessThan(DateTime.Today)
            .When(x => x.DateOfBirth.HasValue)
            .WithMessage("Date of birth must be in the past")
            .GreaterThan(DateTime.Today.AddYears(-120))
            .When(x => x.DateOfBirth.HasValue)
            .WithMessage("Date of birth is unrealistic");

        RuleFor(x => x.Address)
            .MaximumLength(500)
            .WithMessage("Address cannot exceed 500 characters");

        RuleFor(x => x.Avatar)
            .MaximumLength(500)
            .WithMessage("Avatar URL cannot exceed 500 characters")
            .Matches(@"^(https?://)?[\w\-]+(\.[\w\-]+)+[/#?]?.*$", null)
            .When(x => !string.IsNullOrEmpty(x.Avatar))
            .WithMessage("Avatar must be a valid URL");
    }
}
