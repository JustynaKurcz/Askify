using Askify.Core.Users.Entities;
using Askify.Core.Users.Enums;

namespace Askify.Infrastructure.EF.Users.Configurations;

internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);

        builder.HasIndex(x => x.Email).IsUnique();
        builder.Property<string>("Email")
            .IsRequired()
            .HasMaxLength(100);

        builder.HasIndex(x => x.UserName).IsUnique();
        builder.Property<string>("UserName")
            .IsRequired()
            .HasMaxLength(150);

        builder.Property<string>("Password")
            .IsRequired();

        builder.Property<DateTimeOffset>("CreatedAt")
            .IsRequired();

        builder.Property<DateTimeOffset?>("UpdatedAt")
            .ValueGeneratedOnUpdate();

        builder.Property<Role>("Role")
            .IsRequired();

        builder.ToTable("Users");
    }
}