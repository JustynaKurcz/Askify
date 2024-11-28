namespace Askify.Shared.Auth.Context;

public interface IIdentityContext
{
    bool IsAuthenticated { get; }
    public Guid Id { get; }
    string? Role { get; }
}