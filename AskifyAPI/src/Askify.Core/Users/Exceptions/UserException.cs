using Askify.Shared.Results;

namespace Askify.Core.Users.Exceptions;

public sealed class UserException
{
    public sealed class UserAlreadyExistsException : AskifyException
    {
        public UserAlreadyExistsException() : base($"User already exists.")
        {
        }
    }

    public sealed class UserNotFoundException : AskifyException
    {
        public UserNotFoundException(string email) : base($"User with email '{email}' was not found.")
        {
        }

        public UserNotFoundException(Guid userId) : base($"User with id '{userId}' was not found.")
        {
        }
    }

    public sealed class InvalidPasswordException : AskifyException
    {
        public InvalidPasswordException() : base($"The password is invalid.")
        {
        }
    }
}