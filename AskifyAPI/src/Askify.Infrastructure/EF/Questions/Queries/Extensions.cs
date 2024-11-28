using Askify.Application.Questions.Queries.BrowseQuestions.DTO;
using Askify.Application.Questions.Queries.GetQuestion.DTO;
using Askify.Core.Questions.Entities;

namespace Askify.Infrastructure.EF.Questions.Queries;

internal static class Extensions
{
    public static QuestionDetailsDto AsDetailsDto(this Question question)
        => new(
            QuestionId: question.Id,
            Title: question.Title,
            Content: question.Content,
            UserId: question.UserId,
            CreatedAt: question.CreatedAt
        );

    public static QuestionDto AsDto(this Question question)
        => new(
            QuestionId: question.Id,
            Title: question.Title,
            CreatedAt: question.CreatedAt,
            UserId: question.User.Id
        );
}