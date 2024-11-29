using Askify.Core.Users.Repositories;
using Askify.Shared.Auth.Context;

namespace Askify.Application.Users.Commands.DeleteUser;

internal sealed class DeleteUserCommandHandler(
    IUserRepository userRepository,
    IContext context
) : IRequestHandler<DeleteUserCommand>
{
    public async Task Handle(DeleteUserCommand command,
        CancellationToken cancellationToken)
    {
        var userId = context.Identity.Id;

        var user = await userRepository
            .GetAsync(userId, cancellationToken);

        await userRepository.DeleteAsync(user!);
        await userRepository.SaveChangesAsync(cancellationToken);
    }
}