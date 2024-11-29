using Askify.Core.Questions.Enums;

namespace Askify.Application.Questions.Commands.CreateQuestion;

internal record CreateQuestionCommand(string Title, string Content, Tag Tag)
    : IRequest<CreateQuestionResponse>;