using Askify.Core.Questions.Entities;
using Askify.Core.Questions.Repositories;

namespace Askify.Infrastructure.EF.Questions.Repositories;

internal sealed class QuestionRepository(AskifyDbContext dbContext) : IQuestionRepository
{
    private readonly DbSet<Question> _questions = dbContext.Questions;

    public async Task AddAsync(Question question, CancellationToken cancellationToken)
        => await _questions.AddAsync(question, cancellationToken);

    public Task<bool> AnyAsync(Expression<Func<Question, bool>> predicate,
        CancellationToken cancellationToken)
        => _questions.AnyAsync(predicate, cancellationToken);

    public Task SaveChangesAsync(CancellationToken cancellationToken)
        => dbContext.SaveChangesAsync(cancellationToken);

    public async Task<Question?> GetAsync(Guid questionId, bool asNoTracking, CancellationToken cancellationToken)
    {
        var query = _questions.AsQueryable();

        if (asNoTracking)
        {
            query = query.AsNoTracking();
        }

        return await query
            .Include(q => q.User)
            .AsSplitQuery()
            .FirstOrDefaultAsync(q => q.Id == questionId, cancellationToken);
    }

    public async Task DeleteAsync(Question question)
        => _questions.Remove(question);

    public async Task<IQueryable<Question>> GetAll(CancellationToken cancellationToken)
        => _questions
            .Include(q => q.User)
            .AsSplitQuery()
            .AsQueryable();
}