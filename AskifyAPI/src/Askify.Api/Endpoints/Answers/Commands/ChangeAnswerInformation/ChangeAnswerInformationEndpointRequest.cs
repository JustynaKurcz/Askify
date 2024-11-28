using Askify.Application.Answers.Command.ChangeAnswerInformation;

namespace Askify.Api.Endpoints.Answers.Commands.ChangeAnswerInformation;

internal sealed class ChangeAnswerInformationEndpointRequest
{
    [FromRoute(Name = "questionId")] public Guid QuestionId { get; init; }
    [FromRoute(Name = "answerId")] public Guid AnswerId { get; init; }
    [FromBody] public ChangeAnswerInformationCommand Command { get; init; } = default!;
}