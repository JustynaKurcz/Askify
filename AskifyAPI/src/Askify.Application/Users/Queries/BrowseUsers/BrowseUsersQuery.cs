using Askify.Application.Users.Queries.GetCurrentLoggedUser.DTO;

namespace Askify.Application.Users.Queries.BrowseUsers;

public sealed class BrowseUsersQuery : PagedQuery, IRequest<PagedResponse<UserDetailsDto>>
{
    [SwaggerParameter("Search filter - matches against user's email or username")]
    public string? Search { get; set; }
}