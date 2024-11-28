using FluentValidation;

namespace Askify.Application.Questions.Commands.CreateQuestion;

internal sealed class CreateQuestionCommandValidator : AbstractValidator<CreateQuestionCommand>
{
    public CreateQuestionCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required")
            .NotNull().WithMessage("Title is required")
            .MaximumLength(250).WithMessage("Title must not exceed 250 characters");

        RuleFor(x => x.Content)
            .NotEmpty().WithMessage("Content is required")
            .NotNull().WithMessage("Content is required")
            .MaximumLength(5000).WithMessage("Content must not exceed 5000 characters");
    }
}