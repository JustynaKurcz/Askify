using Askify.Core.Users.Exceptions;
using Askify.Core.Users.Repositories;
using Askify.Shared.Auth;
using Askify.Shared.Emails;

namespace Askify.Application.Users.Commands.ForgotPassword;

internal sealed class ForgotPasswordCommandHandler(
    IUserRepository userRepository,
    IEmailService emailService,
    IAuthManager authManager
) : IRequestHandler<ForgotPasswordCommand>
{
    public async Task Handle(ForgotPasswordCommand command, CancellationToken cancellationToken)
    {
        var email = command.Email.ToLowerInvariant();
        var user = await userRepository.GetByEmailAsync(email, cancellationToken)
                   ?? throw new UserException.UserNotFoundException(email);

        var resetToken = authManager.GeneratePasswordResetToken(user.Email);

        await emailService.SendPasswordResetEmailAsync(user.Email, resetToken, cancellationToken);
    }
}