using Askify.Core.Questions.Entities;
using Askify.Core.Questions.Exceptions;
using Askify.Core.Questions.Repositories;
using Askify.Core.Users.Exceptions;
using Askify.Core.Users.Repositories;
using Askify.Shared.Auth.Context;
using Askify.Shared.Results;
using MediatR;

namespace Askify.Application.Questions.Commands.CreateQuestion;

internal sealed class CreateQuestionCommandHandler(
    IQuestionRepository questionRepository,
    IUserRepository userRepository,
    IContext context
) : IRequestHandler<CreateQuestionCommand, Result<CreateQuestionResponse, Error>>
{
    public async Task<Result<CreateQuestionResponse, Error>> Handle(CreateQuestionCommand command,
        CancellationToken cancellationToken)
    {
        var userId = context.Identity.Id;

        var existingQuestionForUser = await questionRepository
            .AnyAsync(
                x => x.UserId == userId && x.Title == command.Title,
                cancellationToken
            );

        if (existingQuestionForUser)
            throw new QuestionException.QuestionExistsForUser();

        var userExists = await userRepository
            .AnyAsync(x => x.Id == userId, cancellationToken);

        if (!userExists)
            throw new UserException.UserNotFoundException(userId);

        var question = new QuestionBuilder()
            .WithTitle(command.Title)
            .WithContent(command.Content)
            .WithUser(userId)
            .Build();

        await questionRepository.AddAsync(question, cancellationToken);
        await questionRepository.SaveChangesAsync(cancellationToken);

        return new CreateQuestionResponse(question.Id);
    }
}