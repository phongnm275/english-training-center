using FluentValidation;
using EnglishTrainingCenter.Application.DTOs;

namespace EnglishTrainingCenter.Application.Validators;

/// <summary>
/// Validator for CreateStudentRequest
/// </summary>
public class CreateStudentRequestValidator : AbstractValidator<CreateStudentRequest>
{
    public CreateStudentRequestValidator()
    {
        RuleFor(x => x.FullName)
            .NotEmpty().WithMessage("Full name is required")
            .MaximumLength(100).WithMessage("Full name must not exceed 100 characters");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email format");

        RuleFor(x => x.Phone)
            .Matches(@"^[0-9\-\+\(\)]{7,15}$")
            .When(x => !string.IsNullOrEmpty(x.Phone))
            .WithMessage("Phone number format is invalid");

        RuleFor(x => x.DateOfBirth)
            .LessThan(DateTime.UtcNow)
            .When(x => x.DateOfBirth.HasValue)
            .WithMessage("Date of birth must be in the past");
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
            .NotEmpty().WithMessage("Full name is required")
            .MaximumLength(100).WithMessage("Full name must not exceed 100 characters");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email format");

        RuleFor(x => x.Phone)
            .Matches(@"^[0-9\-\+\(\)]{7,15}$")
            .When(x => !string.IsNullOrEmpty(x.Phone))
            .WithMessage("Phone number format is invalid");
    }
}

/// <summary>
/// Validator for CreateCourseRequest
/// </summary>
public class CreateCourseRequestValidator : AbstractValidator<CourseDto>
{
    public CreateCourseRequestValidator()
    {
        RuleFor(x => x.CourseName)
            .NotEmpty().WithMessage("Course name is required")
            .MaximumLength(100).WithMessage("Course name must not exceed 100 characters");

        RuleFor(x => x.Duration)
            .GreaterThan(0)
            .When(x => x.Duration.HasValue)
            .WithMessage("Duration must be greater than 0");

        RuleFor(x => x.Fee)
            .GreaterThan(0)
            .When(x => x.Fee.HasValue)
            .WithMessage("Fee must be greater than 0");
    }
}
