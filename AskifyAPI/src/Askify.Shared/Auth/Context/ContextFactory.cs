namespace Askify.Shared.Auth.Context;

internal class ContextFactory(IHttpContextAccessor httpContextAccessor) : IContextFactory
{
    public IContext Create()
    {
        var httpContext = httpContextAccessor.HttpContext;

        return httpContext is null ? Context.Empty : new Context(httpContext);
    }
}