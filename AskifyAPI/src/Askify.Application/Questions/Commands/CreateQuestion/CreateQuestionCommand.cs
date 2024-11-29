using Askify.Core.Questions.Enums;
using Askify.Shared.Results;
using MediatR;

namespace Askify.Application.Questions.Commands.CreateQuestion;

internal record CreateQuestionCommand(string Title, string Content, Tag Tag)
    : IRequest<Result<CreateQuestionResponse, Error>>;