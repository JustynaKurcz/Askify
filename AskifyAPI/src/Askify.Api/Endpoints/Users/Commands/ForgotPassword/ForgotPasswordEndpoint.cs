using Askify.Application.Users.Commands.ForgotPassword;

namespace Askify.Api.Endpoints.Users.Commands.ForgotPassword;

public sealed class ForgotPasswordEndpoint : IEndpointDefinition
{
    public void DefineEndpoints(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost($"{UserEndpoints.BasePath}/forgot-password", async (
                [FromBody] ForgotPasswordCommand command,
                [FromServices] IMediator mediator,
                CancellationToken cancellationToken) =>
            {
                await mediator.Send(command, cancellationToken);
                return Results.Ok();
            })
            .WithOpenApi(operation => new OpenApiOperation(operation)
            {
                Summary = "Request password reset",
                Description = "Send password reset token to user's email",
            })
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest);
    }
}