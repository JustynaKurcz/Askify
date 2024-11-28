using Askify.Application.Questions.Queries.GetQuestion.DTO;
using MediatR;

namespace Askify.Application.Questions.Queries.GetQuestion;

public record GetQuestionQuery(Guid QuestionId) : IRequest<QuestionDetailsDto>;