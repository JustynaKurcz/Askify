using Askify.Application.Users.Queries.GetUserName;
using Askify.Core.Users.Exceptions;
using Askify.Core.Users.Repositories;
using Askify.Shared.Auth.Context;

namespace Askify.Infrastructure.EF.Users.Queries.GetUserName;

internal sealed class GetUserNameQueryHandler(
    IContext context,
    IUserRepository userRepository
) : IRequestHandler<GetUserNameQuery, string>
{
    public async Task<string> Handle(GetUserNameQuery query, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetAsync(query.UserId, cancellationToken);

        if (user is null)
            throw new UserException.UserNotFoundException(query.UserId);
       
        return user.UserName;
    }
}