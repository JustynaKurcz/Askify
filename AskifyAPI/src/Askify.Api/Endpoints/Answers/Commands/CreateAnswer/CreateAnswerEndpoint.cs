using Askify.Api.Endpoints.Questions;
using Askify.Application.Answers.Command.CreateAnswer;

namespace Askify.Api.Endpoints.Answers.Commands.CreateAnswer;

internal sealed class CreateAnswerEndpoint : IEndpointDefinition
{
    public void DefineEndpoints(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost($"{QuestionEndpoints.BasePath}/{{questionId:guid}}/answers", async (
                [AsParameters] CreateAnswerEndpointRequest request,
                [FromServices] IMediator mediator,
                CancellationToken cancellationToken) =>
            {
                var command = request.Command with { QuestionId = request.QuestionId };
                var response = await mediator.Send(command, cancellationToken);

                return Results.Created(QuestionEndpoints.BasePath, response);
            })
            .WithOpenApi(options => new OpenApiOperation(options)
            {
                Summary = "Create answer",
                Description = "This endpoint allows users to create an answer."
            })
            .WithTags(AnswerEndpoints.Answers)
            .Produces<CreateAnswerResponse>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .RequireAuthorization(AuthorizationPolicies.UserPolicy);
    }
}