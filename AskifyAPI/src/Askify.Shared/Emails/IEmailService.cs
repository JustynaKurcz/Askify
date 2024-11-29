namespace Askify.Shared.Emails;

public interface IEmailService
{
    Task SendPasswordResetEmailAsync(string email, string token, CancellationToken cancellationToken);
}