using Askify.Shared.Results;

namespace Askify.Core.Questions.Exceptions;

public class QuestionException
{
    public sealed class QuestionExistsForUser : AskifyException
    {
        public QuestionExistsForUser() : base("Question already exists for the user.")
        {
        }
    }

    public sealed class QuestionNotFoundException : AskifyException
    {
        public QuestionNotFoundException(Guid questionId) : base($"Question with ID {questionId} not found.")
        {
        }
    }

    public sealed class QuestionCannotBeChanged : AskifyException
    {
        public QuestionCannotBeChanged() : base("Question cannot be changed after 30 minutes.")
        {
        }
    }

    public sealed class QuestionNotBelongToUser : AskifyException
    {
        public QuestionNotBelongToUser() : base("Question does not belong to the user.")
        {
        }
    }

    public sealed class QuestionNotOwnedByUser : AskifyException
    {
        public QuestionNotOwnedByUser(Guid questionId) : base(
            $"Question with ID {questionId} is not owned by the user.")
        {
        }
    }

    public class CannotDeleteQuestionException : AskifyException
    {
        public CannotDeleteQuestionException() : base("Cannot delete the question.")
        {
        }
    }
}