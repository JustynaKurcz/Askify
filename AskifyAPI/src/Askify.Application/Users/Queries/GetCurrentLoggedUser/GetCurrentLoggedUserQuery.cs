using Askify.Application.Users.Queries.DTO;
using Askify.Shared.Results;
using MediatR;

namespace Askify.Application.Users.Queries.GetCurrentLoggedUser;

public record GetCurrentLoggedUserQuery : IRequest<Result<UserDetailsDto, Error>>;