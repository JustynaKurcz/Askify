using Askify.Shared.Results;

namespace Askify.Core.Questions.Errors;

public static class QuestionError
{
    public static Error QuestionExistsForUser => new
        ("Questions.QuestionExistsForUser", "Question already exists for the user.");

    public static Error QuestionNotFound(Guid questionId) => new
        ("Questions.QuestionNotFound", $"Question with ID {questionId} not found.");

    public static Error QuestionCannotBeChanged() => new
        ("Questions.QuestionCannotBeChanged", "Question cannot be changed after 30 minutes.");

    public static Error QuestionNotBelongToUser() => new
        ("Questions.QuestionNotBelongToUser", "Question does not belong to the user.");

    public static Error QuestionNotOwnedByUser(Guid questionId) => new
        ("Questions.QuestionNotOwnedByUser", $"Question with ID {questionId} is not owned by the user.");
}