namespace Askify.Application.Questions.Queries.BrowseQuestions.DTO;

public record QuestionDto(
    Guid QuestionId,
    string Title,
    string Content,
    DateTimeOffset CreatedAt,
    Guid UserId,
    string Tag
);