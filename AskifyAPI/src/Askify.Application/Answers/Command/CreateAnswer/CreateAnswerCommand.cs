using MediatR;

namespace Askify.Application.Answers.CreateAnswer;

internal record CreateAnswerCommand(string Content)
    : IRequest<CreateAnswerResponse>
{
    internal Guid QuestionId { get; init; }
}