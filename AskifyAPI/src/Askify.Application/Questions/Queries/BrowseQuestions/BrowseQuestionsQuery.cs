using Askify.Application.Questions.Queries.BrowseQuestions.DTO;
using Askify.Shared.Pagination;
using MediatR;

namespace Askify.Application.Questions.Queries.BrowseQuestions;

public sealed class BrowseQuestionsQuery : PagedQuery, IRequest<PagedResponse<QuestionDto>>
{
    public string? Search { get; set; }
}