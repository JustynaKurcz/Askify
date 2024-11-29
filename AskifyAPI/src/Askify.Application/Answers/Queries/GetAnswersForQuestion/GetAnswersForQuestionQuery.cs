using Askify.Application.Answers.Queries.GetAnswersForQuestion.DTO;

namespace Askify.Application.Answers.Queries.GetAnswersForQuestion;

public record GetAnswersForQuestionQuery(Guid QuestionId) : IRequest<List<AnswerDto>>;