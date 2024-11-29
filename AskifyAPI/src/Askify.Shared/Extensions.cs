using Askify.Shared.Auth;
using Askify.Shared.Auth.Context;
using Askify.Shared.Auth.Middlewares;
using Askify.Shared.Emails;
using Askify.Shared.Exceptions;
using Askify.Shared.Hash;
using Microsoft.AspNetCore.Builder;

namespace Askify.Shared;

public static class Extensions
{
    public static IServiceCollection AddShared(this IServiceCollection services,
        IConfiguration configuration)
    {
        var jwtIssuer = configuration.GetSection("Authentication:Issuer").Get<string>();
        var jwtKey = configuration.GetSection("Authentication:JwtKey").Get<string>();

        services
            .AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    RequireExpirationTime = true,
                    ValidIssuer = jwtIssuer!,
                    ValidAudience = jwtIssuer!,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey!))
                };
            });


        services.AddScoped<IAuthManager, AuthManager>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddEmailService(configuration);
        services.AddHttpClient();
        services.AddSingleton<IContextFactory, ContextFactory>();
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddTransient(sp => sp.GetRequiredService<IContextFactory>().Create());
        services.AddScoped<ExceptionMiddleware>();

        return services;
    }

    public static WebApplication UseMiddlewares(this WebApplication app)
    {
        app.UseMiddleware<TokenExpirationMiddleware>();
        app.UseMiddleware<ExceptionMiddleware>();


        return app;
    }
}