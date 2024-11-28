using Askify.Core.Questions.Entities;
using Askify.Core.Questions.Exceptions;
using Askify.Core.Questions.Repositories;
using Askify.Shared.Auth.Context;
using MediatR;

namespace Askify.Application.Questions.Commands.DeleteQuestion;

internal sealed class DeleteQuestionCommandHandler(
    IQuestionRepository questionRepository,
    IContext context
) : IRequestHandler<DeleteQuestionCommand>
{
    public async Task Handle(DeleteQuestionCommand command,
        CancellationToken cancellationToken)
    {
        var question = await questionRepository
            .GetAsync(command.QuestionId, false, cancellationToken);

        if (question is null)
            throw new QuestionException.QuestionNotFoundException(command.QuestionId);

        var userId = context.Identity.Id;

        if (!IsQuestionOwnedByUser(question, userId))
            throw new QuestionException.QuestionNotOwnedByUser(command.QuestionId);

        await questionRepository.DeleteAsync(question);
        await questionRepository.SaveChangesAsync(cancellationToken);
    }

    private static bool IsQuestionOwnedByUser(Question question, Guid userId)
        => question.UserId == userId;
}