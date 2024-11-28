using System.Linq.Expressions;
using Askify.Core.Answers.Entities;

namespace Askify.Core.Answers.Repositories;

public interface IAnswerRepository
{
    Task AddAsync(Answer question, CancellationToken cancellationToken);
    Task<bool> AnyAsync(Expression<Func<Answer, bool>> predicate, CancellationToken cancellationToken);
    Task SaveChangesAsync(CancellationToken cancellationToken);
    Task DeleteAsync(Answer question);
    Task<IQueryable<Answer>> GetAll(Guid questionId, CancellationToken cancellationToken);
}