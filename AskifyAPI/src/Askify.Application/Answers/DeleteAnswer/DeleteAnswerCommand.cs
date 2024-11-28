using MediatR;

namespace Askify.Application.Answers.DeleteAnswer;

public record DeleteAnswerCommand(Guid QuestionId, Guid AnswerId) : IRequest;