using MediatR;

namespace Askify.Application.Users.Queries.GetUserName;

public record GetUserNameQuery(Guid UserId) : IRequest<string>;