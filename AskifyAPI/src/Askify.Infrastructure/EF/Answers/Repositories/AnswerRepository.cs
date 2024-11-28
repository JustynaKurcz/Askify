using System.Linq.Expressions;
using Askify.Core.Answers.Entities;
using Askify.Core.Answers.Repositories;
using Askify.Infrastructure.EF.DbContext;

namespace Askify.Infrastructure.EF.Answers.Repositories;

internal sealed class AnswerRepository(AskifyDbContext dbContext) : IAnswerRepository
{
    private readonly DbSet<Answer> _answers = dbContext.Answers;

    public async Task AddAsync(Answer question, CancellationToken cancellationToken)
        => await _answers.AddAsync(question, cancellationToken);

    public async Task<bool> AnyAsync(Expression<Func<Answer, bool>> predicate, CancellationToken cancellationToken)
        => await _answers.AnyAsync(predicate, cancellationToken);

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
        => await dbContext.SaveChangesAsync(cancellationToken);

    public async Task DeleteAsync(Answer question)
        => _answers.Remove(question);

    public async Task<IQueryable<Answer>> GetAll(Guid questionId, CancellationToken cancellationToken)
        => _answers
            .Where(x => x.QuestionId == questionId)
            .AsSplitQuery()
            .AsQueryable();
}