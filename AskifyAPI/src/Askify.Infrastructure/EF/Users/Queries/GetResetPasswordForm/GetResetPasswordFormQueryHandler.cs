using Askify.Application.Users.Queries.GetResetPasswordForm;
using Askify.Core.Users.Exceptions;
using Askify.Core.Users.Repositories;
using Askify.Shared.Auth;

namespace Askify.Infrastructure.EF.Users.Queries.GetResetPasswordForm;

public class GetResetPasswordFormQueryHandler(
    IAuthManager authManager,
    IUserRepository userRepository
) : IRequestHandler<GetResetPasswordFormQuery, string>
{
    private const string ResetPasswordFormPath = "Askify.Shared.Emails.Templates.ResetPasswordForm.html";

    public async Task<string> Handle(GetResetPasswordFormQuery query, CancellationToken cancellationToken)
    {
        if (!authManager.VerifyPasswordResetToken(query.Token, out var email))
        {
            throw new InvalidOperationException("Invalid password reset token.");
        }

        var user = await userRepository.GetByEmailAsync(email, cancellationToken)
            ?? throw new UserException.UserNotFoundException(email);
        

        var assembly = typeof(IAuthManager).Assembly;

        await using var stream = assembly.GetManifestResourceStream(ResetPasswordFormPath);
        if (stream == null)
        {
            throw new InvalidOperationException($"Could not find embedded resource '{ResetPasswordFormPath}'");
        }

        using var reader = new StreamReader(stream);
        var formHtml = await reader.ReadToEndAsync(cancellationToken);

        formHtml = formHtml.Replace("{token}", query.Token);
        formHtml = formHtml.Replace("{email}", user.Email);

        return formHtml;
    }
}