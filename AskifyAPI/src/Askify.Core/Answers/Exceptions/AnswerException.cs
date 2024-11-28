using Askify.Shared.Results;

namespace Askify.Core.Answers.Exceptions;

public sealed class AnswerException
{
    public sealed class AnswerNotFoundException : AskifyException
    {
        public AnswerNotFoundException(Guid answerId) : base($"Answer with ID {answerId} not found.")
        {
        }
    }

    public sealed class AnswerNotOwnedByUser : AskifyException
    {
        public AnswerNotOwnedByUser(Guid answerId) : base(
            $"Answer with ID {answerId} is not owned by the user.")
        {
        }
    }
}