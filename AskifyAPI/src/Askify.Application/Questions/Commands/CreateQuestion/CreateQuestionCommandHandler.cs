using Askify.Core.Questions.Entities;
using Askify.Core.Questions.Errors;
using Askify.Core.Questions.Repositories;
using Askify.Core.Users.Errors;
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
        {
            return QuestionError.QuestionExistsForUser;
        }

        var userExists = await userRepository
            .AnyAsync(x => x.Id == userId, cancellationToken);

        if (!userExists)
        {
            return UserError.UserNotFound(userId);
        }

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