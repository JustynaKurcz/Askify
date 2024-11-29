using System.Net.Mail;
using Microsoft.Extensions.Options;

namespace Askify.Shared.Emails;

public sealed class EmailService(IOptions<EmailOptions> emailOptions) 
    : IEmailService
{
    private readonly string _emailTemplate = LoadEmailTemplate();
    private readonly EmailOptions _emailOptions = emailOptions.Value;
    private const string TemplateName = "PasswordReset.html";

    public async Task SendPasswordResetEmailAsync(string email, string token, CancellationToken cancellationToken)
    {
        var message = CreatePasswordResetMessage(email, token);
        await SendEmailAsync(message, cancellationToken);
    }
    
    private MailMessage CreatePasswordResetMessage(string email, string token)
    {
        var resetLink = $"{_emailOptions.BaseUrl}/api/v1/users/reset-password/{Uri.EscapeDataString(token)}";
        var emailBody = _emailTemplate.Replace("{{resetLink}}", resetLink);

        return new MailMessage
        {
            From = new MailAddress(_emailOptions.Username),
            Subject = "Reset hasła",
            Body = emailBody,
            IsBodyHtml = true,
            To = { email }
        };
    }

    private async Task SendEmailAsync(MailMessage message, CancellationToken cancellationToken)
    {
        using var client = CreateSmtpClient();
        await client.SendMailAsync(message, cancellationToken);
    }

    private SmtpClient CreateSmtpClient()
    {
        return new SmtpClient(_emailOptions.SmtpHost, _emailOptions.SmtpPort)
        {
            Credentials = new NetworkCredential(_emailOptions.Username, _emailOptions.Password),
            EnableSsl = true
        };
    }
    
    private static string LoadEmailTemplate()
    {
        var assembly = Assembly.GetExecutingAssembly();
        var resourcePath = assembly.GetManifestResourceNames()
                               .FirstOrDefault(x => x.EndsWith(TemplateName))
                           ?? throw new InvalidOperationException("Could not find email template.");

        using var stream = assembly.GetManifestResourceStream(resourcePath)
                           ?? throw new InvalidOperationException("Could not load email template.");

        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }
}