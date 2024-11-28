using Askify.Core.Questions.Entities;

namespace Askify.Infrastructure.EF.Questions.Configurations;

internal sealed class QuestionConfiguration : IEntityTypeConfiguration<Question>
{
    public void Configure(EntityTypeBuilder<Question> builder)
    {
        builder.HasKey(q => q.Id);

        builder.Property<string>("Title")
            .IsRequired()
            .HasMaxLength(250);

        builder.Property<string>("Content")
            .IsRequired();

        builder.Property<DateTimeOffset>("CreatedAt")
            .IsRequired();

        builder.HasOne(q => q.User)
            .WithMany(u => u.Questions)
            .HasForeignKey(q => q.UserId);

        builder.ToTable("Questions");
    }
}