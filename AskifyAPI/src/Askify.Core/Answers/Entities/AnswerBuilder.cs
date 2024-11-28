namespace Askify.Core.Answers.Entities;

public class AnswerBuilder
{
    private readonly Answer _answer = Answer.Create();

    public Answer Build() => _answer;

    public AnswerBuilder WithContent(string content)
    {
        _answer.Content = content;
        return this;
    }

    public AnswerBuilder WithUser(Guid userId)
    {
        _answer.UserId = userId;
        return this;
    }

    public AnswerBuilder WithQuestion(Guid questionId)
    {
        _answer.QuestionId = questionId;
        return this;
    }
    
}