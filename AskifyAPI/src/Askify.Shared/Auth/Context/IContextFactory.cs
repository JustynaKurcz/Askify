namespace Askify.Shared.Auth.Context;

internal interface IContextFactory
{
    IContext Create();
}