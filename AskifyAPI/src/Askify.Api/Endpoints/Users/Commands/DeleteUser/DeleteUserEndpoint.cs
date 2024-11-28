using Askify.Application.Users.Commands.DeleteUser;
using Askify.Shared.Endpoints;
using MediatR;

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
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .WithTags(UserEndpoints.Users)
            .WithOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Delete user",
                Description = "This endpoint allows users to delete their account.",
            })
            .RequireAuthorization();
    }
}