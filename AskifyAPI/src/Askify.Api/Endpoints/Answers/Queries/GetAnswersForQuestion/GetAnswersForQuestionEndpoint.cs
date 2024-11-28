using Askify.Api.Endpoints.Questions;
using Askify.Application.Answers.Queries.GetAnswersForQuestion;
using Askify.Application.Questions.Queries.BrowseQuestions.DTO;
using Askify.Shared.Endpoints;
using MediatR;

namespace Askify.Api.Endpoints.Answers.Queries.GetAnswersForQuestion;

internal sealed class GetAnswersForQuestionEndpoint : IEndpointDefinition
{
    public void DefineEndpoints(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet($"{QuestionEndpoints.BasePath}/{{questionId:guid}}/answers", async (
                [FromRoute] Guid questionId,
                [FromServices] IMediator mediator,
                CancellationToken cancellationToken) =>
            {
                var query = new GetAnswersForQuestionQuery(questionId);
                var response = await mediator.Send(query, cancellationToken);

                return Results.Ok(response);
            })
            .WithOpenApi(operation => new OpenApiOperation(operation)
            {
                Summary = "Browse questions",
                Description = "This endpoint allows you to browse questions.",
            })
            .WithTags(QuestionEndpoints.Questions)
            .Produces<List<QuestionDto>>(StatusCodes.Status200OK)
            .RequireAuthorization();
    }
}