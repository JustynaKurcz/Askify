using Askify.Core.Questions.Entities;
using Askify.Core.Users.Enums;

namespace Askify.Core.Users.Entities;

public class User
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string Email { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public DateTimeOffset CreatedAt { get; init; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? UpdatedAt { get; set; }
    public Role Role { get; set; }
    public ICollection<Question> Questions { get; set; } = [];

    private User()
    {
    }

    public static User Create() => new();
}