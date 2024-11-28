using MediatR;

namespace Askify.Application.Answers.Command.ChangeAnswerInformation;

public record ChangeAnswerInformationCommand(string Content) : IRequest
{
    internal Guid QuestionId { get; init; }
    internal Guid AnswerId { get; init; }
}