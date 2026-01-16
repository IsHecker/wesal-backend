using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wesal.Domain.Entities.FamilyCourts;
using Wesal.Domain.Entities.Users;

namespace Wesal.Infrastructure.FamilyCourts;

internal sealed class FamilyCourtConfiguration : IEntityTypeConfiguration<FamilyCourt>
{
    public void Configure(EntityTypeBuilder<FamilyCourt> builder)
    {
        builder.HasOne<User>()
            .WithOne()
            .HasForeignKey<FamilyCourt>(court => court.UserId);

        builder.HasIndex(court => court.Email).IsUnique();
    }
}