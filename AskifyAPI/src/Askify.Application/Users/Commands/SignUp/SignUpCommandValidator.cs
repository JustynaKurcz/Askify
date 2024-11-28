using FluentValidation;

namespace Askify.Application.Users.Commands.SignUp;

internal sealed class SignUpCommandValidator : AbstractValidator<SignUpCommand>
{
    public SignUpCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email cannot be empty")
            .NotNull().WithMessage("Email cannot be null")
            .EmailAddress().WithMessage("Email is not valid");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password cannot be empty")
            .NotNull().WithMessage("Password cannot be null")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters long")
            .MaximumLength(16).WithMessage("Password must be at most 16 characters long");

        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("UserName cannot be empty")
            .NotNull().WithMessage("UserName cannot be null")
            .MaximumLength(150).WithMessage("UserName must be at most 150 characters long");
    }
}