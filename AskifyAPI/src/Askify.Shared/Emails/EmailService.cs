using System.Net.Mail;

namespace Askify.Shared.Emails;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;
    private readonly string _emailTemplate;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
        _emailTemplate = LoadEmailTemplate();
    }

    private static string LoadEmailTemplate()
    {
        var assembly = Assembly.GetExecutingAssembly();
        var resourcePath = assembly.GetManifestResourceNames()
            .FirstOrDefault(x => x.EndsWith("PasswordReset.html")) 
            ?? throw new InvalidOperationException("Could not find email template.");

        using var stream = assembly.GetManifestResourceStream(resourcePath);
        if (stream == null) throw new InvalidOperationException("Could not load email template.");
        
        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }

    public async Task SendPasswordResetEmailAsync(string email, string token, CancellationToken cancellationToken)
    {
        var smtpServer = _configuration["Email:SmtpHost"];
        var smtpPort = int.Parse(_configuration["Email:SmtpPort"]);
        var smtpUsername = _configuration["Email:Username"];
        var smtpPassword = _configuration["Email:Password"];
        var fromEmail = smtpUsername;

        var baseUrl = _configuration["BaseUrl"] ?? "http://localhost:5244";
        var resetLink = $"{baseUrl}/api/v1/users/reset-password/{Uri.EscapeDataString(token)}";

        var emailBody = _emailTemplate.Replace("{{resetLink}}", resetLink);

        var message = new MailMessage
        {
            From = new MailAddress(fromEmail),
            Subject = "Reset hasła",
            Body = emailBody,
            IsBodyHtml = true
        };
        message.To.Add(email);

        using var client = new SmtpClient(smtpServer, smtpPort)
        {
            Credentials = new NetworkCredential(smtpUsername, smtpPassword),
            EnableSsl = true
        };

        await client.SendMailAsync(message, cancellationToken);
    }
}