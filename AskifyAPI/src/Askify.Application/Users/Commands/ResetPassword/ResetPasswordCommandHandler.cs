using Askify.Core.Users.Exceptions;
using Askify.Core.Users.Repositories;
using Askify.Shared.Hash;

namespace Askify.Application.Users.Commands.ResetPassword;

internal sealed class ResetPasswordCommandHandler(
    IUserRepository userRepository,
    IPasswordHasher passwordHasher) : IRequestHandler<ResetPasswordCommand>
{
    public async Task Handle(ResetPasswordCommand command, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByEmailAsync(command.Email, cancellationToken);
        if (user is null)
            throw new UserException.UserNotFoundException(command.Email);
        
        var hashedPassword = passwordHasher.HashPassword(command.NewPassword);
        user.ChangePassword(hashedPassword);
        
        await userRepository.SaveChangesAsync(cancellationToken);
    }
}