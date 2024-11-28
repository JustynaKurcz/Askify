using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

[assembly: InternalsVisibleTo("Askify.Api")]

namespace Askify.Core;

internal static class Extensions
{
    internal static IServiceCollection AddCore(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));        
        return services;
    }
}