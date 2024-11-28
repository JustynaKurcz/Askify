using Askify.Core.Answers.Entities;
using Askify.Core.Questions.Entities;
using Askify.Core.Users.Entities;

namespace Askify.Infrastructure.EF.DbContext;

internal sealed class AskifyDbContext(DbContextOptions<AskifyDbContext> options)
    : Microsoft.EntityFrameworkCore.DbContext(options)
{
    public DbSet<User> Users { get; init; }
    public DbSet<Question> Questions { get; init; }
    public DbSet<Answer> Answers { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}