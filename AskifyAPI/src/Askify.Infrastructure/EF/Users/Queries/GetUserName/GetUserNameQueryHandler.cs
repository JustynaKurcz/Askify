using Askify.Application.Users.Queries.GetUserName;
using Askify.Core.Users.Exceptions;
using Askify.Core.Users.Repositories;

namespace Askify.Infrastructure.EF.Users.Queries.GetUserName;

internal sealed class GetUserNameQueryHandler(
    IUserRepository userRepository
) : IRequestHandler<GetUserNameQuery, string>
{
    public async Task<string> Handle(GetUserNameQuery query, CancellationToken cancellationToken)
    {
        var userName = await userRepository.GetUserName(query.UserId, cancellationToken);

        if (userName is null)
            throw new UserException.UserNotFoundException(query.UserId);
       
        return userName;
    }
}