using Askify.Application.Users.Queries.DTO;
using Askify.Application.Users.Queries.GetCurrentLoggedUser;
using Askify.Shared.Auth.Policies;
using Askify.Shared.Endpoints;
using MediatR;

namespace Askify.Api.Endpoints.Users.Queries.GetCurrentLoggedUser;

internal sealed class GetCurrentLoggedUserEndpoint : IEndpointDefinition
{
    public void DefineEndpoints(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(UserEndpoints.BasePath, async (
                [FromServices] IMediator mediator,
                CancellationToken cancellationToken
            ) =>
            {
                var result = await mediator.Send(new GetCurrentLoggedUserQuery(), cancellationToken);

                return Results.Ok(result);
            })
            .RequireAuthorization(AuthorizationPolicies.UserPolicy)
            .WithOpenApi(operation => new OpenApiOperation(operation)
            {
                Summary = "Get current logged user",
                Description = "This endpoint allows you to get the current logged user."
            })
            .WithTags(UserEndpoints.Users)
            .Produces<UserDetailsDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status401Unauthorized);
    }
}