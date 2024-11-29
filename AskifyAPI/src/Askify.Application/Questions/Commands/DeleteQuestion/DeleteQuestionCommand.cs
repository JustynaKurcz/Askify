namespace Askify.Application.Questions.Commands.DeleteQuestion;

internal record DeleteQuestionCommand(Guid QuestionId) : IRequest;