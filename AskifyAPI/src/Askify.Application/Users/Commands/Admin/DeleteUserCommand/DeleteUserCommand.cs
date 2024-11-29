namespace Askify.Application.Users.Commands.Admin.DeleteUserCommand;

public record DeleteUserCommand(Guid UserId) : IRequest;