using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wesal.Domain.Entities.CourtCases;
using Wesal.Domain.Entities.Families;
using Wesal.Domain.Entities.Parents;
using Wesal.Domain.Entities.VisitationLocations;
using Wesal.Domain.Entities.VisitationSchedules;

namespace Wesal.Infrastructure.VisitationSchedules;

internal sealed class VisitationScheduleConfiguration : IEntityTypeConfiguration<VisitationSchedule>
{
    public void Configure(EntityTypeBuilder<VisitationSchedule> builder)
    {
        builder.HasOne<CourtCase>()
            .WithMany()
            .HasForeignKey(courtCase => courtCase.CourtCaseId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne<Family>()
            .WithMany()
            .HasForeignKey(visit => visit.FamilyId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne<Parent>()
            .WithMany()
            .HasForeignKey(visit => visit.ParentId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne<VisitationLocation>()
            .WithMany()
            .HasForeignKey(courtCase => courtCase.LocationId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}