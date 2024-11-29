namespace Askify.Application.Questions.Queries.GetQuestion.DTO;

public record QuestionDetailsDto(
    Guid QuestionId,
    string Title,
    string Content,
    Guid UserId,
    DateTimeOffset CreatedAt,
    string Tag
);