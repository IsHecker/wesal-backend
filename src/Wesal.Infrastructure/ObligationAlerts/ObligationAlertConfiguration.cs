using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wesal.Domain.Entities.FamilyCourts;
using Wesal.Domain.Entities.ObligationAlerts;
using Wesal.Domain.Entities.Parents;

namespace Wesal.Infrastructure.ObligationAlerts;

internal sealed class ObligationAlertConfiguration : IEntityTypeConfiguration<ObligationAlert>
{
    public void Configure(EntityTypeBuilder<ObligationAlert> builder)
    {
        builder.HasOne<Parent>()
            .WithMany()
            .HasForeignKey(alert => alert.ParentId);

        builder.HasOne<FamilyCourt>()
            .WithMany()
            .HasForeignKey(alert => alert.CourtId);

        builder.HasOne(alert => alert.AssignedStaff)
            .WithMany()
            .HasForeignKey(alert => alert.AssignedStaffId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}