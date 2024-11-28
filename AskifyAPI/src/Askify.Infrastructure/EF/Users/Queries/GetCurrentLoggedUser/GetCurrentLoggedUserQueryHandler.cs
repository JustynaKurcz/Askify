using Askify.Application.Users.Queries.DTO;
using Askify.Application.Users.Queries.GetCurrentLoggedUser;
using Askify.Core.Users.Exceptions;
using Askify.Core.Users.Repositories;
using Askify.Shared.Auth.Context;
using Askify.Shared.Results;
using MediatR;

namespace Askify.Infrastructure.EF.Users.Queries.GetCurrentLoggedUser;

internal sealed class GetCurrentLoggedUserQueryHandler(
    IContext context,
    IUserRepository userRepository
) : IRequestHandler<GetCurrentLoggedUserQuery, Result<UserDetailsDto, Error>>
{
    public async Task<Result<UserDetailsDto, Error>> Handle(GetCurrentLoggedUserQuery query,
        CancellationToken cancellationToken)
    {
        var userId = context.Identity.Id;
        var user = await userRepository.GetAsync(userId, cancellationToken);

        if (user is null)
            throw new UserException.UserNotFoundException(userId);

        return user.AsDetailsDto();
    }
}