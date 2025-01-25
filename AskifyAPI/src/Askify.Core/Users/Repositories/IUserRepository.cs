using System.Linq.Expressions;
using Askify.Core.Users.Entities;

namespace Askify.Core.Users.Repositories;

public interface IUserRepository
{
    Task AddAsync(User user, CancellationToken cancellationToken);
    Task SaveChangesAsync(CancellationToken cancellationToken);
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken);
    Task<bool> AnyAsync(Expression<Func<User, bool>> predicate, CancellationToken cancellationToken);
    Task<User?> GetAsync(Guid id, CancellationToken cancellationToken);
    Task<string> GetUserName(Guid id, CancellationToken cancellationToken);

    Task DeleteAsync(User user);
    Task<IQueryable<User>> GetAll(CancellationToken cancellationToken);
}