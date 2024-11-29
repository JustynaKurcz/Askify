using Askify.Application.Answers.Queries.GetAnswersForQuestion;
using Askify.Application.Answers.Queries.GetAnswersForQuestion.DTO;
using Askify.Core.Answers.Repositories;

namespace Askify.Infrastructure.EF.Answers.Queries.GetAnswersForQuestion;

internal sealed class GetAnswersForQuestionQueryHandler(
    IAnswerRepository answerRepository
) : IRequestHandler<GetAnswersForQuestionQuery, List<AnswerDto>>
{
    public async Task<List<AnswerDto>> Handle(GetAnswersForQuestionQuery query, CancellationToken cancellationToken)
    {
        var answers = await answerRepository.GetAnswerForQuestion(query.QuestionId, cancellationToken);

        return answers
            .Select(x => x.AsDto())
            .ToList();
    }
}