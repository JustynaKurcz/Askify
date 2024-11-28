using Askify.Core.Users.Enums;

namespace Askify.Core.Users.Entities;

public class UserBuilder
{
    private readonly User _user = User.Create();

    public User Build() => _user;

    public UserBuilder WithEmail(string email)
    {
        _user.Email = email;
        return this;
    }

    public UserBuilder WithUserName(string userName)
    {
        _user.UserName = userName;
        return this;
    }

    public UserBuilder WithPassword(string password)
    {
        _user.Password = password;
        return this;
    }
    
    public UserBuilder WithRole(Role role)
    {
        _user.Role = role;
        return this;
    }
}