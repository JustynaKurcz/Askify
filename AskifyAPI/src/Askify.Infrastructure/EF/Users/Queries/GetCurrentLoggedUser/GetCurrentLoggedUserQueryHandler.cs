using Askify.Application.Users.Queries.DTO;
using Askify.Application.Users.Queries.GetCurrentLoggedUser;
using Askify.Core.Users.Exceptions;
using Askify.Core.Users.Repositories;
using Askify.Shared.Auth.Context;
using MediatR;

namespace Askify.Infrastructure.EF.Users.Queries.GetCurrentLoggedUser;

internal sealed class GetCurrentLoggedUserQueryHandler(
    IContext context,
    IUserRepository userRepository
) : IRequestHandler<GetCurrentLoggedUserQuery, UserDetailsDto>
{
    public async Task<UserDetailsDto> Handle(GetCurrentLoggedUserQuery query,
        CancellationToken cancellationToken)
    {
        var userId = context.Identity.Id;
        var user = await userRepository.GetAsync(userId, cancellationToken);

        if (user is null)
            throw new UserException.UserNotFoundException(userId);

        return user.AsDetailsDto();
    }
}