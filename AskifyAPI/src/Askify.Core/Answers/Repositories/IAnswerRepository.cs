using System.Linq.Expressions;
using Askify.Core.Answers.Entities;

namespace Askify.Core.Answers.Repositories;

public interface IAnswerRepository
{
    Task AddAsync(Answer answer, CancellationToken cancellationToken);
    Task<bool> AnyAsync(Expression<Func<Answer, bool>> predicate, CancellationToken cancellationToken);
    Task SaveChangesAsync(CancellationToken cancellationToken);
    Task DeleteAsync(Answer answer);
    Task<IQueryable<Answer>> GetAnswerForQuestion(Guid questionId, CancellationToken cancellationToken);
    Task<Answer?> GetAsync(Guid answerId, bool asNoTracking, CancellationToken cancellationToken);
}