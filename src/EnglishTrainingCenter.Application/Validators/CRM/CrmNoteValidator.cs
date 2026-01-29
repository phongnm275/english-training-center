using FluentValidation;
using EnglishTrainingCenter.Application.DTOs.CRM;

namespace EnglishTrainingCenter.Application.Validators.CRM;

/// <summary>
/// Validator for creating CRM note
/// </summary>
public class CreateCrmNoteValidator : AbstractValidator<CreateCrmNoteDto>
{
    public CreateCrmNoteValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required")
            .MaximumLength(200).WithMessage("Title must not exceed 200 characters");

        RuleFor(x => x.Content)
            .NotEmpty().WithMessage("Content is required")
            .MaximumLength(5000).WithMessage("Content must not exceed 5000 characters");

        RuleFor(x => x.Tags)
            .MaximumLength(500).WithMessage("Tags must not exceed 500 characters")
            .When(x => !string.IsNullOrEmpty(x.Tags));
    }
}

/// <summary>
/// Validator for updating CRM note
/// </summary>
public class UpdateCrmNoteValidator : AbstractValidator<UpdateCrmNoteDto>
{
    public UpdateCrmNoteValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required")
            .MaximumLength(200).WithMessage("Title must not exceed 200 characters");

        RuleFor(x => x.Content)
            .NotEmpty().WithMessage("Content is required")
            .MaximumLength(5000).WithMessage("Content must not exceed 5000 characters");

        RuleFor(x => x.Tags)
            .MaximumLength(500).WithMessage("Tags must not exceed 500 characters")
            .When(x => !string.IsNullOrEmpty(x.Tags));
    }
}
