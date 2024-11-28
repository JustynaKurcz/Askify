using Askify.Core.Questions.Enums;
using Askify.Shared.Endpoints;
using Microsoft.OpenApi.Attributes;

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
                        (int)tag,
                        tag.ToString(),
                        tag.GetType()
                            .GetMember(tag.ToString())
                            .First()
                            .GetCustomAttribute<DisplayAttribute>()
                            ?.Name ?? tag.ToString()
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
            .RequireAuthorization();
    }
}