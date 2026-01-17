using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wesal.Domain.Entities.FamilyCourts;
using Wesal.Domain.Entities.VisitationLocations;

namespace Wesal.Infrastructure.VisitationLocations;

internal sealed class VisitationLocationConfiguration : IEntityTypeConfiguration<VisitationLocation>
{
    public void Configure(EntityTypeBuilder<VisitationLocation> builder)
    {
        builder.HasOne<FamilyCourt>()
            .WithMany()
            .HasForeignKey(location => location.CourtId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasIndex(location => new { location.Name, location.Governorate })
            .IsUnique();
    }
}