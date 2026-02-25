using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wesal.Domain.Entities.Children;
using Wesal.Domain.Entities.Families;

namespace Wesal.Infrastructure.Families;

internal sealed class FamilyConfiguration : IEntityTypeConfiguration<Family>
{
    public void Configure(EntityTypeBuilder<Family> builder)
    {
        builder.HasOne(family => family.Father)
            .WithMany()
            .HasForeignKey(family => family.FatherId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(family => family.Mother)
            .WithOne()
            .HasForeignKey<Family>(family => family.MotherId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasMany(family => family.Children)
            .WithOne()
            .HasForeignKey(child => child.FamilyId);
    }
}