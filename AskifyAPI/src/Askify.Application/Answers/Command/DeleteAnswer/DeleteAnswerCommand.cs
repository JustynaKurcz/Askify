namespace Askify.Application.Answers.Command.DeleteAnswer;

public record DeleteAnswerCommand(Guid QuestionId, Guid AnswerId) : IRequest;