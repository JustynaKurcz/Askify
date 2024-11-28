using Askify.Core.Answers.Entities;
using Askify.Core.Answers.Repositories;
using Askify.Core.Questions.Exceptions;
using Askify.Core.Questions.Repositories;
using Askify.Shared.Auth.Context;
using MediatR;

namespace Askify.Application.Answers.CreateAnswer;

internal sealed class CreateAnswerCommandHandler(
    IQuestionRepository questionRepository,
    IAnswerRepository answerRepository,
    IContext context)
    : IRequestHandler<CreateAnswerCommand, CreateAnswerResponse>
{
    public async Task<CreateAnswerResponse> Handle(CreateAnswerCommand command,
        CancellationToken cancellationToken)
    {
        var userId = context.Identity.Id;

        var questionExists = await questionRepository
            .AnyAsync(x => x.Id == command.QuestionId, cancellationToken);
        
        if (!questionExists)
            throw new QuestionException.QuestionNotFoundException(command.QuestionId);

        var answer = new AnswerBuilder()
            .WithContent(command.Content)
            .WithQuestion(command.QuestionId)
            .WithUser(userId)
            .Build();

        await answerRepository.AddAsync(answer, cancellationToken);
        await answerRepository.SaveChangesAsync(cancellationToken);

        return new CreateAnswerResponse(answer.Id);
    }
}