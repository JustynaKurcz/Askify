using Askify.Application.Questions.Queries.BrowseQuestions.DTO;
using Askify.Application.Questions.Queries.GetQuestion;

namespace Askify.Api.Endpoints.Questions.Queries.GetQuestion;

internal sealed class GetQuestionEndpoint : IEndpointDefinition
{
    public void DefineEndpoints(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet($"{QuestionEndpoints.BasePath}/{{questionId:guid}}", async (
                [FromServices] IMediator mediator,
                [FromRoute] Guid questionId) =>
            {
                var query = new GetQuestionQuery(questionId);
                var result = await mediator.Send(query);

                return Results.Ok(result);
            })
            .WithOpenApi(operation => new OpenApiOperation(operation)
            {
                Summary = "Get question",
                Description = "This endpoint allows users to get a question by its identifier."
            })
            .WithTags(QuestionEndpoints.Questions)
            .Produces<QuestionDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);
    }
}