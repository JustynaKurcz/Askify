using Askify.Core.Questions.Entities;
using Askify.Core.Questions.Exceptions;
using Askify.Core.Questions.Repositories;
using Askify.Shared.Auth.Context;
using MediatR;

namespace Askify.Application.Questions.Commands.ChangeQuestionInformation;

internal sealed class ChangeQuestionInformationCommandHandler(
    IQuestionRepository questionRepository,
    IContext context
) : IRequestHandler<ChangeQuestionInformationCommand>
{
    public async Task Handle(ChangeQuestionInformationCommand command,
        CancellationToken cancellationToken)
    {
        var question = await questionRepository.GetAsync(command.QuestionId, false, cancellationToken);

        if (question is null)
            throw new QuestionException.QuestionNotFoundException(command.QuestionId);

        var userId = context.Identity.Id;

        if (question.UserId != userId)
            throw new QuestionException.QuestionNotBelongToUser();

        if (!CanEdit(question))
            throw new QuestionException.QuestionCannotBeChanged();

        question.ChangeInformation(command.Title, command.Content, command.Tag);
        await questionRepository.SaveChangesAsync(cancellationToken);
    }

    private static bool CanEdit(Question question)
        => question.CreatedAt > DateTimeOffset.Now.AddMinutes(-30);
}