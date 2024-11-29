
using Askify.Application.Users.Queries.GetResetPasswordForm;

namespace Askify.Api.Endpoints.Users.Queries.GetResetPasswordForm;

internal sealed class GetResetPasswordFormEndpoint : IEndpointDefinition
{
    public void DefineEndpoints(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet($"{UserEndpoints.BasePath}/reset-password/{{token}}", async (
                [FromRoute] string token,
                [FromServices] IMediator mediator,
                CancellationToken cancellationToken) =>
            {
                var query = new GetResetPasswordFormQuery(token);
                var formHtml = await mediator.Send(query, cancellationToken);
                return Results.Content(formHtml, "text/html");
            })
            .WithTags(UserEndpoints.Users);
    }
}