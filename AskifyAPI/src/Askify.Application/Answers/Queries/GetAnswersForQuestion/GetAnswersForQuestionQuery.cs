using Askify.Application.Answers.Queries.GetAnswersForQuestion.DTO;
using MediatR;

namespace Askify.Application.Answers.Queries.GetAnswersForQuestion;

public record GetAnswersForQuestionQuery(Guid QuestionId) : IRequest<List<AnswerDto>>;