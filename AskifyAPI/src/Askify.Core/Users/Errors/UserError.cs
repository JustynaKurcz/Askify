using Askify.Shared.Results;

namespace Askify.Core.Users.Errors;

public static class UserError
{
    public static readonly Error UsernameInUse = new
        ("Users.UsernameInUse", "The username is already in use.");

    public static Error UserNotFound(string email) => new
        ("Users.UserNotFound", $"User with email '{email}' was not found.");
    
    public static Error UserNotFound(Guid  userId) => new
        ("Users.UserNotFound", $"User with id '{userId}' was not found.");
    
    public static readonly Error InvalidPassword = new
        ("Users.InvalidPassword", "The password is invalid.");
}