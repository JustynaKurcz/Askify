using Askify.Core.Users.Entities;
using Askify.Core.Users.Enums;
using Askify.Core.Users.Exceptions;
using Askify.Core.Users.Repositories;
using Askify.Shared.Hash;

namespace Askify.Application.Users.Commands.SignUp;

internal sealed class SignUpCommandHandler(
    IUserRepository userRepository,
    IPasswordHasher passwordHasher)
    : IRequestHandler<SignUpCommand, SignUpResponse>
{
    public async Task<SignUpResponse> Handle(SignUpCommand command,
        CancellationToken cancellationToken)
    {
        var userExists = await userRepository
            .AnyAsync(
                x => x.UserName == command.UserName,
                cancellationToken
            );

        if (userExists)
            throw new UserException.UserAlreadyExistsException();

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