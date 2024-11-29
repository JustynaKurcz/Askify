using Askify.Application.Questions.Queries.BrowseQuestions;
using Askify.Application.Questions.Queries.BrowseQuestions.DTO;
using Askify.Core.Questions.Entities;
using Askify.Core.Questions.Repositories;

namespace Askify.Infrastructure.EF.Questions.Queries.BrowseQuestions;

internal sealed class BrowseQuestionsQueryHandler(
    IQuestionRepository questionRepository
) : IRequestHandler<BrowseQuestionsQuery, PagedResponse<QuestionDto>>
{
    public async Task<PagedResponse<QuestionDto>> Handle(BrowseQuestionsQuery query,
        CancellationToken cancellationToken)
    {
        var questions = await questionRepository.GetAll(cancellationToken);

        questions = Search(query, questions);
        
        return await questions
            .OrderByDescending(x => x.CreatedAt)
            .Select(x => x.AsDto())
            .PaginateAsync(query.GetPageNumber(), query.GetPageSize(), cancellationToken);
    }

    private IQueryable<Question> Search(BrowseQuestionsQuery query, IQueryable<Question> questions)
    {
        if (string.IsNullOrWhiteSpace(query.Search)) return questions;
        var searchTxt = $"%{query.Search}%";
        return questions.Where(question =>
            Microsoft.EntityFrameworkCore.EF.Functions.ILike(question.Title, searchTxt));
    }
}