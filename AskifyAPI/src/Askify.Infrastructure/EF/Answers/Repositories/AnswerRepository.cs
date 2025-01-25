using Askify.Core.Answers.Entities;
using Askify.Core.Answers.Repositories;

namespace Askify.Infrastructure.EF.Answers.Repositories;

internal sealed class AnswerRepository(AskifyDbContext dbContext) : IAnswerRepository
{
    private readonly DbSet<Answer> _answers = dbContext.Answers;

    public async Task AddAsync(Answer answer, CancellationToken cancellationToken)
        => await _answers.AddAsync(answer, cancellationToken);

    public async Task<bool> AnyAsync(Expression<Func<Answer, bool>> predicate, CancellationToken cancellationToken)
        => await _answers.AnyAsync(predicate, cancellationToken);

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
        => await dbContext.SaveChangesAsync(cancellationToken);

    public async Task DeleteAsync(Answer answer)
        => await Task.FromResult(_answers.Remove(answer));

    public async Task<IQueryable<Answer>> GetAnswerForQuestion(Guid questionId, CancellationToken cancellationToken)
        => await Task.FromResult(_answers
            .AsNoTracking()
            .Where(x => x.QuestionId == questionId)
            .AsSplitQuery()
            .AsQueryable());

    public async Task<Answer?> GetAsync(Guid answerId, bool asNoTracking, CancellationToken cancellationToken)
    {
        var query = _answers.AsQueryable();

        if (asNoTracking)
        {
            query = query.AsNoTracking();
        }

        return await query
            .AsSplitQuery()
            .FirstOrDefaultAsync(q => q.Id == answerId, cancellationToken);
    }
}