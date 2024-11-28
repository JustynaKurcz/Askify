namespace Askify.Shared.Auth.Context;

internal  class Context : IContext
{
    public IIdentityContext Identity { get; }

    internal Context()
    {
    }

    public Context(HttpContext context) : this(new IdentityContext(context.User))
    {
    }

    internal Context(IIdentityContext identity) => Identity = identity;

    public static IContext Empty => new Context();
}