using FluentValidation;

namespace Askify.Application.Questions.Commands.ChangeQuestionInformation;

internal sealed class ChangeQuestionInformationCommandValidator : AbstractValidator<ChangeQuestionInformationCommand>
{
    public ChangeQuestionInformationCommandValidator()
    {
        RuleFor(x => x.QuestionId)
            .NotEmpty().WithMessage("QuestionId is required.")
            .NotNull().WithMessage("QuestionId is required.")
            .NotEqual(Guid.Empty).WithMessage("QuestionId is required.");

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .NotNull().WithMessage("Title is required.")
            .MaximumLength(250).WithMessage("Title must not exceed 250 characters.");

        RuleFor(x => x.Content)
            .NotEmpty().WithMessage("Content is required")
            .NotNull().WithMessage("Content is required")
            .MaximumLength(5000).WithMessage("Content must not exceed 5000 characters");

        RuleFor(x => x.Tag)
            .IsInEnum().WithMessage("Tag is invalid.");
    }
}