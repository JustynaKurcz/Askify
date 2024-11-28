using Askify.Core.Users.Errors;
using Askify.Core.Users.Repositories;
using Askify.Shared.Auth;
using Askify.Shared.Hash;
using Askify.Shared.Results;
using MediatR;
using Microsoft.OpenApi.Extensions;

namespace Askify.Application.Users.Commands.SignIn;

internal sealed class SignInCommandHandler(
    IUserRepository userRepository,
    IPasswordHasher passwordHasher,
    IAuthManager authManager)
    : IRequestHandler<SignInCommand, Result<SignInResponse, Error>>
{
    public async Task<Result<SignInResponse, Error>> Handle(SignInCommand command,
        CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByEmailAsync(command.Email, cancellationToken);

        if (user is null)
        {
            return UserError.UserNotFound(command.Email);
        }

        var isValidPassword = passwordHasher.VerifyHashedPassword(user.Password, command.Password);

        if (!isValidPassword)
        {
            return UserError.InvalidPassword;
        }

        var jwt = authManager.GenerateToken(user.Id, user.Role.GetDisplayName());

        return new SignInResponse(jwt.AccessToken);
    }
}