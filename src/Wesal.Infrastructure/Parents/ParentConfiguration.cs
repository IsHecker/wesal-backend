using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wesal.Domain.Entities.Parents;
using Wesal.Domain.Entities.Users;

namespace Wesal.Infrastructure.Parents;

internal sealed class ParentConfiguration : IEntityTypeConfiguration<Parent>
{
    public void Configure(EntityTypeBuilder<Parent> builder)
    {
        builder.HasOne<User>()
            .WithOne()
            .HasForeignKey<Parent>(parent => parent.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasIndex(parent => parent.Email).IsUnique();
        builder.HasIndex(parent => parent.NationalId).IsUnique();
    }
}