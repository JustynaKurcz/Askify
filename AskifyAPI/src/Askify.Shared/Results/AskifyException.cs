namespace Askify.Shared.Results;

public abstract class AskifyException : Exception
{
    protected AskifyException(string message) : base(message)
    {
    }
}