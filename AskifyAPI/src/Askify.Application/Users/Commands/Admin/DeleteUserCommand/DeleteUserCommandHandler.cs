using Askify.Core.Users.Exceptions;
using Askify.Core.Users.Repositories;

namespace Askify.Application.Users.Commands.Admin.DeleteUserCommand;

internal sealed class DeleteUserCommandHandler(
    IUserRepository userRepository
) : IRequestHandler<DeleteUserCommand>
{
    public async Task Handle(DeleteUserCommand command, CancellationToken cancellationToken)
    {
        var user = await userRepository
            .GetAsync(command.UserId, cancellationToken);

        if (user is null)
            throw new UserException.UserNotFoundException(command.UserId);

        await userRepository.DeleteAsync(user);
        await userRepository.SaveChangesAsync(cancellationToken);
    }
}