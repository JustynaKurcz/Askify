using System.Reflection;
using Askify.Core.Questions.Repositories;
using Askify.Core.Users.Repositories;
using Askify.Infrastructure.EF.DbContext;
using Askify.Infrastructure.EF.Questions.Repositories;
using Askify.Infrastructure.EF.Users.Repositories;
using Askify.Infrastructure.Services;

[assembly: InternalsVisibleTo("Askify.Api")]

namespace Askify.Infrastructure;

internal static class Extensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        
        var connectionString = configuration["postgres:connectionString"];

        services.AddDbContext<AskifyDbContext>(options =>
            options.UseNpgsql(connectionString));

        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

        services.AddHostedService<AppInitializer>();
        
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IQuestionRepository, QuestionRepository>();

        return services;
    }
}