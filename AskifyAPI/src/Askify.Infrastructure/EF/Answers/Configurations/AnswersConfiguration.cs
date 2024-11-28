using Askify.Core.Answers.Entities;

namespace Askify.Infrastructure.EF.Answers.Configurations;

internal sealed class AnswersConfiguration : IEntityTypeConfiguration<Answer>
{
    public void Configure(EntityTypeBuilder<Answer> builder)
    {
        builder.HasKey(a => a.Id);

        builder.Property<string>("Content")
            .IsRequired();

        builder.Property<DateTimeOffset>("CreatedAt")
            .IsRequired();

        builder.Property<Guid>("UserId")
            .IsRequired();

        builder.Property<Guid>("QuestionId")
            .IsRequired();

        builder.HasOne(a => a.Question)
            .WithMany(q => q.Answers)
            .HasForeignKey(a => a.QuestionId);

        builder.ToTable("Answers");
    }
}