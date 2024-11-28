using Askify.Core.Users.Entities;

namespace Askify.Core.Questions.Entities;

public class Question
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string Title { get; set; }
    public string Content { get; set; }
    public DateTimeOffset CreatedAt { get; init; } = DateTimeOffset.Now;
    public Guid UserId { get; set; }
    public User User { get; init; }

    private Question()
    {
    }

    public static Question Create() => new();

    public void ChangeInformation(string title, string content)
    {
        Title = title;
        Content = content;
    }
}