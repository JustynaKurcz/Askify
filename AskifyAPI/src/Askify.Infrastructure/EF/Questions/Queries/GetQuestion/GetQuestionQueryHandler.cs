using Askify.Application.Questions.Queries.GetQuestion;
using Askify.Application.Questions.Queries.GetQuestion.DTO;
using Askify.Core.Questions.Exceptions;
using Askify.Core.Questions.Repositories;

namespace Askify.Infrastructure.EF.Questions.Queries.GetQuestion;

internal sealed class GetQuestionQueryHandler(IQuestionRepository questionRepository)
    : IRequestHandler<GetQuestionQuery, QuestionDetailsDto>
{
    public async Task<QuestionDetailsDto> Handle(GetQuestionQuery query,
        CancellationToken cancellationToken)
    {
        var question = await questionRepository.GetAsync(query.QuestionId, true, cancellationToken);

        if (question is null)
            throw new QuestionException.QuestionNotFoundException(query.QuestionId);

        return question.AsDetailsDto();
    }
}