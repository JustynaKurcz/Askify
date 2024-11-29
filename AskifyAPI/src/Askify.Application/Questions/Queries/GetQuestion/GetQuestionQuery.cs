using Askify.Application.Questions.Queries.GetQuestion.DTO;

namespace Askify.Application.Questions.Queries.GetQuestion;

public record GetQuestionQuery(Guid QuestionId) : IRequest<QuestionDetailsDto>;