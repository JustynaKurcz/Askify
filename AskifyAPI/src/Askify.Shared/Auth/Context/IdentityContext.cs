namespace Askify.Shared.Auth.Context;

internal class IdentityContext : IIdentityContext
{
    public bool IsAuthenticated { get; }
    public Guid Id { get; }
    public string? Role { get; }

    public IdentityContext(ClaimsPrincipal principal)
    {
        IsAuthenticated = principal.Identity?.IsAuthenticated is true;
        Id = IsAuthenticated ? Guid.Parse(principal.Identity.Name) : Guid.Empty;
        Role = principal.Claims.SingleOrDefault(x => x.Type == ClaimTypes.Role)?.Value;
    }
}