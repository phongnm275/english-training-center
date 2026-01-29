using FluentValidation;
using EnglishTrainingCenter.Application.DTOs.Course;

namespace EnglishTrainingCenter.Application.Validators.Course;

/// <summary>
/// Validator for CreateCourseRequest.
/// </summary>
public class CreateCourseRequestValidator : AbstractValidator<CreateCourseRequest>
{
    public CreateCourseRequestValidator()
    {
        RuleFor(x => x.CourseCode)
            .NotEmpty().WithMessage("Course code is required")
            .Length(3, 20).WithMessage("Course code must be between 3 and 20 characters")
            .Matches(@"^[A-Z]{3,}\d{2,}$").WithMessage("Course code must be uppercase letters followed by numbers (e.g., ENG101)");

        RuleFor(x => x.CourseName)
            .NotEmpty().WithMessage("Course name is required")
            .Length(3, 256).WithMessage("Course name must be between 3 and 256 characters");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required")
            .Length(10, 2000).WithMessage("Description must be between 10 and 2000 characters");

        RuleFor(x => x.Level)
            .NotEmpty().WithMessage("Level is required")
            .Must(x => new[] { "Beginner", "Intermediate", "Advanced", "Professional" }
                .Contains(x, StringComparer.OrdinalIgnoreCase))
            .WithMessage("Level must be one of: Beginner, Intermediate, Advanced, Professional");

        RuleFor(x => x.DurationHours)
            .GreaterThan(0).WithMessage("Duration must be greater than 0")
            .LessThanOrEqualTo(1000).WithMessage("Duration cannot exceed 1000 hours");

        RuleFor(x => x.MaxCapacity)
            .GreaterThan(0).WithMessage("Maximum capacity must be greater than 0")
            .LessThanOrEqualTo(100).WithMessage("Maximum capacity cannot exceed 100 students");

        RuleFor(x => x.Fee)
            .GreaterThanOrEqualTo(0).WithMessage("Fee cannot be negative")
            .LessThanOrEqualTo(10000000).WithMessage("Fee cannot exceed 10,000,000");

        RuleFor(x => x.StartDate)
            .NotEmpty().WithMessage("Start date is required")
            .GreaterThan(DateTime.UtcNow.AddDays(-1)).WithMessage("Start date must be today or in the future");

        RuleFor(x => x.EndDate)
            .NotEmpty().WithMessage("End date is required")
            .GreaterThan(x => x.StartDate).WithMessage("End date must be after start date");
    }
}

/// <summary>
/// Validator for UpdateCourseRequest.
/// </summary>
public class UpdateCourseRequestValidator : AbstractValidator<UpdateCourseRequest>
{
    public UpdateCourseRequestValidator()
    {
        RuleFor(x => x.CourseCode)
            .NotEmpty().WithMessage("Course code is required")
            .Length(3, 20).WithMessage("Course code must be between 3 and 20 characters")
            .Matches(@"^[A-Z]{3,}\d{2,}$").WithMessage("Course code must be uppercase letters followed by numbers (e.g., ENG101)");

        RuleFor(x => x.CourseName)
            .NotEmpty().WithMessage("Course name is required")
            .Length(3, 256).WithMessage("Course name must be between 3 and 256 characters");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required")
            .Length(10, 2000).WithMessage("Description must be between 10 and 2000 characters");

        RuleFor(x => x.Level)
            .NotEmpty().WithMessage("Level is required")
            .Must(x => new[] { "Beginner", "Intermediate", "Advanced", "Professional" }
                .Contains(x, StringComparer.OrdinalIgnoreCase))
            .WithMessage("Level must be one of: Beginner, Intermediate, Advanced, Professional");

        RuleFor(x => x.DurationHours)
            .GreaterThan(0).WithMessage("Duration must be greater than 0")
            .LessThanOrEqualTo(1000).WithMessage("Duration cannot exceed 1000 hours");

        RuleFor(x => x.MaxCapacity)
            .GreaterThan(0).WithMessage("Maximum capacity must be greater than 0")
            .LessThanOrEqualTo(100).WithMessage("Maximum capacity cannot exceed 100 students");

        RuleFor(x => x.Fee)
            .GreaterThanOrEqualTo(0).WithMessage("Fee cannot be negative")
            .LessThanOrEqualTo(10000000).WithMessage("Fee cannot exceed 10,000,000");

        RuleFor(x => x.StartDate)
            .NotEmpty().WithMessage("Start date is required")
            .GreaterThan(DateTime.UtcNow.AddDays(-1)).WithMessage("Start date must be today or in the future");

        RuleFor(x => x.EndDate)
            .NotEmpty().WithMessage("End date is required")
            .GreaterThan(x => x.StartDate).WithMessage("End date must be after start date");
    }
}
