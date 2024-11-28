namespace Askify.Application.Questions.Queries.BrowseQuestions.DTO;

public record QuestionDto(Guid QuestionId, string Title, DateTimeOffset CreatedAt);