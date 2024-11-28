using System.Linq.Expressions;
using Askify.Core.Questions.Entities;

namespace Askify.Core.Questions.Repositories;

public interface IQuestionRepository
{
    Task AddAsync(Question question, CancellationToken cancellationToken);
    Task<bool> AnyAsync(Expression<Func<Question, bool>> predicate, CancellationToken cancellationToken);
    Task SaveChangesAsync(CancellationToken cancellationToken);
    Task<Question?> GetAsync(Guid questionId, bool asNoTracking, CancellationToken cancellationToken);
    Task DeleteAsync(Question question);
    Task<IQueryable<Question>> GetAll(CancellationToken cancellationToken);
}