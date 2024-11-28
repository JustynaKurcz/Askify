using Askify.Application.Users.Commands.SignIn;
using Askify.Shared.Endpoints;
using MediatR;

namespace Askify.Api.Endpoints.Users.Commands.SignIn;

internal sealed class SignInEndpoint : IEndpointDefinition
{
    public void DefineEndpoints(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost(UserEndpoints.SignIn, async (
                [FromBody] SignInCommand command,
                [FromServices] IMediator mediator,
                CancellationToken cancellationToken
            ) =>
            {
                var result = await mediator.Send(command, cancellationToken);
                return Results.Ok(result);
            })
            .WithOpenApi(operation => new OpenApiOperation(operation)
            {
                Summary = "Sign in",
                Description = "This endpoint allows users to sign in."
            })
            .WithTags(UserEndpoints.Users)
            .Produces<SignInResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest);
    }
}