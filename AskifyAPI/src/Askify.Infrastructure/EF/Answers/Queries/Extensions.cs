using Askify.Application.Answers.Queries.GetAnswersForQuestion.DTO;
using Askify.Core.Answers.Entities;

namespace Askify.Infrastructure.EF.Answers.Queries;

internal static class Extensions
{
    public static AnswerDto AsDto(this Answer answer)
        => new(
            AnswerId: answer.Id,
            Content: answer.Content,
            CreatedAt: answer.CreatedAt,
            UserId: answer.UserId   
        );
}