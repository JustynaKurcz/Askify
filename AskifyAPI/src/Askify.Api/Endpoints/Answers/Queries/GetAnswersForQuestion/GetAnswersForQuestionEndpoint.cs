using Askify.Api.Endpoints.Questions;
using Askify.Application.Answers.Queries.GetAnswersForQuestion;
using Askify.Application.Answers.Queries.GetAnswersForQuestion.DTO;

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
                Summary = "Get answers for question",
                Description = "This endpoint allows get answers for question.",
            })
            .WithTags(AnswerEndpoints.Answers)
            .Produces<List<AnswerDto>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized)
            .RequireAuthorization(AuthorizationPolicies.UserPolicy);
    }
}