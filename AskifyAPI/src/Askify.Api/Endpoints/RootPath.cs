namespace Askify.Api.Endpoints;

internal static class RootPath
{
    private const string ApiVersion = "v1";
    internal static string BasePath => $"/api/{ApiVersion}";
}