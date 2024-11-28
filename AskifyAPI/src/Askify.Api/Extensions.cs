using Askify.Infrastructure;

namespace Askify.Api;

internal static class Extensions
{
    internal static IServiceCollection LoadLayers(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddInfrastructure(configuration);

        return services;
    }
}