using Askify.Application.Answers.DeleteAnswer;

namespace Askify.Api.Endpoints.Answers.Commands.DeleteAnswer;

internal sealed class DeleteAnswerEndpointRequest
{
    [FromRoute(Name = "questionId")] public Guid QuestionId { get; init; }
    [FromBody] public DeleteAnswerCommand Command { get; init; } = default!;
}