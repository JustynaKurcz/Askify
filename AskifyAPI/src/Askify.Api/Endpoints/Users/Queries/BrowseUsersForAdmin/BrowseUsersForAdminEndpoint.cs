using Askify.Application.Users.Queries.BrowseUsers;
using Askify.Application.Users.Queries.GetCurrentLoggedUser.DTO;

namespace Askify.Api.Endpoints.Users.Queries.BrowseUsersForAdmin;

internal sealed class BrowseUsersForAdminEndpoint : IEndpointDefinition
{
    public void DefineEndpoints(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(UserEndpoints.BasePath, async (
                [AsParameters] BrowseUsersQuery query,
                [FromServices] IMediator mediator,
                CancellationToken cancellationToken) =>
            {
                var response = await mediator.Send(query, cancellationToken);
                return Results.Ok(response);
            })
            .WithOpenApi(operation => new OpenApiOperation(operation)
            {
                Summary = "Browse users",
                Description = "This endpoint allows you to browse users.",
            })
            .WithTags(UserEndpoints.Users)
            .Produces<PagedResponse<UserDetailsDto>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .RequireAuthorization(AuthorizationPolicies.AdminPolicy);
    }
}