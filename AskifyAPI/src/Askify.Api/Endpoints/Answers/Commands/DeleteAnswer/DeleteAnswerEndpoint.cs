using Askify.Api.Endpoints.Questions;
using Askify.Application.Answers.DeleteAnswer;
using Askify.Shared.Endpoints;
using MediatR;

namespace Askify.Api.Endpoints.Answers.Commands.DeleteAnswer;

internal sealed class DeleteAnswerEndpoint : IEndpointDefinition
{
    public void DefineEndpoints(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapDelete($"{QuestionEndpoints.BasePath}/{{questionId:guid}}/answers/{{answerId:guid}}", async (
                [FromRoute(Name = "questionId")] Guid questionId,
                [FromRoute(Name = "answerId")] Guid answerId,
                [FromServices] IMediator mediator,
                CancellationToken cancellationToken) =>
            {
                var command = new DeleteAnswerCommand(questionId, answerId);
                await mediator.Send(command, cancellationToken);

                return Results.NoContent();
            })
            .RequireAuthorization()
            .WithOpenApi(options => new OpenApiOperation(options)
            {
                Summary = "Delete answer",
                Description = "This endpoint allows users to delete an answer."
            })
            .WithTags(AnswerEndpoints.Answers)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest);
    }
}