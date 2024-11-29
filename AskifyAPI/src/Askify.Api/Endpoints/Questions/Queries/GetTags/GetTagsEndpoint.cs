using Askify.Core.Questions.Enums;
using Askify.Shared;
using Humanizer;

namespace Askify.Api.Endpoints.Questions.Queries.GetTags;

internal sealed class GetTagsEndpoint : IEndpointDefinition
{
    public void DefineEndpoints(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet($"{QuestionEndpoints.BasePath}/tags", async () =>
            {
                var tagValues = Enum.GetValues(typeof(Tag))
                    .Cast<Tag>()
                    .Select(tag => new TagResponse(
                        Id: (int)tag,
                        Name: tag.ToString(),
                        DisplayName: tag.Humanize()
                    ))
                    .ToList();

                return Results.Ok(tagValues);
            })
            .WithOpenApi(operation => new OpenApiOperation(operation)
            {
                Summary = "Get all tags",
                Description = "This endpoint returns all available tags with their display names.",
            })
            .WithTags(QuestionEndpoints.Questions)
            .Produces<List<TagResponse>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized);
    }
}