namespace Askify.Shared.Auth;

public interface IAuthManager
{
    JsonWebToken GenerateToken(Guid userId, string role);
    string GeneratePasswordResetToken(string email);
    bool VerifyPasswordResetToken(string token, out string email);
}