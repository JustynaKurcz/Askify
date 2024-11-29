using Askify.Shared.Auth.Policies;
using static Askify.Shared.Auth.AvailableRole;

namespace Askify.Shared.Auth;

public static class Extensions
{
    public static IServiceCollection AddPolicy(this IServiceCollection services)
    {
        services.AddAuthorizationBuilder()
            .AddPolicy(AuthorizationPolicies.UserPolicy,
                policy => policy.RequireRole(nameof(User), nameof(Admin)))
            .AddPolicy(AuthorizationPolicies.AdminPolicy,
                policy => policy.RequireRole(nameof(Admin)));
    
        return services;
    }
}