using Askify.Application.Users.Commands.SignUp;

namespace Askify.Api.Endpoints.Users.Commands.SignUp;

internal sealed class SignUpEndpoint : IEndpointDefinition
{
    public void DefineEndpoints(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost(UserEndpoints.SignUp, async (
                [FromBody] SignUpCommand command,
                [FromServices] IMediator mediator,
                CancellationToken cancellationToken
            ) =>
            {
                var result = await mediator.Send(command, cancellationToken);
                return Results.Created(UserEndpoints.BasePath, result);
            })
            .WithOpenApi(option => new OpenApiOperation(option)
            {
                Summary = "Sign up",
                Description = "This endpoint allows users to sign up."
            })
            .WithTags(UserEndpoints.Users)
            .Produces<SignUpResponse>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest);
    }
}