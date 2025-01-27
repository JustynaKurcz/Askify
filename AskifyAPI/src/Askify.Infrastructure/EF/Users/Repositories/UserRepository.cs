using Askify.Core.Users.Entities;
using Askify.Core.Users.Enums;
using Askify.Core.Users.Repositories;

namespace Askify.Infrastructure.EF.Users.Repositories;

internal sealed class UserRepository(AskifyDbContext dbContext) : IUserRepository
{
    private readonly DbSet<User> _users = dbContext.Users;

    public async Task AddAsync(User user, CancellationToken cancellationToken)
        => await _users.AddAsync(user, cancellationToken);

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
        => await dbContext.SaveChangesAsync(cancellationToken);

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken)
        => await _users
            .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);

    public async Task<bool> AnyAsync(Expression<Func<User, bool>> predicate, CancellationToken cancellationToken)
        => await _users.AnyAsync(predicate, cancellationToken);

    public async Task<User?> GetAsync(Guid id, CancellationToken cancellationToken)
        => await _users
            .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);

    public async Task<string> GetUserName(Guid id, CancellationToken cancellationToken)
        => await _users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == id, cancellationToken)
            .ContinueWith(u => u.Result?.UserName ?? string.Empty, cancellationToken);

    public async Task DeleteAsync(User user)
        => await Task.FromResult(_users.Remove(user));

    public async Task<IQueryable<User>> GetAll(CancellationToken cancellationToken)
        => await Task.FromResult(_users
            .AsNoTracking()
            .Where(x => x.Role != Role.Admin)
            .AsSplitQuery()
            .AsQueryable());
}