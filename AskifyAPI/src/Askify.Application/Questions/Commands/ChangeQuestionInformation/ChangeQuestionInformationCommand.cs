using Askify.Core.Questions.Enums;
using MediatR;

namespace Askify.Application.Questions.Commands.ChangeQuestionInformation;

internal record ChangeQuestionInformationCommand(string Title, string Content, Tag Tag) : IRequest
{
    internal Guid QuestionId { get; init; }
}