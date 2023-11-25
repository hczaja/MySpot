using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MySpot.Core.Entities;
using MySpot.Core.ValueObjects;

namespace MySpot.Infrastructure.DAL.Configurations;

internal sealed class UserConfiguration
    : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasConversion(x => x.Value, x => new UserId(x));
        builder.HasIndex(x => x.Email).IsUnique();
        builder.Property(x => x.Email)
            .HasConversion(x => x.Value, x => new Email(x))
            .IsRequired()
            .HasMaxLength(100);
        builder.HasIndex(x => x.Username).IsUnique();
        builder.Property(x => x.Username)
            .IsRequired()
            .HasConversion(x => x.Value, x => new Username(x));
        builder.Property(x => x.Password)
            .IsRequired()
            .HasConversion(x => x.Value, x => new Password(x));
        builder.Property(x => x.FullName)
            .IsRequired()
            .HasConversion(x => x.Value, x => new FullName(x));
        builder.Property(x => x.Role)
            .IsRequired()
            .HasConversion(x => x.Value, x => new Role(x));
        builder.Property(x => x.CreatedAt).IsRequired();
    }
}
