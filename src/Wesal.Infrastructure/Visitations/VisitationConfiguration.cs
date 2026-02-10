using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wesal.Domain.Entities.Families;
using Wesal.Domain.Entities.Parents;
using Wesal.Domain.Entities.VisitationLocations;
using Wesal.Domain.Entities.Visitations;
using Wesal.Domain.Entities.VisitCenterStaffs;

namespace Wesal.Infrastructure.Visitations;

internal sealed class VisitationConfiguration : IEntityTypeConfiguration<Visitation>
{
    public void Configure(EntityTypeBuilder<Visitation> builder)
    {
        builder.HasOne<Family>()
            .WithMany()
            .HasForeignKey(visit => visit.FamilyId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne<Parent>()
            .WithMany()
            .HasForeignKey(visit => visit.NonCustodialParentId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne<VisitationLocation>()
            .WithMany()
            .HasForeignKey(visit => visit.LocationId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(visit => visit.VisitationSchedule)
            .WithMany()
            .HasForeignKey(visit => visit.VisitationScheduleId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne<VisitCenterStaff>()
            .WithMany()
            .HasForeignKey(visit => visit.VerifiedById)
            .OnDelete(DeleteBehavior.NoAction);
    }
}