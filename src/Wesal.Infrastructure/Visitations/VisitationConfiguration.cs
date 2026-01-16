using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wesal.Domain.Entities.CourtStaffs;
using Wesal.Domain.Entities.Families;
using Wesal.Domain.Entities.Parents;
using Wesal.Domain.Entities.VisitationLocations;
using Wesal.Domain.Entities.Visitations;
using Wesal.Domain.Entities.VisitationSchedules;

namespace Wesal.Infrastructure.Visitations;

internal sealed class VisitationConfiguration : IEntityTypeConfiguration<Visitation>
{
    public void Configure(EntityTypeBuilder<Visitation> builder)
    {
        // builder.HasOne<Family>()
        //     .WithMany()
        //     .HasForeignKey(visit => visit.FamilyId)
        //     .OnDelete(DeleteBehavior.NoAction);

        // builder.HasOne<Parent>()
        //     .WithMany()
        //     .HasForeignKey(visit => visit.ParentId)
        //     .OnDelete(DeleteBehavior.NoAction);

        // builder.HasOne<VisitationLocation>()
        //     .WithMany()
        //     .HasForeignKey(visit => visit.LocationId)
        //     .OnDelete(DeleteBehavior.NoAction);

        // builder.HasOne<VisitationSchedule>()
        //     .WithMany()
        //     .HasForeignKey(visit => visit.VisitationScheduleId)
        //     .OnDelete(DeleteBehavior.NoAction);

        // builder.HasOne<CourtStaff>()
        //     .WithMany()
        //     .HasForeignKey(visit => visit.VerifiedById)
        //     .OnDelete(DeleteBehavior.NoAction);
    }
}