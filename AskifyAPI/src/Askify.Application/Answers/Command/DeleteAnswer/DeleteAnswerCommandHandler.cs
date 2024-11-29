using Askify.Core.Answers.Entities;
using Askify.Core.Answers.Exceptions;
using Askify.Core.Answers.Repositories;
using Askify.Core.Questions.Exceptions;
using Askify.Core.Questions.Repositories;
using Askify.Shared.Auth.Context;

namespace Askify.Application.Answers.Command.DeleteAnswer;

internal sealed class DeleteAnswerCommandHandler(
    IAnswerRepository answerRepository,
    IQuestionRepository questionRepository,
    IContext context
) : IRequestHandler<DeleteAnswerCommand>
{
    public async Task Handle(DeleteAnswerCommand command, CancellationToken cancellationToken)
    {
        var question = await questionRepository.GetAsync(command.QuestionId, false, cancellationToken)
            ?? throw new QuestionException.QuestionNotFoundException(command.QuestionId);

        var answer = await answerRepository.GetAsync(command.AnswerId, false, cancellationToken)
                     ?? throw new AnswerException.AnswerNotFoundException(command.AnswerId);

        var userId = context.Identity.Id;

        if (!IsAnswerOwnedByUser(answer, userId))
            throw new AnswerException.AnswerNotOwnedByUser(command.AnswerId);

        await answerRepository.DeleteAsync(answer);
        await answerRepository.SaveChangesAsync(cancellationToken);
    }

    private static bool IsAnswerOwnedByUser(Answer answer, Guid userId)
        => answer.UserId == userId;
}