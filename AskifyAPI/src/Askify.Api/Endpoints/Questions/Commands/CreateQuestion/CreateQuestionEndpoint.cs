using Askify.Application.Questions.Commands.CreateQuestion;

namespace Askify.Api.Endpoints.Questions.Commands.CreateQuestion;

internal sealed class CreateQuestionEndpoint : IEndpointDefinition
{
    public void DefineEndpoints(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost(QuestionEndpoints.BasePath, async (
                [FromBody] CreateQuestionCommand command,
                [FromServices] IMediator mediator,
                CancellationToken cancellationToken
            ) =>
            {
                var result = await mediator.Send(command, cancellationToken);

                return Results.Created(QuestionEndpoints.BasePath, result);
            })
            .WithOpenApi(options => new OpenApiOperation(options)
            {
                Summary = "Create question",
                Description = "This endpoint allows users to create a question."
            })
            .WithTags(QuestionEndpoints.Questions)
            .Produces<CreateQuestionResponse>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .RequireAuthorization(AuthorizationPolicies.UserPolicy);
    }
}