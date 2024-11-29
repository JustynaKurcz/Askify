using Askify.Application.Users.Queries.Admin.BrowseUsers;
using Askify.Application.Users.Queries.GetCurrentLoggedUser.DTO;
using Askify.Core.Users.Entities;
using Askify.Core.Users.Repositories;

namespace Askify.Infrastructure.EF.Users.Queries.BrowseUsers;

internal sealed class BrowseUsersQueryHandler(
    IUserRepository userRepository
) : IRequestHandler<BrowseUsersQuery, PagedResponse<UserDetailsDto>>
{
    public async Task<PagedResponse<UserDetailsDto>> Handle(BrowseUsersQuery query, CancellationToken cancellationToken)
    {
        var users = await userRepository.GetAll(cancellationToken);

        users = Search(query, users);

        return await users
            .Select(x => x.AsDetailsDto())
            .PaginateAsync(query.GetPageNumber(), query.GetPageSize(), cancellationToken);
    }

    private IQueryable<User> Search(BrowseUsersQuery query, IQueryable<User> users)
    {
        if (string.IsNullOrWhiteSpace(query.Search)) return users;
        var searchTxt = $"%{query.Search}%";
        return users.Where(user =>
            Microsoft.EntityFrameworkCore.EF.Functions.ILike(user.Email, searchTxt) ||
            Microsoft.EntityFrameworkCore.EF.Functions.ILike(user.UserName, searchTxt));
    }
}