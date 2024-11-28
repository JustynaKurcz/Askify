using Askify.Core.Questions.Enums;

namespace Askify.Core.Questions.Entities;

public class QuestionBuilder
{
    private readonly Question _question = Question.Create();

    public Question Build() => _question;

    public QuestionBuilder WithTitle(string title)
    {
        _question.Title = title;
        return this;
    }

    public QuestionBuilder WithContent(string content)
    {
        _question.Content = content;
        return this;
    }

    public QuestionBuilder WithTag(Tag tag)
    {
        _question.Tag = tag;
        return this;
    }

    public QuestionBuilder WithUser(Guid userId)
    {
        _question.UserId = userId;
        return this;
    }
}