using Askify.Core.Users.Entities;
using Askify.Core.Users.Enums;
using Askify.Core.Users.Errors;
using Askify.Core.Users.Repositories;
using Askify.Shared.Hash;
using Askify.Shared.Results;
using MediatR;

namespace Askify.Application.Users.Commands.SignUp;

internal sealed class SignUpCommandHandler(
    IUserRepository userRepository,
    IPasswordHasher passwordHasher)
    : IRequestHandler<SignUpCommand, Result<SignUpResponse, Error>>
{
    public async Task<Result<SignUpResponse, Error>> Handle(SignUpCommand command,
        CancellationToken cancellationToken)
    {
        var userExists = await userRepository
            .AnyAsync(
                x => x.UserName == command.UserName,
                cancellationToken
            );

        if (userExists)
        {
            return UserError.UsernameInUse;
        }

        var hashedPassword = passwordHasher.HashPassword(command.Password);

        var user = new UserBuilder()
            .WithEmail(command.Email)
            .WithUserName(command.UserName)
            .WithPassword(hashedPassword)
            .WithRole(Role.User)
            .Build();

        await userRepository.AddAsync(user, cancellationToken);
        await userRepository.SaveChangesAsync(cancellationToken);

        return new SignUpResponse(user.Id);
    }
}