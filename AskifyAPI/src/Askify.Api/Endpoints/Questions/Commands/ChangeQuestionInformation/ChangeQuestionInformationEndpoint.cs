using Askify.Application.Questions.Commands.ChangeQuestionInformation;
using Askify.Shared.Auth.Policies;
using Askify.Shared.Endpoints;
using Askify.Shared.Results;
using MediatR;

namespace Askify.Api.Endpoints.Questions.Commands.ChangeQuestionInformation;

internal sealed class ChangeQuestionInformationEndpoint : IEndpointDefinition
{
    public void DefineEndpoints(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPut($"{QuestionEndpoints.BasePath}/{{questionId:guid}}", async (
                [FromRoute(Name = "questionId")] Guid questionId,
                [FromBody] ChangeQuestionInformationCommand command,
                [FromServices] IMediator mediator,
                CancellationToken cancellationToken
            ) =>
            {
                command = command with { QuestionId = questionId };
                await mediator.Send(command, cancellationToken);

                return Results.NoContent();
            })
            .RequireAuthorization(AuthorizationPolicies.UserPolicy)
            .WithOpenApi(option => new OpenApiOperation(option)
            {
                Summary = "Change a question",
                Description = "This endpoint allows you to change a question."
            })
            .WithTags(QuestionEndpoints.Questions)
            .Produces(StatusCodes.Status204NoContent)
            .Produces<Error>(StatusCodes.Status400BadRequest);
    }
}