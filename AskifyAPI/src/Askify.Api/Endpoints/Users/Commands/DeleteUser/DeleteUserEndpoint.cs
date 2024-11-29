using Askify.Application.Users.Commands.DeleteUser;

namespace Askify.Api.Endpoints.Users.Commands.DeleteUser;

internal sealed class DeleteUserEndpoint : IEndpointDefinition
{
    public void DefineEndpoints(IEndpointRouteBuilder app)
    {
        app.MapDelete($"{UserEndpoints.BasePath}", async (
                [FromServices] IMediator mediator,
                CancellationToken cancellationToken) =>
            {
                await mediator.Send(new DeleteUserCommand(), cancellationToken);
                return Results.NoContent();
            })
            .WithOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Delete user",
                Description = "This endpoint allows users to delete their account.",
            })
            .WithTags(UserEndpoints.Users)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status404NotFound)
            .RequireAuthorization(AuthorizationPolicies.UserPolicy);
    }
}