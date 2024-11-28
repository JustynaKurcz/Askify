using FluentValidation;

namespace Askify.Application.Answers.Command.ChangeAnswerInformation;

internal sealed class ChangeAnswerInformationCommandValidator : AbstractValidator<ChangeAnswerInformationCommand>
{
    public ChangeAnswerInformationCommandValidator()
    {
        RuleFor(x => x.Content)
            .NotEmpty().WithMessage("Content is required")
            .NotNull().WithMessage("Content is required")
            .MaximumLength(5000).WithMessage("Content must not exceed 5000 characters");
    }
}