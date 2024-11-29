
using Askify.Application.Users.Commands.Admin.DeleteUserCommand;

namespace Askify.Api.Endpoints.Users.Commands.Admin.DeleteUser;

internal sealed class DeleteUserEndpoint : IEndpointDefinition
{
    public void DefineEndpoints(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapDelete($"{UserEndpoints.BasePath}/{{userId}}", async (
                [FromRoute] Guid userId,
                [FromServices] IMediator mediator,
                CancellationToken cancellationToken) =>
            {
                await mediator.Send(new DeleteUserCommand(userId), cancellationToken);
                return Results.NoContent();
            })
            .WithOpenApi(operation => new OpenApiOperation(operation)
            {
                Summary = "Delete user for admin",
                Description = "This endpoint allows admins to delete a user.",
            })
            .WithTags(UserEndpoints.Users)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .Produces(StatusCodes.Status404NotFound)
            .RequireAuthorization(AuthorizationPolicies.AdminPolicy);
    }
}