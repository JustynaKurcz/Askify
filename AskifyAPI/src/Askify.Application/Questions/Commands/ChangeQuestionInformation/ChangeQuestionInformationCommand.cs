using Askify.Core.Questions.Enums;

namespace Askify.Application.Questions.Commands.ChangeQuestionInformation;

internal record ChangeQuestionInformationCommand(string Title, string Content, Tag Tag) : IRequest
{
    internal Guid QuestionId { get; init; }
}