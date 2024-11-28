namespace Askify.Application.Answers.Queries.GetAnswersForQuestion.DTO;

public record AnswerDto(
    Guid AnswerId,
    string Content,
    DateTimeOffset CreatedAt,
    Guid UserId
);