using Askify.Application.Questions.Queries.BrowseQuestions;
using Askify.Application.Questions.Queries.BrowseQuestions.DTO;
using Askify.Core.Questions.Repositories;
using MediatR;

namespace Askify.Infrastructure.EF.Questions.Queries.BrowseQuestions;

internal sealed class BrowseQuestionsQueryHandler(
    IQuestionRepository questionRepository
) : IRequestHandler<BrowseQuestionsQuery, List<QuestionDto>>
{
    public async Task<List<QuestionDto>> Handle(BrowseQuestionsQuery query,
        CancellationToken cancellationToken)
    {
        var questions = await questionRepository.GetAll(cancellationToken);

        return questions.Select(x => x.AsDto()).ToList();
    }
}