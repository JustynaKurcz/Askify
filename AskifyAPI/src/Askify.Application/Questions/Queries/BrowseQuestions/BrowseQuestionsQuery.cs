using Askify.Application.Questions.Queries.BrowseQuestions.DTO;

namespace Askify.Application.Questions.Queries.BrowseQuestions;

public sealed class BrowseQuestionsQuery : PagedQuery, IRequest<PagedResponse<QuestionDto>>
{
    [SwaggerParameter("Search filter - matches against question's title")]
    public string? Search { get; set; }
}