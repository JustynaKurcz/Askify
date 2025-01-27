using Askify.Core.Questions.Entities;
using Askify.Core.Questions.Exceptions;
using Askify.Core.Questions.Repositories;
using Askify.Core.Users.Enums;
using Askify.Shared.Auth.Context;
using Microsoft.OpenApi.Extensions;

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
                           .GetAsync(command.QuestionId, false, cancellationToken)
                       ?? throw new QuestionException.QuestionNotFoundException(command.QuestionId);

        var userId = context.Identity.Id;

        if (!CanDelete(question, userId))
            throw new QuestionException.CannotDeleteQuestionException();

        await questionRepository.DeleteAsync(question);
        await questionRepository.SaveChangesAsync(cancellationToken);
    }

    private bool CanDelete(Question question, Guid userId)
        => question.UserId == userId || IsAdmin();

    private bool IsAdmin()
        => context.Identity.Role == Role.Admin.GetDisplayName();
}