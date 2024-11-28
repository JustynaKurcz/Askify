using Askify.Core.Users.Exceptions;
using Askify.Core.Users.Repositories;
using Askify.Shared.Auth;
using Askify.Shared.Hash;
using MediatR;
using Microsoft.OpenApi.Extensions;

namespace Askify.Application.Users.Commands.SignIn;

internal sealed class SignInCommandHandler(
    IUserRepository userRepository,
    IPasswordHasher passwordHasher,
    IAuthManager authManager)
    : IRequestHandler<SignInCommand, SignInResponse>
{
    public async Task<SignInResponse> Handle(SignInCommand command,
        CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByEmailAsync(command.Email, cancellationToken);

        if (user is null)
            throw new UserException.UserNotFoundException(command.Email);

        var isValidPassword = passwordHasher.VerifyHashedPassword(user.Password, command.Password);

        if (!isValidPassword)
            throw new UserException.InvalidPasswordException();

        var jwt = authManager.GenerateToken(user.Id, user.Role.GetDisplayName());

        return new SignInResponse(jwt.AccessToken);
    }
}