using Askify.Core.Answers.Exceptions;
using Askify.Core.Answers.Repositories;
using Askify.Core.Questions.Exceptions;
using Askify.Core.Questions.Repositories;
using Askify.Shared.Auth.Context;

namespace Askify.Application.Answers.Command.ChangeAnswerInformation;

internal sealed class ChangeAnswerInformationCommandHandler(
    IAnswerRepository answerRepository,
    IQuestionRepository questionRepository,
    IContext context
    ) : IRequestHandler<ChangeAnswerInformationCommand>
{
    public async Task Handle(ChangeAnswerInformationCommand command, CancellationToken cancellationToken)
    {
        var question = await questionRepository.GetAsync(command.QuestionId, false, cancellationToken);

        if (question is null)
            throw new QuestionException.QuestionNotFoundException(command.QuestionId);
        
        var answer = await answerRepository.GetAsync(command.AnswerId, false, cancellationToken);
        
        if (answer is null)
            throw new AnswerException.AnswerNotFoundException(command.AnswerId);
        
        var userId = context.Identity.Id;
        
        if (answer.UserId != userId)
            throw new AnswerException.AnswerNotBelongToUser();
        
        answer.ChangeInformation(command.Content);
        await answerRepository.SaveChangesAsync(cancellationToken);
    }
}