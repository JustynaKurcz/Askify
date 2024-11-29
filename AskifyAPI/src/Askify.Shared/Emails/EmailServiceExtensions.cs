namespace Askify.Shared.Emails;

public static class EmailServiceExtensions
{
    public static IServiceCollection AddEmailService(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<EmailOptions>(
            configuration.GetSection(EmailOptions.SectionName));
            
        services.AddScoped<IEmailService, EmailService>();
        
        return services;
    }
}