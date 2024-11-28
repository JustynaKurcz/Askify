using Askify.Application.Questions.Commands.DeleteQuestion;
using Askify.Shared.Auth.Policies;
using Askify.Shared.Endpoints;
using Askify.Shared.Results;
using MediatR;

namespace Askify.Api.Endpoints.Questions.Commands.DeleteQuestion;

internal sealed class DeleteQuestionEndpoint : IEndpointDefinition
{
    public void DefineEndpoints(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapDelete($"{QuestionEndpoints.BasePath}/{{questionId:guid}}", async (
                [FromRoute(Name = "questionId")] Guid questionId,
                [FromServices] IMediator mediator,
                CancellationToken cancellationToken
            ) =>
            {
                var command = new DeleteQuestionCommand(questionId);
                await mediator.Send(command, cancellationToken);

                return Results.NoContent();
            })
            .RequireAuthorization(AuthorizationPolicies.UserPolicy)
            .WithOpenApi(option => new OpenApiOperation(option)
            {
                Summary = "Delete a question",
                Description = "This endpoint allows you to delete a question."
            })
            .WithTags(QuestionEndpoints.Questions)
            .Produces(StatusCodes.Status204NoContent)
            .Produces<Error>(StatusCodes.Status400BadRequest);
    }
}