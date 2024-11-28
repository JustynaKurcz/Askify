using MediatR;

namespace Askify.Application.Users.Commands.SignUp;

public record SignUpCommand(string Email, string UserName, string Password)
    : IRequest<SignUpResponse>;