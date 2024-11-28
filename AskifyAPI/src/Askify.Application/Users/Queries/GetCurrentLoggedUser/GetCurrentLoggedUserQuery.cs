using Askify.Application.Users.Queries.DTO;
using MediatR;

namespace Askify.Application.Users.Queries.GetCurrentLoggedUser;

public record GetCurrentLoggedUserQuery : IRequest<UserDetailsDto>;