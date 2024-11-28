using Askify.Shared.Results;
using MediatR;

namespace Askify.Application.Questions.Commands.CreateQuestion;

internal record CreateQuestionCommand(string Title, string Content)
    : IRequest<Result<CreateQuestionResponse, Error>>;