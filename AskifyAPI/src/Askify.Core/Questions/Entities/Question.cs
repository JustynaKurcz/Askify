using Askify.Core.Answers.Entities;
using Askify.Core.Questions.Enums;
using Askify.Core.Users.Entities;

namespace Askify.Core.Questions.Entities;

public class Question
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string Title { get; set; }
    public string Content { get; set; }
    public Tag Tag { get; set; }
    public DateTimeOffset CreatedAt { get; init; } = DateTimeOffset.UtcNow;
    public Guid UserId { get; set; }
    public User User { get; init; }
    public ICollection<Answer> Answers { get; set; } = [];

    private Question()
    {
    }

    public static Question Create() => new();

    public void ChangeInformation(string title, string content, Tag tag)
    {
        Title = title;
        Content = content;
        Tag = tag;
    }
}