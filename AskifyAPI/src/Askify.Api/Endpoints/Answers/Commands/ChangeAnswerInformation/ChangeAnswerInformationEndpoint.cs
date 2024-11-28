using Askify.Api.Endpoints.Questions;
using Askify.Shared.Endpoints;
using MediatR;

namespace Askify.Api.Endpoints.Answers.Commands.ChangeAnswerInformation;

internal sealed class ChangeAnswerInformationEndpoint : IEndpointDefinition
{
    public void DefineEndpoints(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPut($"{QuestionEndpoints.BasePath}/{{questionId}}/answers/{{answerId}}", async (
                [AsParameters] ChangeAnswerInformationEndpointRequest request,
                [FromServices] IMediator mediator,
                CancellationToken cancellationToken) =>
            {
                var command = request.Command with
                {
                    QuestionId = request.QuestionId,
                    AnswerId = request.AnswerId
                };
                await mediator.Send(command, cancellationToken);
                return Results.NoContent();
            })
            .WithOpenApi(operation => new OpenApiOperation(operation)
            {
                Summary = "Change answer information",
                Description = "This endpoint allows you to change answer information.",
            })
            .WithTags(AnswerEndpoints.Answers)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .RequireAuthorization();
    }
}