using Askify.Application.Answers.Command.CreateAnswer;

namespace Askify.Api.Endpoints.Answers.Commands.CreateAnswer;

internal sealed class CreateAnswerEndpointRequest
{
    [FromRoute(Name = "questionId")] public Guid QuestionId { get; init; }
    [FromBody] public CreateAnswerCommand Command { get; init; } = default!;
}