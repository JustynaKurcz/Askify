using Askify.Application.Questions.Commands.CreateQuestion;
using Askify.Shared.Auth.Policies;
using Askify.Shared.Endpoints;
using Askify.Shared.Results;
using MediatR;

namespace Askify.Api.Endpoints.Questions.Commands.CreateQuestion;

internal sealed class CreateQuestionEndpoint : IEndpointDefinition
{
    public void DefineEndpoints(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost(QuestionEndpoints.BasePath, async (
                [FromBody] CreateQuestionCommand command,
                [FromServices] IMediator mediator,
                CancellationToken cancellationToken
            ) =>
            {
                var result = await mediator.Send(command, cancellationToken);

                return result.Match(
                    success => Results.Created(string.Empty, success),
                    Results.BadRequest);
            })
            .RequireAuthorization(AuthorizationPolicies.UserPolicy)
            .WithOpenApi(options => new OpenApiOperation(options)
            {
                Summary = "Create question",
                Description = "This endpoint allows users to create a question."
            })
            .WithTags(QuestionEndpoints.Questions)
            .Produces<CreateQuestionResponse>(StatusCodes.Status201Created)
            .Produces<Error>(StatusCodes.Status400BadRequest);
    }
}