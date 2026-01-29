using FluentValidation;
using EnglishTrainingCenter.Application.DTOs.Grade;

namespace EnglishTrainingCenter.Application.Validators.Grade;

/// <summary>
/// Validator for CreateGradeRequest.
/// </summary>
public class CreateGradeRequestValidator : AbstractValidator<CreateGradeRequest>
{
    public CreateGradeRequestValidator()
    {
        RuleFor(x => x.StudentId)
            .GreaterThan(0).WithMessage("Student ID must be greater than 0");

        RuleFor(x => x.CourseId)
            .GreaterThan(0).WithMessage("Course ID must be greater than 0");

        RuleFor(x => x.Grade)
            .NotEmpty().WithMessage("Grade is required")
            .Must(x => new[] { "A", "B", "C", "D", "F" }
                .Contains(x.ToUpper()))
            .WithMessage("Grade must be one of: A, B, C, D, F");

        RuleFor(x => x.NumericScore)
            .GreaterThanOrEqualTo(0).WithMessage("Numeric score cannot be negative")
            .LessThanOrEqualTo(100).WithMessage("Numeric score cannot exceed 100")
            .When(x => x.NumericScore.HasValue);

        RuleFor(x => x.Comments)
            .MaximumLength(500).WithMessage("Comments cannot exceed 500 characters");
    }
}

/// <summary>
/// Validator for UpdateGradeRequest.
/// </summary>
public class UpdateGradeRequestValidator : AbstractValidator<UpdateGradeRequest>
{
    public UpdateGradeRequestValidator()
    {
        RuleFor(x => x.Grade)
            .NotEmpty().WithMessage("Grade is required")
            .Must(x => new[] { "A", "B", "C", "D", "F" }
                .Contains(x.ToUpper()))
            .WithMessage("Grade must be one of: A, B, C, D, F");

        RuleFor(x => x.NumericScore)
            .GreaterThanOrEqualTo(0).WithMessage("Numeric score cannot be negative")
            .LessThanOrEqualTo(100).WithMessage("Numeric score cannot exceed 100")
            .When(x => x.NumericScore.HasValue);

        RuleFor(x => x.Comments)
            .MaximumLength(500).WithMessage("Comments cannot exceed 500 characters");
    }
}
