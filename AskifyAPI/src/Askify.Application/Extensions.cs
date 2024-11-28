using Askify.Shared.Validation;

[assembly: InternalsVisibleTo("Askify.Api")]

namespace Askify.Application;

internal static class Extensions
{
    internal static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        services
            .AddValidators(Assembly.GetExecutingAssembly());

        return services;
    }
}