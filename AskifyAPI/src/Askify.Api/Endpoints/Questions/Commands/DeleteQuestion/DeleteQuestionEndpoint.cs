using Askify.Application.Questions.Commands.DeleteQuestion;

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
            .WithOpenApi(option => new OpenApiOperation(option)
            {
                Summary = "Delete a question",
                Description = "This endpoint allows you to delete a question."
            })
            .WithTags(QuestionEndpoints.Questions)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .RequireAuthorization(AuthorizationPolicies.UserPolicy);
    }
}