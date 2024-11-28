using Askify.Core.Questions.Entities;

namespace Askify.Core.Answers.Entities;

public class Answer
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string Content { get; set; }
    public DateTimeOffset CreatedAt { get; init; } = DateTimeOffset.UtcNow;
    public Guid UserId { get; set; }
    public Guid QuestionId { get; set; }
    public Question Question { get; init; }

    private Answer()
    {
    }

    public static Answer Create() => new();

    public void ChangeContent(string content)
    {
        Content = content;
    }
}