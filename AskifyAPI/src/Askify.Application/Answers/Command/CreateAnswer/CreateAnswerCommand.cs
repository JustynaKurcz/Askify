namespace Askify.Application.Answers.Command.CreateAnswer;

internal record CreateAnswerCommand(string Content)
    : IRequest<CreateAnswerResponse>
{
    internal Guid QuestionId { get; init; }
}