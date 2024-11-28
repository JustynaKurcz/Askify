using Askify.Application.Questions.Queries.BrowseQuestions.DTO;
using MediatR;

namespace Askify.Application.Questions.Queries.BrowseQuestions;

public record BrowseQuestionsQuery() : IRequest<List<QuestionDto>>;