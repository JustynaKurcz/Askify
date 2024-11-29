using Askify.Application.Questions.Queries.BrowseQuestions;
using Askify.Application.Questions.Queries.BrowseQuestions.DTO;
using Microsoft.AspNetCore.Html;

namespace Askify.Api.Endpoints.Questions.Queries.BrowseQuestions;

internal sealed class BrowseQuestionsEndpoint : IEndpointDefinition
{
    public void DefineEndpoints(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(QuestionEndpoints.BasePath, async (
                [AsParameters] BrowseQuestionsQuery query,
                [FromServices] IMediator mediator,
                CancellationToken cancellationToken) =>
            {
                var response = await mediator.Send(query, cancellationToken);
                return Results.Ok();
            })
            .WithOpenApi(operation => new OpenApiOperation(operation)
            {
                Summary = "Browse questions",
                Description = "This endpoint allows you to browse questions.",
            })
            .WithTags(QuestionEndpoints.Questions)
            .Produces<PagedResponse<QuestionDto>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .RequireAuthorization(AuthorizationPolicies.UserPolicy);
    }
}