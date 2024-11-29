using Askify.Application.Users.Queries.GetUserName;

namespace Askify.Api.Endpoints.Users.Queries.GetUserName;

internal sealed class GetUserNameEndpoint : IEndpointDefinition
{
    public void DefineEndpoints(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet($"{UserEndpoints.BasePath}/{{userId:guid}}/name", async (
                [FromRoute] Guid userId,
                [FromServices] IMediator mediator,
                CancellationToken cancellationToken) =>
            {
                var query = new GetUserNameQuery(userId);
                var response = await mediator.Send(query, cancellationToken);

                return Results.Ok(response);
            })
            .WithOpenApi(operation => new OpenApiOperation(operation)
            {
                Summary = "Get user name",
                Description = "This endpoint allows get user name by user id.",
            })
            .WithTags(UserEndpoints.Users)
            .Produces<string>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);
    }
}