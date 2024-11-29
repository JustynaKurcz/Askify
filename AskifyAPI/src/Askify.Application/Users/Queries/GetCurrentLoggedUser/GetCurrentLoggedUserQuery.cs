using Askify.Application.Users.Queries.GetCurrentLoggedUser.DTO;

namespace Askify.Application.Users.Queries.GetCurrentLoggedUser;

public record GetCurrentLoggedUserQuery : IRequest<UserDetailsDto>;