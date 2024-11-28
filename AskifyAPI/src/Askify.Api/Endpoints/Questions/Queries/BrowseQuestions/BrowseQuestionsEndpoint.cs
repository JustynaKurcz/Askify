using Askify.Application.Questions.Queries.BrowseQuestions;
using Askify.Application.Questions.Queries.BrowseQuestions.DTO;
using Askify.Shared.Endpoints;
using MediatR;

namespace Askify.Api.Endpoints.Questions.Queries.BrowseQuestions;

internal sealed class BrowseQuestionsEndpoint : IEndpointDefinition
{
    public void DefineEndpoints(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet($"{QuestionEndpoints.BasePath}/browse", async (
                [FromServices] IMediator mediator,
                CancellationToken cancellationToken) =>
            {
                var query = new BrowseQuestionsQuery();
                return await mediator.Send(query, cancellationToken);
            })
            .WithOpenApi(operation => new OpenApiOperation(operation)
            {
                Summary = "Browse questions",
                Description = "This endpoint allows you to browse questions.",
            })
            .WithTags(QuestionEndpoints.Questions)
            .Produces<List<QuestionDto>>(StatusCodes.Status200OK);
    }
}