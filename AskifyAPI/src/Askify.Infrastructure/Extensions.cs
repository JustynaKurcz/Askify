using Askify.Infrastructure.EF.DbContext;

[assembly: InternalsVisibleTo("Askify.Api")]

namespace Askify.Infrastructure;

internal static class Extensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration["postgres:connectionString"];

        services.AddDbContext<AskifyDbContext>(options =>
            options.UseNpgsql(connectionString));

        return services;
    }
}