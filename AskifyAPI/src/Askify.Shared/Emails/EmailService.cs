using System.Net.Mail;

namespace Askify.Shared.Emails;

public class EmailService(IConfiguration configuration) : IEmailService
{
    private readonly string _emailTemplate = LoadEmailTemplate();

    private static string LoadEmailTemplate()
    {
        var assembly = Assembly.GetExecutingAssembly();
        var resourcePath = assembly.GetManifestResourceNames()
                               .FirstOrDefault(x => x.EndsWith("PasswordReset.html"))
                           ?? throw new InvalidOperationException("Could not find email template.");

        using var stream = assembly.GetManifestResourceStream(resourcePath)
                           ?? throw new InvalidOperationException("Could not load email template.");

        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }

    public async Task SendPasswordResetEmailAsync(string email, string token, CancellationToken cancellationToken)
    {
        var smtpServer = configuration["Email:SmtpHost"];
        var smtpPort = int.Parse(configuration["Email:SmtpPort"] ?? string.Empty);
        var smtpUsername = configuration["Email:Username"];
        var smtpPassword = configuration["Email:Password"];

        var baseUrl = configuration["BaseUrl"] ?? "http://localhost:5244";
        var resetLink = $"{baseUrl}/api/v1/users/reset-password/{Uri.EscapeDataString(token)}";

        var emailBody = _emailTemplate.Replace("{{resetLink}}", resetLink);

        var message = new MailMessage
        {
            From = new MailAddress(smtpUsername!),
            Subject = "Reset hasła",
            Body = emailBody,
            IsBodyHtml = true
        };
        message.To.Add(email);

        using var client = new SmtpClient(smtpServer, smtpPort);
        client.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
        client.EnableSsl = true;

        await client.SendMailAsync(message, cancellationToken);
    }
}