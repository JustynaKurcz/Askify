[assembly: InternalsVisibleTo("Askify.Api")]

namespace Askify.Shared;

internal static class SwaggerOptions
{
    internal static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(swagger =>
        {
            swagger.CustomSchemaIds(x => x.FullName?.Replace("+", "."));
            swagger.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Askify API",
                Version = "v1"
            });

            swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please insert JWT with Bearer into field",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "Bearer"
            });

            swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
            
            swagger.EnableAnnotations();
        });


        return services;
    }
}