using Askify.Core.Users.Repositories;
using Askify.Shared.Auth;

namespace Askify.Api.Endpoints.Users.Commands.ResetPassword;

public sealed class ResetPasswordEndpoint : IEndpointDefinition
{
    public void DefineEndpoints(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost($"{UserEndpoints.BasePath}/reset-password/{{token}}", async (
                [AsParameters] ResetPasswordEndpointRequest request,
                [FromServices] IMediator mediator,
                CancellationToken cancellationToken) =>
            {
                await mediator.Send(request.Command, cancellationToken);

                return Results.Ok();
            })
            .WithOpenApi(operation => new OpenApiOperation(operation)
            {
                Summary = "Reset password",
                Description = "Reset user password using token from email",
            })
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest);
    }
}